using Bacariq.CcmDal.Models;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Bacariq.CcmDal.Helper
{

    #region ******************************************* Refresh Page
    public static class FrmEveRefreshEvent
    {
        public static void OnNavigationEvent(FrmEveRefresh_EventArgs e)
        {
            EventHandler<FrmEveRefresh_EventArgs> handler = FrmEveRefreshDePage;
            if (handler != null)
            {
                handler(handler, e);
            }
        }
        public static event EventHandler<FrmEveRefresh_EventArgs> FrmEveRefreshDePage;
    }

    public class FrmEveRefresh_EventArgs : EventArgs
    {
        public JsonData ObjectToBeRefresh { get; set; }
        public Socket InfosCnx { get; set; }
        public FrmEveRefresh_EventArgs()
        {
        }
    }
    #endregion

}
