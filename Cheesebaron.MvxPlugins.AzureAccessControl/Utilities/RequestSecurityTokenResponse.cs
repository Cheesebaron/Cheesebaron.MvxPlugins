// Modified by Tomasz Cielecki (tomasz@ostebaronen.dk) 2013

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
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

//---------------------------------------------------------------------------------
// Copyright 2010 Microsoft Corporation
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
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.Utilities
{
    /// <summary>
    /// Contains the data returned in a RequestSecurityTokenResponse
    /// </summary>
    [DataContract]
    public class RequestSecurityTokenResponse
    {
        private const string WsSecuritySecExtNamespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        private const string BinarySecurityTokenName = "BinarySecurityToken";

        public RequestSecurityTokenResponse()
        {
            SecurityToken = null;
            TokenType = null;
            Expires = 0;
            Created = 0;
        }

        /// <summary>
        /// The raw string value of the security token contained in the RequestSecurityTokenResponse
        /// </summary>
        [DataMember]
        public string SecurityToken { get; set; }

        /// <summary>
        /// The uri which uniquely identifies the type of token contained in the RequestSecurityTokenResponse
        /// </summary>
        [DataMember]
        public string TokenType { get; set; }

        /// <summary>
        /// The expiration time of the token in the RequestSecurityTokenResponse
        /// </summary>
        [DataMember]
        public long Expires { get; set; }

        /// <summary>
        /// The creation time of the token in the RequestSecurityTokenResponse
        /// </summary>
        [DataMember]
        public long Created { get; set; }

        internal static async Task<RequestSecurityTokenResponse> FromJSON(string jsonRequestSecurityTokenService)
        {
            RequestSecurityTokenResponse returnToken;

            using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonRequestSecurityTokenService)))
            using(var str = new StreamReader(stream))
            {
                var mystr = await str.ReadToEndAsync();
                returnToken = JsonConvert.DeserializeObject<RequestSecurityTokenResponse>(mystr);

                returnToken.SecurityToken = Uri.UnescapeDataString(returnToken.SecurityToken);
            }
                
            using (var sr = new StringReader(returnToken.SecurityToken))
            using (var reader = XmlReader.Create(sr))
            {
                reader.MoveToContent();
                var binaryToken = reader.ReadElementContentAsString(BinarySecurityTokenName, WsSecuritySecExtNamespace);
                var tokenBytes = Convert.FromBase64String(binaryToken);
                returnToken.SecurityToken = Encoding.UTF8.GetString(tokenBytes, 0, tokenBytes.Length);
            }

            return returnToken;
        }

        public bool IsExpired
        {
            get
            {
                bool result;
                if (Expires <= 0) return true;
                var now = ConvertToUnixTimestamp(DateTime.UtcNow);
                var diff = now - Expires;

                result = diff >= 0;
                return result;
            }
        }

        static long ConvertToUnixTimestamp(DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var diff = date - origin;
            return (long)Math.Floor(diff.TotalSeconds);
        }

        public override string ToString()
        {
            return string.Format("Created {0}\nExpires {1}\nIsExpired {2}\nType {3}\nToken {4}", Created, Expires,
                IsExpired, TokenType, SecurityToken);
        }
    }
}
