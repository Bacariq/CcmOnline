using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bacariq.CcmDal.Singleton
{
    public class SingletonIcone
    {
        private static SingletonIcone m_instance = null;
        private static readonly object m_padlock = new object();
        private readonly string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;


        public string BcgImg { get; set; }
        public string IcoLanConnect { get; set; }
        public string IcoLanDisconnect { get; set; }
        public string IcoLock { get; set; }
        public string IcoUnlock { get; set; }
        public string IcoSauver { get; set; }
        public string IcoDateRange { get; set; }
        public string IcoClose { get; set; }
        public string IcoFolderNetworkOutline { get; set; }
        public string IcoDownload { get; set; }

        public static SingletonIcone Instance
        {
            get
            {
                lock (m_padlock)
                {
                    if (m_instance == null)
                    {
                        m_instance = new SingletonIcone();
                    }
                    return m_instance;
                }
            }
        }

        public SingletonIcone()
        {
            BcgImg = Path.Combine(projectPath, "Ressource\\bcp.jpg");
            IcoLock = "Lock";
            IcoUnlock = "Unlocked";
            IcoLanDisconnect = "LanDisconnect";
            IcoLanConnect = "LanConnect";
            IcoSauver = "ContentSaveSettingsOutline";
            IcoDateRange = "DateRange"; 
            IcoClose = "Close";
            IcoFolderNetworkOutline = "FolderNetworkOutline";
            IcoDownload = "Download";
        }
    }
}
