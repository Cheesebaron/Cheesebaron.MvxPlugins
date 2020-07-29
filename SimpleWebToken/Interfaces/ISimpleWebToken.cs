using System;
using System.Collections.Generic;

namespace Cheesebaron.MvxPlugins.SimpleWebToken
{
    public interface ISimpleWebToken
    {
        /// <summary>
        /// Gets the Audience for the token.
        /// </summary>
        /// <value>The audience of the token.</value>
        string? Audience { get; }

        /// <summary>
        /// Gets the time when the token expires.
        /// </summary>
        /// <value>The time upto which the token is valid.</value>
        DateTime? ExpiresOn { get; }

        /// <summary>
        /// Gets the Issuer for the token.
        /// </summary>
        /// <value>The issuer for the token.</value>
        string? Issuer { get; }

        /// <summary>
        /// Gets the serialized form of the token if the token was created from its serialized form by the token handler.
        /// </summary>
        /// <value>The serialized form of the token.</value>
        string? RawToken { get; }

        /// <summary>
        /// Gets the signature for the token.
        /// </summary>
        /// <value>The signature for the token.</value>
        string? Signature { get; }

        string? IdentityProvider { get; }
        IDictionary<string, string>? Properties { get; }

        IEnumerable<string> Keys { get; }
        string? this[string key] { get; }

        ISimpleWebToken CreateTokenFromRaw(string rawToken);
        ISimpleWebToken CreateToken(
            string issuer, string audience, DateTime expiryTime, string signingKey,
            IEnumerable<KeyValuePair<string, string>>? values = null);

        bool ValidateSignature(string keyString);

        /// <summary>
        /// Generates an HMACSHA256 signature for a given string and key.
        /// </summary>
        /// <param name="unsignedToken">The token to be signed.</param>
        /// <param name="signingKey">The key used to generate the signature.</param>
        /// <returns>The generated signature.</returns>
        string GenerateSignature(string unsignedToken, byte[] signingKey);
    }
}
