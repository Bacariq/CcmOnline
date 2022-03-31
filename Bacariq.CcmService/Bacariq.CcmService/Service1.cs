using Bacariq.CcmDal.Helper;
using Bacariq.CcmDal.Models;
using Bacariq.CcmDal.Singleton;
using Bacariq.CcmService.Models;
using Bacariq.CcmService.Repos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bacariq.CcmService
{
    public partial class Service1 : ServiceBase
    {

        private bool isWorking = false;
        private string Ip = "";

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            SettingsApp.LoadSetting();
            SingletonDataService.Instance.GuidSrvOmniPro = "bbbbbbbb-09e7-4bc0-9e87-e9322d366eba";
            SingletonDataService.Instance.UrlApi = "http://bacariq.com/ApiCCMOmniPro/Api.php";
            SingletonDataService.Instance.isLocked = false;
            SingletonDataService.Instance.DbType = "ACCESS";
            LoadFile();
            SettingsApp.SaveSetting();

            Ip = "IpV6 : " + new WebClient().DownloadString("http://icanhazip.com") + "IpV4 : " + GetIPAddress();

            SetTimerAPI();
            //Thread t = new Thread(new ThreadStart(ThreadProc));
            //t.Start();

            Console.ReadLine();
        }

        protected override void OnStop()
        {
            SettingsApp.SaveSetting();
        }


        private static void LoadFile()
        {
            if (File.Exists(@"C:\Bacariq.SynchroRdv\config.txt") == true)
            {
                List<string> TmpListe = File.ReadAllLines(@"C:\Bacariq.SynchroRdv\config.txt").ToList();
                foreach (string item in TmpListe)
                {
                    if (item.ToUpper().StartsWith(("GuidSrvOmniPro").ToUpper()) == true)
                    {
                        SingletonDataService.Instance.GuidSrvOmniPro = item.Split('=')[1];
                    }
                    if (item.ToUpper().StartsWith(("UrlApi").ToUpper()) == true)
                    {
                        SingletonDataService.Instance.UrlApi = item.Split('=')[1];
                    }
                    if (item.ToUpper().StartsWith(("DbType").ToUpper()) == true)
                    {
                        SingletonDataService.Instance.DbType = item.Split('=')[1];
                    }
                }
            }
        }

        private static void ThreadProc()
        {
            while (true) { }
        }

        #region Timer API ********************************************************************************

        private System.Timers.Timer aPutMessages;
        private System.Timers.Timer aGetMessages;

        private void SetTimerAPI()
        {
            aPutMessages = new System.Timers.Timer(10000);
            aPutMessages.Elapsed += OnTimedEventPutMsg;
            aPutMessages.AutoReset = false;
            aPutMessages.Enabled = true;

            aGetMessages = new System.Timers.Timer(15000);
            aGetMessages.Elapsed += OnTimedEventGetMsg;
            aGetMessages.AutoReset = false;
            aGetMessages.Enabled = true;
        }

        private string GetIPAddress()
        {
            String address = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }

            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);


            return address;
        }

        private void OnTimedEventPutMsg(Object source, System.Timers.ElapsedEventArgs e)
        {
            aPutMessages.Stop();
            aPutMessages.Enabled = false;


            if (isWorking == false)
            {
                isWorking = true;

                JsonData mJsonData = new JsonData();
                string sText;
                mJsonData = new JsonData()
                {
                    GUID_SRV_OMNIPRO = SingletonDataService.Instance.GuidSrvOmniPro,
                    DTE_LAST_SYNCHRO = DateTime.Now,
                    ACTION = "STATUT",
                    LOCKED = SingletonDataService.Instance.isLocked,
                    VALUE = Ip
                };
                sText = JsonConvert.SerializeObject(mJsonData).Base64Encode().Encrypt("B@c@riq.is.Very.g00d");
                CnxWebApi.SendMessage("PutMessage", SingletonDataService.Instance.UrlApi, "[START]" + sText + "[EOF]", "OmniPro", SingletonDataService.Instance.GuidSrvOmniPro);


                if (SingletonDataService.Instance.isLocked == false)
                {
                    ReposAgdRdvOmniPro mReposAgdRdvOmniPro = new ReposAgdRdvOmniPro();
                    List<JSonExport> TmpJSonExport = mReposAgdRdvOmniPro.GetAllFrom(SingletonDataService.Instance.LastSynchroOmniPro);

                    for (int i = 0; i < TmpJSonExport.Count; i++)
                    {
                        TmpJSonExport[i].GuidClientOP = SingletonDataService.Instance.GuidSrvOmniPro;
                        mJsonData = new JsonData()
                        {
                            GUID_SRV_OMNIPRO = SingletonDataService.Instance.GuidSrvOmniPro,
                            DTE_LAST_SYNCHRO = SingletonDataService.Instance.LastSynchroOmniPro,
                            ACTION = "RDV_FROM_OMNIPRO",
                            LOCKED = SingletonDataService.Instance.isLocked,
                            VALUE = JsonConvert.SerializeObject(TmpJSonExport[i]).Base64Encode()
                        };
                        sText = JsonConvert.SerializeObject(mJsonData).Base64Encode().Encrypt("B@c@riq.is.Very.g00d");
                        CnxWebApi.SendMessage("PutMessage", SingletonDataService.Instance.UrlApi, "[START]" + sText + "[EOF]", "OmniPro", SingletonDataService.Instance.GuidSrvOmniPro);

                    }
                    if (TmpJSonExport.Count > 0)
                    {
                        SingletonDataService.Instance.LastSynchroOmniPro = DateTime.Now;
                        SettingsApp.SaveSetting();
                    }
                    mReposAgdRdvOmniPro = null;
                    TmpJSonExport = null;
                }

                isWorking = false;
            }

            aPutMessages.Enabled = true;
            aPutMessages.Start();
        }

        private void OnTimedEventGetMsg(Object source, System.Timers.ElapsedEventArgs e)
        {
            aGetMessages.Stop();
            aGetMessages.Enabled = false;

            Ip = "IpV6 : " + new WebClient().DownloadString("http://icanhazip.com") + "IpV4 : " + GetIPAddress();

            if (isWorking == false)
            {
                isWorking = true;
                List<Messages> TmpMessagesListe = CnxWebApi.GetMessage(SingletonDataService.Instance.UrlApi, "Ccm", SingletonDataService.Instance.GuidSrvOmniPro);
                JsonData mJsonDataToSend = new JsonData();
                string Text = "";
                string sIdTraite = "";
                foreach (Messages item in TmpMessagesListe)
                {
                    if (item.Message.IndexOf("[START]") >= 0 && item.Message.IndexOf("[EOF]") >= 0)
                    {
                        sIdTraite += "'" + item.PkGuid + "',";
                        item.Message = item.Message.Replace("[START]", "").Replace("[EOF]", "");
                        JsonData mJsonDataReceive = JsonConvert.DeserializeObject<JsonData>(item.Message.Decrypt("B@c@riq.is.Very.g00d").Base64Decode());
                        switch (mJsonDataReceive.ACTION)
                        {
                            case "LOCK":
                                SingletonDataService.Instance.isLocked = true;
                                SettingsApp.SaveSetting();
                                CnxWebApi.UpdateMessage(SingletonDataService.Instance.UrlApi, item.PkGuid.Base64Encode());
                                break;

                            case "UNLOCK":
                                SingletonDataService.Instance.isLocked = false;
                                SettingsApp.SaveSetting();
                                CnxWebApi.UpdateMessage(SingletonDataService.Instance.UrlApi, item.PkGuid.Base64Encode());
                                break;

                            case "CHANGE_LAST_SYNCHRO_OMNIPRO":
                                SingletonDataService.Instance.LastSynchroOmniPro = mJsonDataReceive.DTE_LAST_SYNCHRO;
                                SettingsApp.SaveSetting();
                                CnxWebApi.UpdateMessage(SingletonDataService.Instance.UrlApi, item.PkGuid.Base64Encode());
                                break;

                            case "ERROR_LOG":
                                ErrorLog.LoadLog(ref mJsonDataReceive);
                                mJsonDataToSend = new JsonData()
                                {
                                    ACTION = "ERROR_LOG",
                                    GUID_SRV_OMNIPRO = SingletonDataService.Instance.GuidSrvOmniPro,
                                    LOCKED = SingletonDataService.Instance.isLocked,
                                    VALUE = mJsonDataReceive.VALUE,
                                    FILENAME = mJsonDataReceive.FILENAME,
                                };
                                Text = JsonConvert.SerializeObject(mJsonDataToSend).Base64Encode().Encrypt("B@c@riq.is.Very.g00d");
                                CnxWebApi.SendMessage("PutMessage", SingletonDataService.Instance.UrlApi, "[START]" + Text + "[EOF]", "OmniPro", SingletonDataService.Instance.GuidSrvOmniPro);
                                mJsonDataToSend = null;
                                CnxWebApi.UpdateMessage(SingletonDataService.Instance.UrlApi, item.PkGuid.Base64Encode());
                                break;

                            case "RDV_FROM_CCM":
                                if (SingletonDataService.Instance.isLocked == false)
                                {
                                    ReposAgdRdvOmniPro mReposAgdRdvOmniPro = new ReposAgdRdvOmniPro();
                                    JSonExport TmpJSonImport = JsonConvert.DeserializeObject<JSonExport>(mJsonDataReceive.VALUE.Base64Decode());
                                    mReposAgdRdvOmniPro.Traiter(TmpJSonImport);
                                    TmpJSonImport = null;
                                    mReposAgdRdvOmniPro = null;
                                }

                                CnxWebApi.UpdateMessage(SingletonDataService.Instance.UrlApi, item.PkGuid.Base64Encode());

                                mJsonDataToSend = new JsonData()
                                {
                                    ACTION = "CHANGE_LAST_SYNCHRO_CCM",
                                    DTE_LAST_SYNCHRO = DateTime.Now,
                                    GUID_SRV_OMNIPRO = SingletonDataService.Instance.GuidSrvOmniPro,
                                    LOCKED = SingletonDataService.Instance.isLocked
                                };
                                Text = JsonConvert.SerializeObject(mJsonDataToSend).Base64Encode().Encrypt("B@c@riq.is.Very.g00d");
                                CnxWebApi.SendMessage("PutMessage", SingletonDataService.Instance.UrlApi, "[START]" + Text + "[EOF]", "OmniPro", SingletonDataService.Instance.GuidSrvOmniPro);
                                mJsonDataToSend = null;

                                break;
                            case "OK":
                                break;
                        }
                    }
                }

                isWorking = false;
            }

            aGetMessages.Enabled = false;
            aGetMessages.Start();
            if (aPutMessages.Enabled == false) { aPutMessages.Enabled = true; }
        }
        #endregion
    }
}
