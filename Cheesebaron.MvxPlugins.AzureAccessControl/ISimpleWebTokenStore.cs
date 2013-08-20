namespace Cheesebaron.MvxPlugins.AzureAccessControl
{
    public interface ISimpleWebTokenStore
    {
        /// <summary>
        /// Gets or sets the configured SimpleWebToken
        /// </summary>
        SimpleWebToken SimpleWebToken { get; set; }

        /// <summary>
        /// Checks if the Simple Web Token currently stored is valid
        /// </summary>
        /// <returns>Returns true if there is a SimpleWebToken in the store and it has not expired, 
        /// otherwise returns false</returns>
        bool IsValid();
    }
}
