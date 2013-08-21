// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious. No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Cheesebaron.MvxPlugins.AzureAccessControl
{
    /// <summary>
    /// Provides a class to wrap a SWT returned by ACS
    /// </summary>
    public class SimpleWebToken
    {
        /// <summary>
        /// The HTTP header name for the Audience token
        /// </summary>
        public const string AudienceTokenName = "Audience";

        /// <summary>
        /// The HTTP header name for the Issuer token
        /// </summary>
        public const string IssuerTokenName = "Issuer";

        /// <summary>
        /// The HTTP header name for the ExpiresOn token
        /// </summary>
        public const string ExpiresOnTokenName = "ExpiresOn";

        /// <summary>
        /// The HTTP header name for the HMAC SHA 256 token
        /// </summary>
        public const string HmacSha256TokenName = "HMACSHA256";

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleWebToken"/> class.
        /// </summary>
        /// <param name="rawToken">
        /// </param>
        public SimpleWebToken(string rawToken)
        {
            if (rawToken == null)
            {
                throw new ArgumentNullException("rawToken", "Raw token must not be null");
            }

            RawToken = rawToken;
            Parse();
        }

        /// <summary>
        /// Gets or sets the Audience from the token
        /// </summary>
        public string Audience { get; private set; }

        /// <summary>
        /// Gets or sets the token's Claims
        /// </summary>
        public IDictionary<string, string> Claims { get; private set; }

        /// <summary>
        /// Gets or sets the token's expire date
        /// </summary>
        public DateTime ExpiresOn { get; private set; }

        /// <summary>
        /// Gets or sets the token's Issuer
        /// </summary>
        public string Issuer { get; private set; }

        /// <summary>
        /// Gets or sets the token's RawToken
        /// </summary>
        public string RawToken { get; private set; }

        /// <summary>
        /// Gets the token's IdentityProvider
        /// </summary>
        public string IdentityProvider 
        {
            get
            {
                return Claims[ClaimTypes.IdentityProvider];
            } 
        }

        /// <summary>
        /// Gets the token's UserIdentifier
        /// </summary>
        public string NameIdentifier
        {
            get
            {
                return Claims[ClaimTypes.NameIdentifier];
            }
        }

        /// <summary>
        /// Provides a string representation of the token
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return RawToken;
        }

        /// <summary>
        /// Turns a UNIX epoch into a DateTime
        /// </summary>
        /// <param name="secondsSince1970"></param>
        /// <returns></returns>
        private static DateTime ToDateTimeFromEpoch(long secondsSince1970)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(secondsSince1970);
        }

        /// <summary>
        /// Parses the SWT to provide an easy access to its information and claims
        /// </summary>
        private void Parse()
        {
            Claims = new Dictionary<string, string>();

            foreach (var rawNameValue in RawToken.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (rawNameValue.StartsWith(string.Concat(HmacSha256TokenName, "="), StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var nameValue = rawNameValue.Split('=');

                if (nameValue.Length != 2)
                {
                    throw new FormatException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "Invalid token contains a name/value pair missing an = character: '{0}'",
                            rawNameValue));
                }

                var key = Uri.UnescapeDataString(nameValue[0]);
                var values = Uri.UnescapeDataString(nameValue[1]);

                switch (key)
                {
                    case IssuerTokenName:
                        Issuer = values;
                        break;
                    case AudienceTokenName:
                        Audience = values;
                        break;
                    case ExpiresOnTokenName:
                        ExpiresOn = ToDateTimeFromEpoch(long.Parse(values, CultureInfo.InvariantCulture));
                        break;
                    case ClaimTypes.IdentityProvider:
                        values = values.Replace('+', ' ');
                        Claims[key] = values;
                        break;
                    default:
                        Claims[key] = values;
                        break;
                }
            }
        }
    }
}
