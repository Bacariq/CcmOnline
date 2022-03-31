using Bacariq.CcmDal;
using Bacariq.CcmDal.Models;
using System;
using System.Data.Common;
using Bacariq.CcmDal.Helper;
using System.Net.Sockets;
using Bacariq.CcmDal.Singleton;

namespace Bacariq.CcmDesktop.Models
{
    public class Clients
    {
        CnxMysSql m_Cnx;
        public string PkGuid { get; set; }
        public string ClientCcm { get; set; }
        public string GuidSrvOmniPro { get; set; }
        public string Ip { get; set; }
        public DateTime LastFromCcm { get; set; }
        public DateTime LastFromOmniPro { get; set; }
        public DateTime LastCheck { get; set; }
        public bool isLocked { get; set; }
        public string Message { get; set; }

        public string IcoConnected { get; set; }
        public string ColorConnected { get; set; }
        public string IcoLock { get; set; }
        public string ColorLock { get; set; }
        public string TimeConnected { get; set; }


        public Clients()
        {
            Init();
            m_Cnx = new CnxMysSql();
            m_Cnx.Table = "clients";
        }
        ~Clients()
        {
            m_Cnx = null;
        }

        private void Init()
        {
            PkGuid = "";
            ClientCcm = "";
            GuidSrvOmniPro = "";
            Ip ="";
            LastFromCcm = System.DateTime.Now;
            LastFromOmniPro = System.DateTime.Now;
            LastCheck = System.DateTime.Now;
            isLocked = false;

            IcoConnected = SingletonIcone.Instance.IcoLanDisconnect;
            TimeConnected = "0";
            IcoLock = SingletonIcone.Instance.IcoUnlock;
            ColorConnected = "Red";
            ColorLock = "Green";

        }

        public void Save()
        {
            if (PkGuid == "") {
                PkGuid = isExist();
            }

            m_Cnx.Requete = GetSql();
            string sRet = m_Cnx.Execute();
            if (sRet != "")
            {
                PkGuid = sRet;
            }
        }

        private string isExist()
        {
            string sRet = "";
            string sQuery = " select PkGuid from clients where GuidSrvOmniPro = '" + GuidSrvOmniPro + "'";

            try
            {
                m_Cnx.OpenDBConnection();
                m_Cnx.Requete = sQuery;
                DbDataReader drItem = m_Cnx.Query();
                try
                {
                    if (drItem != null && drItem.HasRows)
                    {
                        while (drItem!=null && drItem.Read())
                        {
                            sRet = drItem.SafeGetString(drItem.GetOrdinal("PkGuid"));
                        }
                        drItem.Close();
                    }
                    m_Cnx.CloseDBConnection();
                }
                catch (Exception ex)
                {
                    ErrorLog.SaveLog(" Bacariq.Lawyer.Dal.Model.isExist", ex.ToString());
                }
            }
            catch (Exception ex)
            {
                ErrorLog.SaveLog(" Bacariq.Lawyer.Dal.Model.isExist", ex.ToString());
            }
            return sRet;
        }


        public string GetSql()
        {
            if (isExist() == "")
            {
                PkGuid = "";
                return GetInsertSql();
            }
            else
            {
                return GetUpdateSql();
            }
        }

        private string GetInsertSql()
        {
            string sQuery = "";
            sQuery += "INSERT INTO clients (ClientCcm, GuidSrvOmniPro, Ip, LastFromOmniPro, LastFromCcm, LastCheck,isLocked) VALUES ";
            sQuery += "(";
            sQuery +=  "  '" + ClientCcm + "'";
            sQuery +=  ", '" + GuidSrvOmniPro + "'";
            sQuery +=  ", '" + Ip + "'";
            sQuery += $", '{LastFromOmniPro.ToString("yyyy-MM-dd HH:mm:ss")}'";
            sQuery += $", '{LastFromCcm.ToString("yyyy-MM-dd HH:mm:ss")}'";
            sQuery += $", '{LastCheck.ToString("yyyy-MM-dd HH:mm:ss")}'";
            sQuery += $", '{isLocked}'";
            sQuery += ")";
            return sQuery;

        }

        private string GetUpdateSql()
        {
            string sQuery = "";
            sQuery += "UPDATE clients SET ";
            sQuery += $"  ClientCcm = '{ClientCcm.Replace("'","\'")}'";
            sQuery += $", LastFromOmniPro = '{LastFromOmniPro.ToString("yyyy-MM-dd HH:mm:ss")}'";
            sQuery += $", LastFromCcm     = '{LastFromCcm.ToString("yyyy-MM-dd HH:mm:ss")}'";
            sQuery += $", LastCheck       = '{LastCheck.ToString("yyyy-MM-dd HH:mm:ss")}'";
            sQuery += ",  Ip = '" + Ip + "'";
            sQuery += ",  isLocked = " + isLocked;
            sQuery += " WHERE PkGuid = '" + PkGuid + "'";
            return sQuery;
        }

    }
}
