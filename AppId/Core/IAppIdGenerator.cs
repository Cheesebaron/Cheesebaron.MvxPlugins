namespace Cheesebaron.MvxPlugins.AppId
{
    public interface IAppIdGenerator
    {
        /// <summary>
        /// Generates a an AppId optionally using the PhoneId a prefix and a suffix and a Guid to ensure uniqueness
        /// 
        /// The AppId format is as follows {prefix}guid{phoneid}{suffix}, where parts in {} are optional.
        /// </summary>
        /// <param name="usingPhoneId">Setting this to true adds the device specific id to the AppId (remember to give the app the correct permissions)</param>
        /// <param name="prefix">Sets the prefix of the AppId</param>
        /// <param name="suffix">Sets the suffix of the AppId</param>
        /// <returns></returns>
        string GenerateAppId(bool usingPhoneId = false, string prefix = null, string suffix = null);

        /// <summary>
        /// This is the device specific Id (remember the correct permissions in your app to use this)
        /// </summary>
        string PhoneId { get; }

        /// <summary>
        /// Gives you the phone model
        /// </summary>
        string PhoneModel { get; }

        /// <summary>
        /// Gives you the version of the OS on the device
        /// </summary>
        string OsVersion { get; }

        string Platform { get; }
    }
}
