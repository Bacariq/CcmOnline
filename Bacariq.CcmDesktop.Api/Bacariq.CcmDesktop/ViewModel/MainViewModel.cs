using Bacariq.CcmDal.Helper;
using Bacariq.CcmDal.Models;
using Bacariq.CcmDesktop.Models;
using Bacariq.CcmDesktop.Repos;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Bacariq.CcmDal.Singleton;
using System.Windows.Forms;
using System.IO;

namespace Bacariq.CcmDesktop.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        private ReposClient mReposClient;
        private System.Timers.Timer aTimerApi;
        private System.Timers.Timer aTimerConnected;
        private bool isWorking;

        public MainViewModel()
        {
            Init();
        }

        ~MainViewModel()
        {
            UnInit();
        }

        private void Init()
        {
            SettingsApp.LoadSetting();
            SingletonDataApplication.Instance.UrlApi = "http://bacariq.com/ApiCCMOmniPro/Api.php";
            SingletonDataApplication.Instance.UrlApi = "http://127.0.0.1/Bacariq.Web/ApiCCMOmniPro/Api.php";
            SettingsApp.SaveSetting();
            
            isWorking = false;
            mReposClient = new ReposClient();
            ClientListe = new ObservableCollection<Clients>(mReposClient.GetAll());
            if (ClientListe == null) {
                ClientListe = new ObservableCollection<Clients>();
            }

            SetTimerApi();
        }

        public void UnInit()
        {
            mReposClient = null;
        }

        #region Timer Api ********************************************************************************

        private void SetTimerApi()
        {
            aTimerApi = new System.Timers.Timer(10000);
            aTimerApi.Elapsed += OnTimedEventApi;
            aTimerApi.AutoReset = true;
            aTimerApi.Enabled = true;

            aTimerConnected = new System.Timers.Timer(10000);
            aTimerConnected.Elapsed += OnTimedEventConnected;
            aTimerConnected.AutoReset = true;
            aTimerConnected.Enabled = true;
        }

        private void StopTimerApi()
        {
            aTimerApi.Elapsed -= OnTimedEventApi;
            aTimerConnected.Elapsed -= OnTimedEventConnected;
            aTimerApi.Stop();
            aTimerApi.Dispose();
        }

        private void OnTimedEventConnected(Object source, System.Timers.ElapsedEventArgs e)
        {
            aTimerConnected.Stop();

            if (isWorking == false)
            {
                isWorking = true;
                for (int i = 0; i < mClientListe.Count; i++)
                {
                    TimeSpan diff = (DateTime.Now - mClientListe[i].LastCheck);
                    if (diff.TotalSeconds > 30 && diff.TotalMinutes < 1)
                    {
                        if (mClientListe[i].ColorConnected != "Orange")
                        {
                            mClientListe[i].ColorConnected = "Orange";
                        }
                    }
                    if (diff.TotalMinutes >= 1)
                    {
                        if (mClientListe[i].ColorConnected != "Red")
                        {
                            mClientListe[i].IcoConnected = SingletonIcone.Instance.IcoLanDisconnect;
                            mClientListe[i].ColorConnected = "Red";

                        }
                    }
                    string sTimeConnected = "";
                    if (diff.TotalDays >= 1) { sTimeConnected += Math.Round(diff.TotalDays).ToString() + " jour(s) "; }
                    if (diff.TotalHours >= 1) { sTimeConnected += Math.Round(diff.TotalHours).ToString() + " Heure(s) "; }
                    if (diff.TotalMinutes >= 1) { sTimeConnected += Math.Round(diff.TotalMinutes).ToString() + " Minute(s) "; }
                    if (diff.TotalSeconds >= 1) { sTimeConnected += Math.Round((diff.TotalSeconds % 60)).ToString() + " Second(s) "; }
                    mClientListe[i].TimeConnected = $"Inactif depuis : {sTimeConnected}.";
                }

                RaisePropertyChanged(nameof(ClientListe));
                isWorking = false;
            }
            aTimerConnected.Start();

        }

        private void OnTimedEventApi(Object source, System.Timers.ElapsedEventArgs e)
        {
            aTimerApi.Stop();
            if (isWorking == false)
            {
                isWorking = true;
                for (int i = 0; i < ClientListe.Count; i++)
                {
                    TraiterClient(ClientListe[i]);
                }
                SendRdv();

                RaisePropertyChanged(nameof(ClientListe));
                isWorking = false;
            }
            aTimerApi.Start();
        }

        private void SendRdv()
        {
            SingletonDataApplication.Instance.CheckFilders();
            string sFolderSrc = SingletonDataApplication.Instance.UrlCcmToOmniPro;
            string sFolderDts = SingletonDataApplication.Instance.UrlCcmToOmniPro;
            string GuidClient = "";
            string sText;
            string FileName;

            foreach (string file in Directory.EnumerateFiles(sFolderSrc, "*.json"))
            {
                FileName = Path.GetFileName(file);
                GuidClient = FileName.Split('_')[0];

                try
                {
                    sFolderDts = SingletonDataApplication.Instance.UrlCcmToOmniPro;
                    sFolderDts = @Path.Combine(sFolderDts, GuidClient);
                    if (Directory.Exists(sFolderDts) == false) { Directory.CreateDirectory(sFolderDts); }
                    sFolderDts = @Path.Combine(sFolderDts, DateTime.Now.ToString("yyyyMMdd"));
                    if (Directory.Exists(sFolderDts) == false) { Directory.CreateDirectory(sFolderDts); }

                    sText = File.ReadAllText(file);
                    JsonData mJsonDataToSend = new JsonData();
                    mJsonDataToSend.ACTION = "RDV_FROM_CCM";
                    mJsonDataToSend.GUID_SRV_OMNIPRO = GuidClient;
                    mJsonDataToSend.DTE_LAST_SYNCHRO = DateTime.Now;
                    mJsonDataToSend.VALUE = sText.Base64Encode();

                    sText = JsonConvert.SerializeObject(mJsonDataToSend).Base64Encode().Encrypt("B@c@riq.is.Very.g00d");
                    CnxWebApi.SendMessage("PutMessage",SingletonDataApplication.Instance.UrlApi, "[START]" + sText + "[EOF]", "Ccm", GuidClient);
                    mJsonDataToSend = null;
                    if (File.Exists(@Path.Combine(sFolderDts, FileName)) == true) {
                        File.Delete(@Path.Combine(sFolderDts, FileName));
                    }
                    Directory.Move(file, @Path.Combine(sFolderDts, FileName));
                } 
                catch
                {
                    sFolderDts = SingletonDataApplication.Instance.UrlCcmToOmniPro;
                    sFolderDts = @Path.Combine(sFolderDts, "Error");
                    if (Directory.Exists(sFolderDts) == false) { Directory.CreateDirectory(sFolderDts); }
                    Directory.Move(file, @Path.Combine(sFolderDts, FileName));

                }
            }

        }

        private void TraiterClient(Clients pClient)
        {
            List<Messages> TmpMessagesListe = CnxWebApi.GetMessage(SingletonDataApplication.Instance.UrlApi, "OmniPro", pClient.GuidSrvOmniPro);
            JsonData mJsonDataToSend = new JsonData();
            string sText;

            foreach (Messages item in TmpMessagesListe)
            {
                try
                {
                    if (item.Message.IndexOf("[START]") >= 0 && item.Message.IndexOf("[EOF]") >= 0)
                    {
                        item.Message = item.Message.Replace("[START]", "").Replace("[EOF]", "");
                        JsonData mJsonDataReceive = JsonConvert.DeserializeObject<JsonData>(item.Message.Decrypt("B@c@riq.is.Very.g00d").Base64Decode());
                        switch (mJsonDataReceive.ACTION)
                        {
                            case "WORKING_RDV":
                                pClient.Message = mJsonDataReceive.MSG;
                                CnxWebApi.UpdateMessage(SingletonDataApplication.Instance.UrlApi, (item.PkGuid).Base64Encode());

                                break;
                            case "RDV_FROM_OMNIPRO":
                                pClient.LastFromOmniPro = mJsonDataReceive.DTE_LAST_SYNCHRO;

                                string Url = Path.Combine(SingletonDataApplication.Instance.UrlOmniProToCcm, DateTime.Now.ToString("yyyyMMdd"), mJsonDataReceive.GUID_SRV_OMNIPRO);
                                string UrlFile = Path.Combine(Url, mJsonDataReceive.GUID_SRV_OMNIPRO + "_" + item.PkGuid + ".json");

                                if (Directory.Exists(Url) == false)
                                {
                                    Directory.CreateDirectory(Url);
                                }

                                try
                                {
                                    File.WriteAllText(UrlFile, mJsonDataReceive.VALUE.Base64Decode());
                                }
                                catch (Exception ex) {
                                    Console.WriteLine(ex.Message);
                                }

                                CnxWebApi.UpdateMessage(SingletonDataApplication.Instance.UrlApi, (item.PkGuid).Base64Encode());

                                break;

                            case "STATUT":
                                pClient.LastCheck = mJsonDataReceive.DTE_LAST_SYNCHRO;
                                pClient.IcoConnected = SingletonIcone.Instance.IcoLanConnect;
                                pClient.ColorConnected = "Green";
                                pClient.Ip = mJsonDataReceive.VALUE;

                                if (mJsonDataReceive.LOCKED != pClient.isLocked)
                                {

                                    mJsonDataToSend = new JsonData();
                                    mJsonDataToSend.GUID_SRV_OMNIPRO = pClient.GuidSrvOmniPro;
                                    mJsonDataToSend.DTE_LAST_SYNCHRO = DateTime.Now;
                                    mJsonDataToSend.VALUE = "";

                                    if (pClient.isLocked == true)
                                    {
                                        mJsonDataToSend.ACTION = "LOCK";
                                    }
                                    else
                                    {
                                        mJsonDataToSend.ACTION = "UNLOCK";
                                    }

                                    sText = JsonConvert.SerializeObject(mJsonDataToSend).Base64Encode().Encrypt("B@c@riq.is.Very.g00d");
                                    CnxWebApi.SendMessage("PutMessage", SingletonDataApplication.Instance.UrlApi, "[START]" + sText + "[EOF]", "Ccm", pClient.GuidSrvOmniPro);
                                    
                                }
                                CnxWebApi.UpdateMessage(SingletonDataApplication.Instance.UrlApi, (item.PkGuid).Base64Encode());
                                break;

                            case "ERROR_LOG":
                                string FileName = mJsonDataReceive.FILENAME;
                                string Folder = @Path.Combine(SingletonDataApplication.Instance.UrlLogs, pClient.GuidSrvOmniPro);
                                if (Directory.Exists(Folder) == false)
                                {
                                    Directory.CreateDirectory(Folder);
                                }

                                if (File.Exists(@Path.Combine(Folder, FileName)) == false)
                                {
                                    File.Create(@Path.Combine(Folder, FileName));
                                }

                                try
                                {
                                    File.WriteAllText(@Path.Combine(Folder, FileName), mJsonDataReceive.VALUE.Base64Decode());
                                }
                                catch { }
                                CnxWebApi.UpdateMessage(SingletonDataApplication.Instance.UrlApi, (item.PkGuid).Base64Encode());
                                break;

                            case "CHANGE_LAST_SYNCHRO_CCM":
                                pClient.LastFromCcm = mJsonDataReceive.DTE_LAST_SYNCHRO;
                                CnxWebApi.UpdateMessage(SingletonDataApplication.Instance.UrlApi, (item.PkGuid).Base64Encode());
                                break;
                            case "ERROR":
                                MessageBox.Show(mJsonDataReceive.MSG);

                                string sFolderDts = SingletonDataApplication.Instance.UrlCcmToOmniPro;
                                sFolderDts = @Path.Combine(sFolderDts, "Error");
                                if (Directory.Exists(sFolderDts) == false) { Directory.CreateDirectory(sFolderDts); }
                                File.WriteAllText(@Path.Combine(sFolderDts, mJsonDataReceive.GUID_SRV_OMNIPRO + "_" + item.PkGuid + ".json"), mJsonDataReceive.VALUE.Base64Decode());
                                File.WriteAllText(@Path.Combine(sFolderDts, mJsonDataReceive.GUID_SRV_OMNIPRO + "_" + item.PkGuid + ".error"), mJsonDataReceive.MSG);

                                CnxWebApi.UpdateMessage(SingletonDataApplication.Instance.UrlApi, (item.PkGuid).Base64Encode());
                                break;
                            default:
                                CnxWebApi.UpdateMessage(SingletonDataApplication.Instance.UrlApi, (item.PkGuid).Base64Encode());
                                break;
                        }
                    }

                }
                catch (Exception ex)
                {
                    CnxWebApi.UpdateMessage(SingletonDataApplication.Instance.UrlApi, (item.PkGuid).Base64Encode());
                }
            }
            
            pClient.Save();
            RaisePropertyChanged(nameof(ClientListe));

        }
        #endregion


        #region Binding Entete ***************************************************************************
        private Clients mClientSelected;
        private List<Clients> mClientListe;

        public Clients ClientSelected
        {
            get
            {
                return mClientSelected;
            }
            set
            {
                if (value != null && mClientSelected != value)
                {
                    mClientSelected = value;
                    RaisePropertyChanged(nameof(ClientSelected));
                }
            }
        }
        public ObservableCollection<Clients> ClientListe
        {
            get
            {
                return new ObservableCollection<Clients>(mClientListe);
            }
            set
            {
                mClientListe = new List<Clients>(value);
                RaisePropertyChanged(nameof(ClientListe));
            }
        }

        public string UrlOmniProToCcm
        {
            get
            {
                return SingletonDataApplication.Instance.UrlOmniProToCcm;
            }
            set
            {
                SingletonDataApplication.Instance.UrlOmniProToCcm = value;
                RaisePropertyChanged(nameof(UrlOmniProToCcm));
            }
        }
        public string UrlCcmToOmniPro
        {
            get
            {
                return SingletonDataApplication.Instance.UrlCcmToOmniPro;
            }
            set
            {
                SingletonDataApplication.Instance.UrlCcmToOmniPro = value;
                RaisePropertyChanged(nameof(UrlCcmToOmniPro));
            }
        }
        public string UrlLogs
        {
            get
            {
                return SingletonDataApplication.Instance.UrlLogs;
            }
            set
            {
                SingletonDataApplication.Instance.UrlLogs = value;
                RaisePropertyChanged(nameof(UrlLogs));
            }
        }
        public string UrlApi
        {
            get
            {
                return SingletonDataApplication.Instance.UrlApi;
            }
            set
            {
                SingletonDataApplication.Instance.UrlApi = value;
                RaisePropertyChanged(nameof(UrlApi));
            }
        }


        #endregion

        #region Icon *************************************************************************************
        public string BcgImg => SingletonIcone.Instance.BcgImg;
        public string IcoLanConnect => SingletonIcone.Instance.IcoLanConnect;
        public string IcoLanDisconnect => SingletonIcone.Instance.IcoLanDisconnect;
        public string IcoLock => SingletonIcone.Instance.IcoLock;
        public string IcoUnlock => SingletonIcone.Instance.IcoUnlock;
        public string IcoSauver => SingletonIcone.Instance.IcoSauver;
        public string IcoDateRange => SingletonIcone.Instance.IcoDateRange;
        public string IcoFolderNetworkOutline => SingletonIcone.Instance.IcoFolderNetworkOutline;
        public string IcoDownload => SingletonIcone.Instance.IcoDownload;
        #endregion


        #region ICommand Entete **************************************************************************
        public ICommand Cmd_ClientLock { get { return new RelayCommand<Clients>(Execute_ClientLock); } private set { } }
        public ICommand Cmd_ClientDownload { get { return new RelayCommand<Clients>(Execute_ClientDownload); } private set { } }
        public ICommand Cmd_ClientReload { get { return new RelayCommand<Clients>(Execute_ClientReload); } private set { } }
        public ICommand Cmd_ClientSauver { get { return new RelayCommand<Clients>(Execute_ClientSauver); } private set { } }
        public ICommand Cmd_ClientChangeDteOmniPro { get { return new RelayCommand<Clients>(Execute_ClientChangeDteOmniPro); } private set { } }
        public ICommand Cmd_ClientChangeDteCcm { get { return new RelayCommand<Clients>(Execute_ClientChangeDteCcm); } private set { } }
        public ICommand Cmd_UrlCcmToOmniPro { get { return new RelayCommand(() => Execute_UrlCcmToOmniPro()); } private set { } }
        public ICommand Cmd_UrlOmniProToCcm { get { return new RelayCommand(() => Execute_UrlOmniProToCcm()); } private set { } }
        public ICommand Cmd_UrlLogs { get { return new RelayCommand(() => Execute_UrlLogs()); } private set { } }
        public ICommand Cmd_Sauver { get { return new RelayCommand(() => Execute_Sauver()); } private set { } }
        
        private void Execute_ClientLock(Clients pClients)
        {
            try
            {
                string sText;
                if (pClients.isLocked == true)
                {
                    pClients.isLocked = false;
                    pClients.IcoLock = SingletonIcone.Instance.IcoUnlock;
                    pClients.ColorLock = "Green";

                    JsonData mJsonData = new JsonData()
                    {
                        GUID_SRV_OMNIPRO = pClients.GuidSrvOmniPro,
                        DTE_LAST_SYNCHRO = DateTime.Now,
                        ACTION = "UNLOCK",
                        VALUE = ""
                    };
                    sText = JsonConvert.SerializeObject(mJsonData).Base64Encode().Encrypt("B@c@riq.is.Very.g00d");
                }
                else
                {
                    pClients.isLocked = true;
                    pClients.IcoLock = SingletonIcone.Instance.IcoLock;
                    pClients.ColorLock = "Red";

                    JsonData mJsonData = new JsonData()
                    {
                        GUID_SRV_OMNIPRO = pClients.GuidSrvOmniPro,
                        DTE_LAST_SYNCHRO = DateTime.Now,
                        ACTION = "LOCK",
                        VALUE = ""
                    };
                    sText = JsonConvert.SerializeObject(mJsonData).Base64Encode().Encrypt("B@c@riq.is.Very.g00d");
                }

                pClients.Save();
                CnxWebApi.SendMessage("PutMessage", SingletonDataApplication.Instance.UrlApi, "[START]" + sText + "[EOF]", "Ccm", pClients.GuidSrvOmniPro);
            }
            catch {
                pClients.IcoConnected = SingletonIcone.Instance.IcoLanConnect;
                pClients.ColorConnected = "Red";
            }
            RaisePropertyChanged(nameof(ClientListe));
        }
        private void Execute_ClientDownload(Clients pClients)
        {
            try
            {
                SingletonDataApplication.Instance.QstDate = QstInit.Date;
                SingletonDataApplication.Instance.TextAFaire = DateTime.Now.ToString("yyyy-MM-dd");
                QstDate fenetre = new QstDate();
                fenetre.ShowDialog();
                fenetre = null;
                string sTextValue = SingletonDataApplication.Instance.TextAFaire;

                DateTime TmpDte = DateTime.Parse(sTextValue);
                JsonData mJsonData = new JsonData()
                {
                    GUID_SRV_OMNIPRO = pClients.GuidSrvOmniPro,
                    DTE_LAST_SYNCHRO = DateTime.Now,
                    ACTION = "ERROR_LOG",
                    VALUE = TmpDte.ToString("yyyyMMdd")
                };
                sTextValue = JsonConvert.SerializeObject(mJsonData).Base64Encode().Encrypt("B@c@riq.is.Very.g00d");

                CnxWebApi.SendMessage("PutMessage", SingletonDataApplication.Instance.UrlApi, "[START]" + sTextValue + "[EOF]", "Ccm", pClients.GuidSrvOmniPro);
            }
            catch {

            }

        }
        private void Execute_ClientReload(Clients pClients)
        {
            try
            {
                SingletonDataApplication.Instance.QstDate = QstInit.DateTime;
                SingletonDataApplication.Instance.TextAFaire = pClients.LastFromOmniPro.ToString();
                QstDate fenetre = new QstDate();
                fenetre.ShowDialog();
                fenetre = null;
                DateTime TmpDateTime = DateTime.Parse(SingletonDataApplication.Instance.TextAFaire);
                CnxWebApi.SendMessage("ReloadTRansaction", SingletonDataApplication.Instance.UrlApi, TmpDateTime.ToString("yyyy-MM-dd HH:mm:ss").Base64Encode(), "Ccm", pClients.GuidSrvOmniPro);
            }
            catch {

            }

        }

        private void Execute_ClientSauver(Clients pClients)
        {
            SingletonDataApplication.Instance.QstDate = QstInit.Text;
            SingletonDataApplication.Instance.TextAFaire = pClients.ClientCcm;
            QstDate fenetre = new QstDate();
            fenetre.ShowDialog();
            fenetre = null;
            pClients.ClientCcm = SingletonDataApplication.Instance.TextAFaire;
            pClients.Save();
            RaisePropertyChanged(nameof(ClientListe));
        }

        private void Execute_ClientChangeDteOmniPro(Clients pClients)
        {
            SingletonDataApplication.Instance.QstDate = QstInit.DateTime;
            SingletonDataApplication.Instance.TextAFaire = pClients.LastFromOmniPro.ToString();
            QstDate fenetre = new QstDate();
            fenetre.ShowDialog();
            fenetre = null;
            DateTime TmpDateTime = DateTime.Parse(SingletonDataApplication.Instance.TextAFaire);
            if (TmpDateTime != pClients.LastFromOmniPro)
            {
                JsonData mJsonDataToSend = new JsonData();
                mJsonDataToSend.ACTION = "CHANGE_LAST_SYNCHRO_OMNIPRO";
                mJsonDataToSend.GUID_SRV_OMNIPRO = pClients.GuidSrvOmniPro;
                mJsonDataToSend.DTE_LAST_SYNCHRO = TmpDateTime;
                mJsonDataToSend.VALUE = "";
                string sText = JsonConvert.SerializeObject(mJsonDataToSend).Base64Encode().Encrypt("B@c@riq.is.Very.g00d");
                CnxWebApi.SendMessage("PutMessage", SingletonDataApplication.Instance.UrlApi, "[START]" + sText + "[EOF]", "Ccm", pClients.GuidSrvOmniPro);
                pClients.LastFromOmniPro = TmpDateTime;
            }
            RaisePropertyChanged(nameof(ClientListe));
        }

        private void Execute_ClientChangeDteCcm(Clients pClients)
        {
            /*
            SingletonDataApplication.Instance.QstDate = QstInit.DateTime;
            SingletonDataApplication.Instance.TextAFaire = pClients.LastFromCcm.ToString();
            QstDate fenetre = new QstDate();
            fenetre.ShowDialog();
            fenetre = null;
            DateTime TmpDateTime = DateTime.Parse(SingletonDataApplication.Instance.TextAFaire);
            if (TmpDateTime != pClients.LastFromOmniPro)
            {
                JsonData mJsonDataToSend = new JsonData();
                mJsonDataToSend.ACTION = "CHANGE_SYNCHRO_CCM";
                mJsonDataToSend.GUID_SRV_OMNIPRO = pClients.GuidSrvOmniPro;
                mJsonDataToSend.DTE_LAST_SYNCHRO = TmpDateTime;
                mJsonDataToSend.VALUE = "";
                string sText = JsonConvert.SerializeObject(mJsonDataToSend).Base64Encode().Encrypt("B@c@riq.is.Very.g00d");
                CnxWebApi.SendMessage(SingletonDataApplication.Instance.UrlApi, "[START]" + sText + "[EOF]", "Ccm", pClients.GuidSrvOmniPro);
                pClients.LastFromOmniPro = TmpDateTime;
            }
            RaisePropertyChanged(nameof(ClientListe));
            */
        }

        private void Execute_UrlCcmToOmniPro() {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    UrlCcmToOmniPro=fbd.SelectedPath;
                }
            }
        }

        private void Execute_UrlOmniProToCcm()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    UrlOmniProToCcm = fbd.SelectedPath;
                }
            }
        }

        private void Execute_UrlLogs()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    UrlLogs = fbd.SelectedPath;
                }
            }
        }

        private void Execute_Sauver()
        {
            SettingsApp.SaveSetting();
        }


        #endregion

    }
}