using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace SimpleWebToken.WindowsCommon.Test
{
    [TestClass]
    public class SimpleWebTokenTests
    {
        [TestMethod]
        public void FromValidEncoded()
        {
            const string tokenString = "http%3a%2f%2fschemas.xmlsoap.org%2fws%2f2005%2f05%2fidentity%2fclaims%2fnameidentifier=upwJyUGTUPWMEiU3ds61jcsJUeUFjRza6sNEcdyNYnw%3d&http%3a%2f%2fschemas.microsoft.com%2faccesscontrolservice%2f2010%2f07%2fclaims%2fidentityprovider=uri%3aWindowsLiveID&Audience=http%3a%2f%2fnoisesentinel-dev-adminapi.azurewebsites.net%2f&ExpiresOn=1356650451&Issuer=https%3a%2f%2fbruelandkjaer.accesscontrol.windows.net%2f&HMACSHA256=npM6PtfuNUtG6EJ1gpS0s9rVvEx%2buP4UIXe3GB1t4CM%3d";
            var swt = new Cheesebaron.MvxPlugins.SimpleWebToken.WindowsCommon.SimpleWebToken().CreateTokenFromRaw(tokenString);
            Assert.AreEqual(tokenString, swt.RawToken);
            Assert.AreEqual(new DateTime(2012, 12, 27, 23, 20, 51, DateTimeKind.Utc), swt.ExpiresOn);
            Assert.AreEqual("https://bruelandkjaer.accesscontrol.windows.net/", swt.Issuer);
            Assert.AreEqual("http://noisesentinel-dev-adminapi.azurewebsites.net/", swt.Audience);
            Assert.AreEqual("uri:WindowsLiveID", swt["http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider"]);
            Assert.AreEqual("upwJyUGTUPWMEiU3ds61jcsJUeUFjRza6sNEcdyNYnw=", swt["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]);
            Assert.AreEqual("npM6PtfuNUtG6EJ1gpS0s9rVvEx+uP4UIXe3GB1t4CM=", swt.Signature);
            Assert.IsTrue(swt.ValidateSignature("eP+VgZq3YVUXSDt71lKnCRoxdoGngCT9WR4vTprH9TY="));
            Assert.IsFalse(swt.ValidateSignature("nh0BPopBTc7MAzviohoUbWNhO6NlV+LYm+sXOTPULxk="));
            Assert.IsNotNull(swt);
            Assert.AreEqual(2, swt.Keys.Count());
            Assert.IsTrue(swt.Keys.Contains("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider"));
            Assert.IsTrue(swt.Keys.Contains("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
        }

        [TestMethod]
        public void NewToken()
        {
            const string issuer = "http://unittest-ip.com/";
            const string audience = "http://unittest-rt.com/";
            var expiresOn = new DateTime(2034, 4, 7, 23, 12, 34, DateTimeKind.Utc);
            const string signingKey = "nh0BPopBTc7MAzviohoUbWNhO6NlV+LYm+sXOTPULxk=";

            var swt = new Cheesebaron.MvxPlugins.SimpleWebToken.WindowsCommon.SimpleWebToken().CreateToken(issuer, audience, expiresOn, signingKey);
            Assert.AreEqual(expiresOn, swt.ExpiresOn);
            Assert.AreEqual(issuer, swt.Issuer);
            Assert.AreEqual(audience, swt.Audience);
            Assert.IsNotNull(swt.RawToken);
            Assert.IsNotNull(swt.Signature);
            Assert.IsTrue(swt.ValidateSignature(signingKey));
            Assert.AreEqual(0, swt.Keys.Count());
        }

        [TestMethod]
        public void NewTokenWithArgs()
        {
            var args = new Dictionary<string, string>
            {
                {"alfa", "beta"}, 
                {"http://unittest-ip.com/alfa", "fisk & fugl"}
            };

            const string issuer = "http://unittest-ip.com/";
            const string audience = "http://unittest-rt.com/";
            var expiresOn = new DateTime(2034, 4, 7, 23, 12, 34, DateTimeKind.Utc);
            const string signingKey = "nh0BPopBTc7MAzviohoUbWNhO6NlV+LYm+sXOTPULxk=";

            var swt = new Cheesebaron.MvxPlugins.SimpleWebToken.WindowsCommon.SimpleWebToken().CreateToken(issuer, audience, expiresOn, signingKey, args);
            Assert.AreEqual(expiresOn, swt.ExpiresOn);
            Assert.AreEqual(issuer, swt.Issuer);
            Assert.AreEqual(audience, swt.Audience);
            Assert.IsNotNull(swt.RawToken);
            Assert.IsNotNull(swt.Signature);
            Assert.IsTrue(swt.ValidateSignature(signingKey));
            foreach (var item in args)
                Assert.AreEqual(item.Value, swt[item.Key]);
            Assert.AreEqual(2, swt.Keys.Count());
            Assert.IsTrue(swt.Keys.Contains("alfa"));
            Assert.IsTrue(swt.Keys.Contains("http://unittest-ip.com/alfa"));
        }

        [TestMethod]
        public void RoundTrip()
        {
            var args = new Dictionary<string, string>
            {
                {"alfa", "beta"}, 
                {"http://unittest-ip.com/alfa", "fisk & fugl"}
            };

            const string issuer = "http://unittest-ip.com/";
            const string audience = "http://unittest-rt.com/";
            var expiresOn = new DateTime(2034, 4, 7, 23, 12, 34, DateTimeKind.Utc);
            const string signingKey = "nh0BPopBTc7MAzviohoUbWNhO6NlV+LYm+sXOTPULxk=";

            var swt0 = new Cheesebaron.MvxPlugins.SimpleWebToken.WindowsCommon.SimpleWebToken().CreateToken(issuer, audience, expiresOn, signingKey, args);
            var swt = new Cheesebaron.MvxPlugins.SimpleWebToken.WindowsCommon.SimpleWebToken().CreateTokenFromRaw(swt0.RawToken);
            Assert.IsTrue(swt.ValidateSignature(signingKey));
            Assert.AreEqual(expiresOn, swt.ExpiresOn);
            Assert.AreEqual(issuer, swt.Issuer);
            Assert.AreEqual(audience, swt.Audience);
            Assert.IsNotNull(swt.RawToken);
            Assert.IsNotNull(swt.Signature);
            foreach (var item in args)
                Assert.AreEqual(item.Value, swt[item.Key]);
            Assert.AreEqual(2, swt.Keys.Count());
            Assert.IsTrue(swt.Keys.Contains("alfa"));
            Assert.IsTrue(swt.Keys.Contains("http://unittest-ip.com/alfa"));
        }
    }
}
