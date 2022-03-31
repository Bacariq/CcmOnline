using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bacariq.CcmDal.Singleton
{

    public class SingletonDataService
    {
        private static SingletonDataService m_instance = null;
        private static readonly object m_padlock = new object();

        public string TempDirectory
        {
            get { return @Path.Combine(Path.GetTempPath(), "Bacariq_Log"); }
            private set { }
        }

        public DateTime LastSynchroOmniPro { get; set; }
        public string GuidSrvOmniPro { get; set; }
        public bool isAccountLocked { get; set; }
        public string UrlApi { get; set; }
        public string DbType { get; set; }
        public bool isLocked { get; set; }


        public bool isWorkingExportRdv { get; set; }

        public static SingletonDataService Instance
        {
            get
            {
                lock (m_padlock)
                {
                    if (m_instance == null)
                    {
                        m_instance = new SingletonDataService();
                    }
                    return m_instance;
                }
            }
        }

        public SingletonDataService()
        {
            isAccountLocked = true;
            isWorkingExportRdv = false;
        }
    }

    public enum QstInit
    { 
        Text,
        DateTime,
        Date
    }

    public class SingletonDataApplication
    {
        private static SingletonDataApplication m_instance = null;
        private static readonly object m_padlock = new object();
        public string UrlApi { get; set; }
        public string UrlOmniProToCcm { get; set; }
        public string UrlCcmToOmniPro { get; set; }
        public string UrlLogs { get; set; }


        public string TextAFaire { get; set; }
        public QstInit QstDate { get; set; }
        public static SingletonDataApplication Instance
        {
            get
            {
                lock (m_padlock)
                {
                    if (m_instance == null)
                    {
                        m_instance = new SingletonDataApplication();
                    }
                    return m_instance;
                }
            }
        }

        public SingletonDataApplication()
        {

        }

        public void CheckFilders()
        {

            if (Directory.Exists(SingletonDataApplication.Instance.UrlCcmToOmniPro) == false)
            {
                Directory.CreateDirectory(SingletonDataApplication.Instance.UrlCcmToOmniPro);
            }
            if (Directory.Exists(SingletonDataApplication.Instance.UrlLogs) == false)
            {
                Directory.CreateDirectory(SingletonDataApplication.Instance.UrlLogs);
            }
            if (Directory.Exists(SingletonDataApplication.Instance.UrlOmniProToCcm) == false)
            {
                Directory.CreateDirectory(SingletonDataApplication.Instance.UrlOmniProToCcm);
            }
        }

    }
}
