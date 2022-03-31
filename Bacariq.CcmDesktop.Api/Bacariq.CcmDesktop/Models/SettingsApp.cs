using Bacariq.CcmDal.Singleton;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacariq.CcmDesktop.Models
{
    public static class SettingsApp
    {
        public static void LoadSetting()
        {
            SingletonDataApplication.Instance.UrlApi = ReadSetting("UrlApi", "");
            SingletonDataApplication.Instance.UrlCcmToOmniPro = ReadSetting("UrlCcmToOmniPro", "");
            SingletonDataApplication.Instance.UrlOmniProToCcm = ReadSetting("UrlOmniProToCcm", "");
            SingletonDataApplication.Instance.UrlLogs = ReadSetting("UrlLogs", "");
            SingletonDataApplication.Instance.CheckFilders();
        }

        public static void SaveSetting()
        {
            AddUpdateAppSettings("GuidSrvOmniPro", SingletonDataApplication.Instance.UrlApi);
            AddUpdateAppSettings("UrlCcmToOmniPro", SingletonDataApplication.Instance.UrlCcmToOmniPro);
            AddUpdateAppSettings("UrlOmniProToCcm", SingletonDataApplication.Instance.UrlOmniProToCcm);
            AddUpdateAppSettings("UrlLogs", SingletonDataApplication.Instance.UrlLogs);
            SingletonDataApplication.Instance.CheckFilders();
        }

        private static string ReadSetting(string key, string pDefaultValue)
        {
            string sRet = "";
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                sRet = appSettings[key] ?? "Not Found";
                if (sRet == "Not Found")
                {
                    AddUpdateAppSettings(key, pDefaultValue);
                    sRet = pDefaultValue;
                }
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
            return sRet;
        }

        private static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
    }
}
