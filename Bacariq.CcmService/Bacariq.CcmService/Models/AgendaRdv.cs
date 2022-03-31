using Bacariq.CcmService.Helper;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacariq.CcmService.Models
{
    public class AgendaRdv
    {
        private ICnxDataBase m_Cnx;
        public string PKGUID { get; set; }
        public int NUMRDV { get; set; }
        public int NUMDEM { get; set; }
        public int NUMAGD { get; set; }
        public int NUMPAT { get; set; }
        public int NUMUSER { get; set; }
        public int NUMBOSS { get; set; }
        public int NUMDESCRIPTION { get; set; }
        public string DESCRIPTION { get; set; }
        public string SITE { get; set; }
        public int NUMSITE { get; set; }
        public string DTEDEB { get; set; }
        public string HEURE { get; set; }
        public int DUREE { get; set; }
        public DateTime DTECRE { get; set; }
        public DateTime DTEMOD { get; set; }
        public string STATUT { get; set; }
        public string SOUSAGD { get; set; }
        public string COMPTERENDU { get; set; }
        public bool RDVDELETED { get; set; }

        private int GetNewNum(string pType)
        {
            int iRet = 0;

            m_Cnx.OpenDBConnection();
            m_Cnx.Requete = $"select numero from numeros where type ='{pType.ToUpper()}'";
            OleDbDataReader dr_Item = m_Cnx.Query<OleDbDataReader>();
            if (dr_Item.HasRows)
            {
                while (dr_Item.Read())
                {
                    iRet = int.Parse(dr_Item["numero"].ToString());
                }
            }

            m_Cnx.Requete = $"update numeros set numero = {(iRet + 1)} where type ='RDV'";
            m_Cnx.Query<OleDbDataReader>();
            m_Cnx.CloseDBConnection();

            return iRet;
        }

        public AgendaRdv()
        {
            m_Cnx = new CnxAccess();
            m_Cnx.Table = "agd_rdv";
        }
        ~AgendaRdv()
        {
            m_Cnx = null;
        }

        public void SetGuid(string pGuid)
        {
            m_Cnx.OpenDBConnection();
            m_Cnx.Requete = $"update agd_rdv set PkGuid = '{pGuid}' where numrdv = { NUMRDV.ToString()}";
            m_Cnx.Query<OleDbDataReader>();

            m_Cnx.Requete = $"insert into Correspondance (NumRdv, PkGuidRdv) value ({NUMRDV.ToString()}, '{pGuid}')";
            m_Cnx.Query<OleDbDataReader>();
            m_Cnx.CloseDBConnection();
        }

        public void Sauver()
        {
            if (isExist() == true)
            {
                m_Cnx.OpenDBConnection();

                m_Cnx.Requete = UpdateSuivi();
                m_Cnx.Query<OleDbDataReader>();

                m_Cnx.Requete = Update();
                m_Cnx.Query<OleDbDataReader>();

                m_Cnx.CloseDBConnection();
            }
            else
            {
                PKGUID = Guid.NewGuid().ToString();
                NUMRDV = GetNewNum("RDV");
                NUMDEM = GetNewNum("DEM");
                int NumSuivi = GetNewNum("RDV");

                m_Cnx.OpenDBConnection();

                m_Cnx.Requete = InsertSuivi(NumSuivi, "", "");
                m_Cnx.Query<OleDbDataReader>();

                m_Cnx.Requete = Insert();
                m_Cnx.Query<OleDbDataReader>();

                m_Cnx.CloseDBConnection();
            }
        }

        private bool isExist()
        {
            bool bRet = false;
            m_Cnx.OpenDBConnection();
            //m_Cnx.Requete = $"select count(*) as NB from agd_rdv where PkGuid = '{PKGUID}'";
            m_Cnx.Requete = $"select count(*) as NB from agd_rdv where numrdv = {NUMRDV}";
            OleDbDataReader dr_Item = m_Cnx.Query<OleDbDataReader>();
            if (dr_Item != null && dr_Item.HasRows)
            {
                while (dr_Item.Read())
                {
                    if (int.Parse(dr_Item["NB"].ToString()) == 0)
                    {
                        bRet = false;
                    }
                    else
                    {
                        bRet = true;
                    }
                }
            }
            return bRet;
        }

        private string Insert()
        {

            string sRet = $" insert into agd_suivi (numrdv, numboss, numagd, numuser, duree, numpat, numdem, numdescription, " +
                          $"                        dtedeb, heure, description, sousagd, dtecre, dtemod, site, numsite ) values " +
                          $"( " +
                          $"  NUMRDV = " + NUMRDV +
                          $", NUMBOSS = " + NUMBOSS +
                          $", NUMAGD = " + NUMAGD +
                          $", NUMUSER = " + NUMUSER +
                          $", DUREE = " + DUREE +
                          $", NUMPAT = " + NUMPAT +
                          $", NUMDEM = " + NUMDEM +
                          $", NUMDESCRIPTION = " + NUMDESCRIPTION +
                          $", DTEDEB = " + DTEDEB +
                          $", HEURE = " + HEURE +
                          $", DESCRIPTION = " + DESCRIPTION +
                          $", SOUSAGD = " + SOUSAGD +
                          $", DTECRE = " + DTECRE +
                          $", DTEMOD = " + DTEMOD +
                          $", SITE = " + SITE +
                          $", NUMSITE = " + NUMSITE +
                          $")";
            return sRet;
        }

        private string InsertSuivi(int pNumSuivi, string pNomBoss, string pNomUser)
        {
            string sRet = $" insert into agd_suivi (NUMSUIVI, NUMDEM, NUMPAT, NUMDESCRIPTION, NUMBOSS, NUMUSER, NUMUSERMOD, NUMAGD, NUMRDV, " +
                          $"                        NUMSITE, HRESUIVI, DTEDEB, DTEMOD, DTECRE, HEURE, SITE, NOMBOSS, NOMUSER) values " +
                          $"( " +
                          $"  NUMSUIVI = " + pNumSuivi +
                          $", NUMDEM = " + NUMDEM +
                          $", NUMPAT = " + NUMPAT +
                          $", NUMDESCRIPTION = " + NUMDESCRIPTION +
                          $", NUMBOSS = " + NUMBOSS +
                          $", NUMUSER = " + NUMUSER +
                          $", NUMUSERMOD = " + NUMUSER +
                          $", NUMAGD = " + NUMAGD +
                          $", NUMRDV = " + NUMRDV +
                          $", NUMSITE = " + NUMSITE +
                          $", HRESUIVI = " + HEURE +
                          $", DTEDEB = " + DTEDEB +
                          $", DTEMOD = " + DTEMOD +
                          $", DTECRE = " + DTECRE +
                          $", HEURE = " + HEURE +
                          $", SITE = " + SITE +
                          $", NOMBOSS = " + pNomBoss +
                          $", NOMUSER = " + pNomUser +
                          $")";
            return sRet;
        }

        private string UpdateSuivi()
        {
            string sRet = $" update agd_suivi set dtedeb = '{DTEDEB}', heure = '{HEURE}' where numrdv = {NUMRDV} ";
            return sRet;
        }

        private string Update()
        {
            //string sRet = $" update agd_rdv set dtedeb = '{DTEDEB}', heure = '{HEURE}' where PkGuid = '{PKGUID}'";
            string sRet = $" update agd_rdv set dtedeb = '{DTEDEB}', heure = '{HEURE}' where numrdv = {NUMRDV}";
            return sRet;
        }

    }
}
