using Bacariq.CcmDal;
using Bacariq.CcmDesktop.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bacariq.CcmDal.Models;
using Bacariq.CcmDal.Helper;
using Bacariq.CcmDal.Singleton;

namespace Bacariq.CcmDesktop.Repos
{
    public class ReposClient
    {

        private CnxMysSql m_Cnx;
        public ReposClient()
        {
            m_Cnx = new CnxMysSql();
            m_Cnx.Table = "clients";
        }

        private List<Clients> GetBySql(string pQuery)
        {
            List<Clients> TmpObjet = new List<Clients>();
            try
            {
                m_Cnx.OpenDBConnection();
                m_Cnx.Requete = pQuery;
                DbDataReader drObjets = m_Cnx.Query();

                try
                {
                    if (drObjets.HasRows)
                    {
                        while (drObjets.Read())
                        {
                            Clients TmpClients = new Clients()
                            {
                                PkGuid = drObjets.SafeGetString(drObjets.GetOrdinal("PkGuid")),
                                ClientCcm = drObjets.SafeGetString(drObjets.GetOrdinal("ClientCcm")),
                                GuidSrvOmniPro = drObjets.SafeGetString(drObjets.GetOrdinal("GuidSrvOmniPro")),
                                Ip = drObjets.SafeGetString(drObjets.GetOrdinal("Ip")),
                                LastFromOmniPro = drObjets.SafeGetDateTime(drObjets.GetOrdinal("LastFromOmniPro")),
                                LastFromCcm = drObjets.SafeGetDateTime(drObjets.GetOrdinal("LastFromCcm")),
                                LastCheck = drObjets.SafeGetDateTime(drObjets.GetOrdinal("LastCheck")),
                                isLocked = drObjets.SafeGetBool(drObjets.GetOrdinal("isLocked"), false),
                                IcoLock = SingletonIcone.Instance.IcoUnlock,
                                ColorLock = "Green"     ,
                                IcoConnected = SingletonIcone.Instance.IcoLanDisconnect,
                                ColorConnected="Red"
                            };

                            if(TmpClients.isLocked == true)
                            {
                                TmpClients.IcoLock = SingletonIcone.Instance.IcoLock;
                                TmpClients.ColorLock = "Red";
                            }

                            TmpObjet.Add(TmpClients);
                        }
                        drObjets.Close();
                    }
                    m_Cnx.CloseDBConnection();
                }
                catch (Exception ex)
                {
                    ErrorLog.SaveLog(" Bacariq.CcmDesktop.Repos.ReposClient.GetBySql", ex.ToString());
                }
            }
            catch (Exception ex)
            {
                ErrorLog.SaveLog(" Bacariq.CcmDesktop.Repos.ReposClient.GetBySql", ex.ToString());
            }
            return TmpObjet;
        }

        public List<Clients> GetAll()
        {
            string Sql = " SELECT * FROM clients";
            return GetBySql(Sql);
        }

    }
}
