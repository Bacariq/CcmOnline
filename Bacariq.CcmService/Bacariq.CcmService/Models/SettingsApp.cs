using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bacariq.CcmDal.Singleton;
using System;
using System.Configuration;

namespace Bacariq.CcmService.Models
{
    public static class SettingsApp
    {
        public static void LoadSetting()
        {
            //SingletonData.Instance.UrlTcp = "109.88.230.36";
            SingletonDataService.Instance.GuidSrvOmniPro = ReadSetting("GuidSrvOmniPro", "aaaaaaaa-09e7-4bc0-9e87-e9322d366eba");
            SingletonDataService.Instance.UrlApi = ReadSetting("UrlApi", "http://127.0.0.1/Api/Api.php");
            SingletonDataService.Instance.isLocked = ((ReadSetting("isLocked", "false") == "false") ? false : true);
            SingletonDataService.Instance.LastSynchroOmniPro = DateTime.Parse(ReadSetting("LastSynchroOmniPro", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            SingletonDataService.Instance.DbType = ReadSetting("DbType", "ACCESS");
        }

        public static void SaveSetting()
        {
            AddUpdateAppSettings("GuidSrvOmniPro", SingletonDataService.Instance.GuidSrvOmniPro);
            AddUpdateAppSettings("UrlApi", SingletonDataService.Instance.UrlApi);
            AddUpdateAppSettings("isLocked", ((SingletonDataService.Instance.isLocked == true) ? "true" : "false"));
            AddUpdateAppSettings("LastSynchroOmniPro", SingletonDataService.Instance.LastSynchroOmniPro.ToString("yyyy-MM-dd HH:mm:ss"));
            AddUpdateAppSettings("DbType", "ACCESS");
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
