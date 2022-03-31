using Bacariq.CcmDal.Helper;
using Bacariq.CcmDal.Models;
using Bacariq.CcmDal.Singleton;
using Bacariq.CcmService.Helper;
using Bacariq.CcmService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacariq.CcmService.Repos
{
    public class ReposAgdRdvOmniPro
    {
        private ICnxDataBase m_Cnx;
        private JsonData mJsonDataToSend;

        public ReposAgdRdvOmniPro()
        {
            m_Cnx = new CnxAccess();
            m_Cnx.Table = "agd_rdv";
        }

        ~ReposAgdRdvOmniPro()
        {
            m_Cnx = null;
        }


        private List<AgendaRdv> GetBySql(string pSql)
        {
            List<AgendaRdv> LstItem = new List<AgendaRdv>();
            try
            {
                m_Cnx.OpenDBConnection();
                m_Cnx.Requete = pSql;
                OleDbDataReader dr_Item = m_Cnx.Query<OleDbDataReader>();
                if (dr_Item != null && dr_Item.HasRows)
                {
                    while (dr_Item.Read())
                    {
                        LstItem.Add(new AgendaRdv()
                        {
                            //PKGUID = dr_Item["PkGuid"].ToString(),
                            NUMPAT = int.Parse(dr_Item["NUMPAT"].ToString()),
                            NUMRDV = int.Parse(dr_Item["NUMRDV"].ToString()),
                            NUMUSER = int.Parse(dr_Item["NUMUSER"].ToString()),
                            NUMBOSS = int.Parse(dr_Item["NUMBOSS"].ToString()),
                            NUMAGD = int.Parse(dr_Item["NUMAGD"].ToString()),
                            NUMDEM = int.Parse(dr_Item["NUMDEM"].ToString()),
                            DESCRIPTION = dr_Item["DESCRIPTION"].ToString(),
                            NUMDESCRIPTION = int.Parse(dr_Item["NUMDESCRIPTION"].ToString()),
                            DTEDEB = dr_Item["DTEDEB"].ToString(),
                            HEURE = dr_Item["HEURE"].ToString(),
                            DUREE = int.Parse(dr_Item["DUREE"].ToString()),
                            DTECRE = DateTime.Parse(dr_Item["DTECRE"].ToString()),
                            DTEMOD = DateTime.Parse(dr_Item["DTEMOD"].ToString()),
                            NUMSITE = int.Parse(dr_Item["NUMSITE"].ToString()),
                            SITE = dr_Item["SITE"].ToString(),
                            STATUT = dr_Item["STATUT"].ToString(),
                            SOUSAGD = dr_Item["DUREE"].ToString(),
                            COMPTERENDU = dr_Item["COMPTERENDU"].ToString(),
                            RDVDELETED = (dr_Item["RDVDELETED"].ToString() == "1") ? true : false
                        });
                    }
                    dr_Item.Close();
                }
                m_Cnx.CloseDBConnection();
            }
            catch (Exception ex)
            {
                ErrorLog.SaveLog("testApi.Repos.ReposAgdRdvOmniPro.GetAll", ex.ToString());
            }
            finally
            {
                m_Cnx.CloseDBConnection();
            }
            return LstItem;
        }

        public void SetGuid()
        {
            string sQuery = "select numrdv from agd_rdv where Len(PkGuid) is null or Len(PkGuid) = 0 ";
            List<int> LstItem = new List<int>();
            try
            {
                m_Cnx.OpenDBConnection();
                m_Cnx.Requete = sQuery;
                OleDbDataReader dr_Item = m_Cnx.Query<OleDbDataReader>();
                if (dr_Item != null && dr_Item.HasRows)
                {
                    while (dr_Item.Read())
                    {
                        LstItem.Add(int.Parse(dr_Item["NUMRDV"].ToString()));
                    }
                    dr_Item.Close();
                }
                m_Cnx.CloseDBConnection();
            }
            catch (Exception ex)
            {
                ErrorLog.SaveLog("testApi.Repos.ReposAgdRdvOmniPro.SetGuid", ex.ToString());
            }
            finally
            {
                m_Cnx.CloseDBConnection();
            }


            m_Cnx.OpenDBConnection();
            foreach (int item in LstItem)
            {
                string mGuid = Guid.NewGuid().ToString();
                sQuery = $"update agd_rdv set PkGuid = '{mGuid}' where NumRdv = {item}";
                m_Cnx.Requete = sQuery;
                m_Cnx.Query<OleDbDataReader>();

                sQuery = $"insert into Correspondance (NumRdv, PkGuidRdv) value ({item}, '{mGuid}')";
                m_Cnx.Requete = sQuery;
                m_Cnx.Query<OleDbDataReader>();
            }
            m_Cnx.CloseDBConnection();
        }


        public void SetGuid(int pNumRdv)
        {
            string sQuery = $"update agd_rdv set PkGuid = '{Guid.NewGuid().ToString()}' where num_rdv = {pNumRdv}";
            m_Cnx.OpenDBConnection();
            m_Cnx.Requete = sQuery;
            OleDbDataReader dr_Item = m_Cnx.Query<OleDbDataReader>();
            m_Cnx.CloseDBConnection();
        }

        public List<JSonExport> GetAllFrom(DateTime pDateTime)
        {
            ReposPatientOmniPro mReposPatientOmniPro = new ReposPatientOmniPro();
            ReposPersonneOmniPro mReposPersonneOmniPro = new ReposPersonneOmniPro();
            List<JSonExport> mJSonExport = new List<JSonExport>();


            string sQuery = "select * from agd_rdv ";

            if (SingletonDataService.Instance.DbType == "ACCESS")
            {
                sQuery += $" where (agd_rdv.dtecre >=  CDate('{ pDateTime.ToString("dd/MM/yy")} {pDateTime.ToString("HH:mm:ss") }'))" +
                          $"    or (agd_rdv.dtemod >=  CDate('{ pDateTime.ToString("dd/MM/yy")} {pDateTime.ToString("HH:mm:ss") }'))";
            }

            List<AgendaRdv> TmpAgdRdv = new List<AgendaRdv>();
            TmpAgdRdv = GetBySql(sQuery);
            for (int i = 0; i < TmpAgdRdv.Count; i++)
            {
                JSonExport TmpJSonExport = new JSonExport();
                TmpJSonExport.GuidClientOP = SingletonDataService.Instance.GuidSrvOmniPro;
                TmpJSonExport.AgendaRdv = TmpAgdRdv[i];
                TmpJSonExport.Patients = mReposPatientOmniPro.GetByGuid(TmpAgdRdv[i].NUMPAT);
                TmpJSonExport.Boss = mReposPersonneOmniPro.GetByGuid(TmpAgdRdv[i].NUMBOSS);
                TmpJSonExport.User = mReposPersonneOmniPro.GetByGuid(TmpAgdRdv[i].NUMUSER);
                TmpJSonExport.AgendaRdv.PKGUID = "Not used";
                TmpJSonExport.Patients.PKGUID = "Not used";

                if (TmpJSonExport.AgendaRdv.PKGUID == "")
                {
                    string mGuid = "";
                    ReposCorrespondance mReposCorrespondance = new ReposCorrespondance();
                    mGuid = mReposCorrespondance.GetGuid();
                    mReposCorrespondance = null;

                    if (mGuid == "")
                    {
                        mGuid = Guid.NewGuid().ToString();
                    }

                    TmpJSonExport.AgendaRdv.PKGUID = mGuid;
                    TmpJSonExport.AgendaRdv.SetGuid(TmpJSonExport.AgendaRdv.PKGUID);
                }

                mJSonExport.Add(TmpJSonExport);
            }
            mReposPatientOmniPro = null;
            mReposPersonneOmniPro = null;
            return mJSonExport;
        }

        public List<AgendaRdv> GetBetween(DateTime pDateTimeStart, DateTime pDateTimeEnd)
        {
            string sQuery = "select * from agd_rdv " +
                "where (dtecre >= #" + pDateTimeStart.ToString("dd/MM/yyyy") + "# and TimeValue(dtecre)>TimeValue('" + pDateTimeStart.ToString("hh:mm:ss") + "') and dtecre >= #" + pDateTimeEnd.ToString("dd/MM/yyyy") + "# and TimeValue(dtecre)>TimeValue('" + pDateTimeEnd.ToString("hh:mm:ss") + "'))" +
                "  or  (dtemod >= #" + pDateTimeStart.ToString("dd/MM/yyyy") + "# and TimeValue(dtemod)>TimeValue('" + pDateTimeStart.ToString("hh:mm:ss") + "') and dtemod >= #" + pDateTimeEnd.ToString("dd/MM/yyyy") + "# and TimeValue(dtemod)>TimeValue('" + pDateTimeEnd.ToString("hh:mm:ss") + "'))";

            return GetBySql(sQuery);
        }


        private bool isPlageDispo(AgendaRdv pAgendaRdv)
        {
            bool bRet = true;

            int Start = (int.Parse(pAgendaRdv.HEURE.Split(':')[0]) * 60) + int.Parse(pAgendaRdv.HEURE.Split(':')[1]);
            int End = Start + (pAgendaRdv.DUREE * 5);

            m_Cnx.OpenDBConnection();
            m_Cnx.Requete = $"select dtedeb, heure, duree, numboss, NumRdv, " +
                             "       (Left(heure, 2) * 60) as StartInMinutes, " +
                             "       ((Left(heure, 2) * 60) + Right(heure, 2) + (duree * 5)) as EndInMinutes " +
                             "  from agd_rdv " +
                            $" where len(Heure) = 5 and NumBoss = {pAgendaRdv.NUMBOSS}";
            DateTime TmpDate = DateTime.Parse(pAgendaRdv.DTEDEB);

            if (SingletonDataService.Instance.DbType == "ACCESS")
            {
                m_Cnx.Requete += $" and dtedeb = #{TmpDate.ToString("MM/dd/yyyy")}#";
            }

            OleDbDataReader dr_Item = m_Cnx.Query<OleDbDataReader>();
            if (dr_Item != null && dr_Item.HasRows)
            {
                while (dr_Item.Read() && bRet == true)
                {
                    int StartCheck = int.Parse(dr_Item["StartInMinutes"].ToString());
                    int EndCheck = int.Parse(dr_Item["EndInMinutes"].ToString());

                    //500-545 == 
                    if (Start == StartCheck && End == EndCheck && pAgendaRdv.NUMRDV != int.Parse(dr_Item["NUMRDV"].ToString()))
                    {
                        bRet = false;
                    }

                    // si debut se trouve deja dans un rdv
                    if (Start > StartCheck && Start < EndCheck && pAgendaRdv.NUMRDV != int.Parse(dr_Item["NUMRDV"].ToString()))
                    {
                        bRet = false;
                    }
                    // si fin se trouve deja dans un rdv
                    if (End > StartCheck && End < EndCheck && pAgendaRdv.NUMRDV != int.Parse(dr_Item["NUMRDV"].ToString()))
                    {
                        bRet = false;
                    }

                }
            }

            return bRet;
        }


        internal void Traiter(JSonExport pitemData)
        {
            if (isPlageDispo(pitemData.AgendaRdv) == false)
            {
                mJsonDataToSend = new JsonData()
                {
                    ACTION = "ERROR",
                    GUID_SRV_OMNIPRO = SingletonDataService.Instance.GuidSrvOmniPro,
                    LOCKED = SingletonDataService.Instance.isLocked,
                    MSG = $"Impossible de modifier le Rdv ({pitemData.AgendaRdv.DTEDEB.Split(' ')[0]} à {pitemData.AgendaRdv.HEURE}) de Mr/Mme {pitemData.Patients.NOM}, {pitemData.Patients.PRENOM} car la plage est déjà prise.",
                    VALUE = JsonConvert.SerializeObject(pitemData).Base64Encode()
                };

                string Text = JsonConvert.SerializeObject(mJsonDataToSend).Base64Encode().Encrypt("B@c@riq.is.Very.g00d");
                CnxWebApi.SendMessage("PutMessage", SingletonDataService.Instance.UrlApi, "[START]" + Text + "[EOF]", "OmniPro", SingletonDataService.Instance.GuidSrvOmniPro);
                mJsonDataToSend = null;
            }
            else
            {
                AgendaRdv TmpAgendaRdv = new AgendaRdv();
                TmpAgendaRdv = pitemData.AgendaRdv;
                TmpAgendaRdv.Sauver();
                TmpAgendaRdv = null;
            }
        }


    }
}
