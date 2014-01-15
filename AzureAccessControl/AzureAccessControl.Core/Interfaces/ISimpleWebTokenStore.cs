//---------------------------------------------------------------------------------
// Copyright 2013 Tomasz Cielecki (tomasz@ostebaronen.dk)
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

using Cheesebaron.MvxPlugins.SimpleWebToken.Interfaces;

namespace Cheesebaron.MvxPlugins.AzureAccessControl
{
    public interface ISimpleWebTokenStore
    {
        /// <summary>
        /// Gets or sets the configured SimpleWebToken
        /// </summary>
        ISimpleWebToken SimpleWebToken { get; set; }

        /// <summary>
        /// Checks if the Simple Web Token currently stored is valid
        /// </summary>
        /// <returns>Returns true if there is a SimpleWebToken in the store and it has not expired, 
        /// otherwise returns false</returns>
        bool IsValid();
    }
}
