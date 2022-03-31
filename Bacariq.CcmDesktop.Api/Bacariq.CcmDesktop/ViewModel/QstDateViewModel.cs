using Bacariq.CcmDal.Singleton;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bacariq.CcmDesktop.ViewModel
{
    public class QstDateViewModel : ViewModelBase, IDisposable
    {
        public QstDateViewModel()
        {
            Init();
        }

        private void Init()
        {

            isTextVisible = Visibility.Collapsed;
            isDateVisible = Visibility.Collapsed;
            isTimeVisible = Visibility.Collapsed;

            switch (SingletonDataApplication.Instance.QstDate)
            {
                case QstInit.Text:
                    isTextVisible = Visibility.Visible;
                    ToReplaceName = SingletonDataApplication.Instance.TextAFaire;
                    break;
                case QstInit.DateTime:
                    isTimeVisible = Visibility.Visible;
                    ToReplaceTime = SingletonDataApplication.Instance.TextAFaire;
                    break;
                case QstInit.Date:
                    isDateVisible = Visibility.Visible;
                    ToReplaceDate = SingletonDataApplication.Instance.TextAFaire;
                    break;
            }

        }

        public void UnInit()
        {
        }

        #region Icon *************************************************************************************
        public string BcgImg => SingletonIcone.Instance.BcgImg;
        public string IcoSauver => SingletonIcone.Instance.IcoSauver;
        #endregion


        #region Binding **********************************************************************************
        private Visibility misDateVisible;
        private Visibility misTextVisible;
        private Visibility misTimeVisible;

        private string mTextTime;
        private string mTextDate;
        private string mTextName;

        public string ToReplaceTime
        {
            get { return mTextTime; }

            set
            {
                mTextTime = value;
                RaisePropertyChanged(nameof(ToReplaceTime));
            }
        }
        public string ToReplaceDate
        {
            get { return mTextDate; }

            set
            {
                mTextDate = value;
                RaisePropertyChanged(nameof(ToReplaceDate));
            }
        }
        public string ToReplaceName
        {
            get { return mTextName; }

            set
            {
                mTextName = value;
                RaisePropertyChanged(nameof(ToReplaceName));
            }
        }

        public Visibility isTextVisible
        {
            get { return misTextVisible; }

            set
            {
                misTextVisible = value;
                RaisePropertyChanged(nameof(isTextVisible));
            }
        }
        public Visibility isDateVisible
        {
            get { return misDateVisible; }

            set
            {
                misDateVisible = value;
                RaisePropertyChanged(nameof(isDateVisible));
            }
        }
        public Visibility isTimeVisible
        {
            get { return misTimeVisible; }

            set
            {
                misTimeVisible = value;
                RaisePropertyChanged(nameof(isTimeVisible));
            }
        }

        #endregion


        #region ICommand Entete **************************************************************************
        public ICommand Cmd_Sauver { get { return new RelayCommand(() => Execute_Sauver()); } private set { } }
        public ICommand ClosedCommand { get { return new RelayCommand(OnClosed); } }
        public ICommand LoadedCommand { get { return new RelayCommand(OnLoaded); } }

        private void OnClosed()
        {
            Dispose();
        }


        private void OnLoaded()
        {
            Init();
        }
        private void Execute_Sauver()
        {

            switch (SingletonDataApplication.Instance.QstDate)
            {
                case QstInit.Text:
                    SingletonDataApplication.Instance.TextAFaire = ToReplaceName;
                    break;
                case QstInit.DateTime:
                    SingletonDataApplication.Instance.TextAFaire = ToReplaceTime;
                    break;
                case QstInit.Date:
                    SingletonDataApplication.Instance.TextAFaire = ToReplaceDate;
                    break;
            }

        }
        #endregion
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

}
