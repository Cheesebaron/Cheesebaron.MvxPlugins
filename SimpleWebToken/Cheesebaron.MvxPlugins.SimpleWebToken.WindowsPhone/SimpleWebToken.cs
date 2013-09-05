//---------------------------------------------------------------------------------
// Copyright 2013 Bruel & Kjaer EMS
// Licensed under the Apache License, Version 2.0 (the "License"); 
// You may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 

// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, 
// INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR 
// CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, 
// MERCHANTABLITY OR NON-INFRINGEMENT. 

// See the Apache 2 License for the specific language governing 
// permissions and limitations under the License.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Cheesebaron.MvxPlugins.SimpleWebToken.Interfaces;

namespace Cheesebaron.MvxPlugins.SimpleWebToken
{
    public class SimpleWebToken
        : ISimpleWebToken
    {
        private static class SimpleWebTokenConstants
        {
            public const string Audience = "Audience";
            public const string ExpiresOn = "ExpiresOn";
            public const string Issuer = "Issuer";
            public const string Signature = "HMACSHA256";
        }

        public string Audience { get; private set; }
        public IDictionary<string, string> Properties { get; private set; }
        public DateTime ExpiresOn { get; private set; }
        public string Issuer { get; private set; }
        public string RawToken { get; private set; }
        public string Signature { get; private set; }

        public string IdentityProvider
        {
            get
            {
                if (Properties.Any() && Properties.ContainsKey(ClaimTypes.IdentityProvider))
                    return Properties[ClaimTypes.IdentityProvider];
                return null;
            }
        }

        public IEnumerable<string> Keys
        {
            get
            {
                return Properties != null ? Properties.Keys : new string[] { };
            }
        }

        public string this[string key]
        {
            get
            {
                return Properties != null ? Properties[key] : null;
            }
        }

        public SimpleWebToken(string rawToken)
        {
            if (string.IsNullOrEmpty(rawToken)) throw new ArgumentNullException("rawToken");

            RawToken = rawToken;
            Properties = new Dictionary<string, string>();

            foreach (var rawNameValue in rawToken.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var nameValue = rawNameValue.Split('=');

                if (nameValue.Length != 2)
                {
                    throw new FormatException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "Invalid token contains a name/value pair missing an = character: '{0}'",
                            rawNameValue));
                }

                var key = WebUtility.UrlDecode(nameValue[0]);
                var values = WebUtility.UrlDecode(nameValue[1]);

                switch(key)
                {
                    case SimpleWebTokenConstants.Issuer:
                        Issuer = values;
                        break;
                    case SimpleWebTokenConstants.Audience:
                        Audience = values;
                        break;
                    case SimpleWebTokenConstants.ExpiresOn:
                        ExpiresOn = GetTimeAsDateTime(string.IsNullOrEmpty(values) ? "0" : values);
                        break;
                    case SimpleWebTokenConstants.Signature:
                        Signature = values;
                        break;
                    default:
                        Properties[key] = values;
                        break;
                }
            }
        }

        public SimpleWebToken(string issuer, string audience, DateTime expiryTime, string signingKey,
            IEnumerable<KeyValuePair<string, string>> values = null)
        {
            if (string.IsNullOrEmpty(issuer)) throw new ArgumentNullException("issuer");
            if (string.IsNullOrEmpty(audience)) throw new ArgumentNullException("audience");
            if (string.IsNullOrEmpty(signingKey)) throw new ArgumentNullException("signingKey");

            if (expiryTime.Kind != DateTimeKind.Utc) throw new ArgumentOutOfRangeException("expiryTime", "Expiry time must be in UTC.");
            if (expiryTime < _swtBaseTime) throw new ArgumentOutOfRangeException("expiryTime", "Expiry time must be after 1970.");
            var signingKeyBytes = Convert.FromBase64String(signingKey);
            if (signingKeyBytes.Length != 32) throw new ArgumentOutOfRangeException("signingKey", "Signing key must be 32 bytes.");

            Issuer = issuer;
            Audience = audience;
            ExpiresOn = expiryTime;
            
            Properties = new Dictionary<string, string>();
            var sb = new StringBuilder();
            if (values != null)
            {
                foreach (var item in values)
                {
                    Properties.Add(item.Key, item.Value);
                    sb.AppendFormat("{0}={1}&", WebUtility.UrlEncode(item.Key), WebUtility.UrlEncode(item.Value));
                }
            }

            sb.AppendFormat("{0}={1}&", SimpleWebTokenConstants.Audience, WebUtility.UrlEncode(audience));
            sb.AppendFormat("{0}={1}&", SimpleWebTokenConstants.ExpiresOn, GetSwtTime(expiryTime));
            sb.AppendFormat("{0}={1}", SimpleWebTokenConstants.Issuer, WebUtility.UrlEncode(issuer));
            Signature = GenerateSignature(sb.ToString(), signingKeyBytes);
            sb.AppendFormat("&{0}={1}", SimpleWebTokenConstants.Signature, WebUtility.UrlEncode(Signature));

            RawToken = sb.ToString();
        }

        public ISimpleWebToken CreateTokenFromRaw(string rawToken)
        {
            return new SimpleWebToken(rawToken);
        }

        public ISimpleWebToken CreateToken(
            string issuer, string audience, DateTime expiryTime, string signingKey,
            IEnumerable<KeyValuePair<string, string>> values = null)
        {
            return new SimpleWebToken(issuer, audience, expiryTime, signingKey, values);
        }

        public bool ValidateSignature(string keyString)
        {
            if (keyString == null) throw new ArgumentNullException("keyString");

            if (string.IsNullOrEmpty(RawToken) || string.IsNullOrEmpty(RawToken))
                throw new Exception("The token does not have a signature to verify");

            var serializedToken = RawToken;
            string unsignedToken = null;

            // Find the last parameter. The signature must be last per SWT specification.
            var lastSeparator = serializedToken.LastIndexOf(ParameterSeparator);

            // Check whether the last parameter is an hmac.
            if (lastSeparator > 0)
            {
                var lastParamStart = ParameterSeparator + SimpleWebTokenConstants.Signature + "=";
                var lastParam = serializedToken.Substring(lastSeparator);

                // Strip the trailing hmac to obtain the original unsigned string for later hmac verification.               
                if (lastParam.StartsWith(lastParamStart, StringComparison.Ordinal))
                {
                    unsignedToken = serializedToken.Substring(0, lastSeparator);
                }
            }
            var generatedSignature = GenerateSignature(unsignedToken, Convert.FromBase64String(keyString));

            return string.CompareOrdinal(generatedSignature, Signature) == 0;
        }

        public string GenerateSignature(string unsignedToken, byte[] signingKey)
        {
            //HMACSHA256 is platform dependent, hence the need for WP/Droid/Touch plugins
            using (var hmac = new HMACSHA256(signingKey))
            {
                var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(unsignedToken));
                return Convert.ToBase64String(signatureBytes);
            }
        }

        private const char ParameterSeparator = '&';
        private static DateTime _swtBaseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Turns a UNIX epoch into a DateTime
        /// </summary>
        /// <param name="secondsSince1970"></param>
        /// <returns></returns>
        private static DateTime ToDateTimeFromEpoch(long secondsSince1970)
        {
            return _swtBaseTime.AddSeconds(secondsSince1970);
        }

        private static long GetSwtTime(DateTime time)
        {
            var dt = time - _swtBaseTime;
            return (long)dt.TotalSeconds;
        }

        /// <summary>
        /// Convert the time in seconds to a <see cref="DateTime"/> object based on the base time 
        /// defined by the Simple Web Token.
        /// </summary>
        /// <param name="expiryTime">The time in seconds.</param>
        /// <returns>The time as a <see cref="DateTime"/> object.</returns>
        private static DateTime GetTimeAsDateTime(string expiryTime)
        {
            long totalSeconds;
            if (!long.TryParse(expiryTime, out totalSeconds))
            {
                throw new ArgumentOutOfRangeException("expiryTime", "Invalid expiry time. Expected the time to be in seconds passed from 1 January 1970.");
            }

            var maxSeconds = (long)(DateTime.MaxValue - _swtBaseTime).TotalSeconds - 1;
            if (totalSeconds > maxSeconds)
            {
                totalSeconds = maxSeconds;
            }

            return ToDateTimeFromEpoch(totalSeconds);
        }
    }
}
