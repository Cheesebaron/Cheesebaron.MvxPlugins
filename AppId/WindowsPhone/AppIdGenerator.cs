using System;
using Microsoft.Phone.Info;

namespace Cheesebaron.MvxPlugins.AppId
{
    public class AppIdGenerator : IAppIdGenerator
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
                object uniqueId;
                if (!DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
                    return "";
                var bId = (byte[])uniqueId;
                return Convert.ToBase64String(bId);
            }
        }

        public string PhoneModel
        {
            get
            {
                object model;
                if (!DeviceExtendedProperties.TryGetValue("DeviceName", out model))
                    return "";
                return model as string;
            }
        }

        public string OsVersion
        {
            get
            {
                return "WindowsPhone " + Environment.OSVersion;
            }
        }
    }
}
