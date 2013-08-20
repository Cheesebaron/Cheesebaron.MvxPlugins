using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cheesebaron.MvxPlugins.AzureAccessControl
{
    public interface IIdentityProviderClient
    {
        /// <summary>
        /// Retrieve enumeration of awailable Identity providers for your configuration
        /// </summary>
        /// <param name="identityProviderListServiceEndpoint">Uri for </param>
        /// <returns></returns>
        Task<IEnumerable<IdentityProviderInformation>> GetIdentityProviderListAsync(
            Uri identityProviderListServiceEndpoint);

        /// <summary>
        /// Get the default Identity Provider List Service Endpoint from the realm and namespace configured in
        /// Windows Azure Access Control Service.
        /// </summary>
        /// <param name="realm">Your Windows Azure Access Control Service realm</param>
        /// <param name="nameSpace">Your Windows Azure Access Control Service namespace</param>
        /// <returns></returns>
        Uri GetDefaultIdentityProviderListServiceEndpoint(string realm, string nameSpace);
    }
}
