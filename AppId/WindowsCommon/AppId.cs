using System;
using Windows.System.Profile;

namespace Cheesebaron.MvxPlugins.AppId.WindowsCommon
{
    public class AppId 
        : IAppIdGenerator
    {
        public string GenerateAppId(bool usingPhoneId = false, string prefix = null, string suffix = null)
        {
            var appId = "";

            if (!string.IsNullOrEmpty(prefix))
                appId += prefix;

            appId += Guid.NewGuid().ToString();

            if (usingPhoneId)
                appId += PhoneId;

            if (!string.IsNullOrEmpty(suffix))
                appId += suffix;

            return appId;
        }

        public string PhoneId
        {
            get
            {
                // http://www.wadewegner.com/2012/09/getting-the-application-id-and-hardware-id-in-windows-store-applications/
                var token = HardwareIdentification.GetPackageSpecificToken(null);
                var hardwareId = token.Id;
                var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);

                var bytes = new byte[hardwareId.Length];
                dataReader.ReadBytes(bytes);

                return Convert.ToBase64String(bytes);
            }
        }

        public string PhoneModel
        {
            get { return SystemInfoHelper.GetDeviceModelAsync().Result; }
        }

        public string OsVersion
        {
            get { return SystemInfoHelper.GetWindowsVersionAsync().Result; }
        }

        public string Platform
        {
            get { return "Windows 8.1 Universal"; }
        }
    }
}