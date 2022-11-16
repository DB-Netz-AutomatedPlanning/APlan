using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace APLan.HelperClasses
{
    public class Loading :  INotifyPropertyChanged
    {
        #region Inotify essentials
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region attributes
        private System.Timers.Timer timer;
        private int loadingIconAngle;
        private string loadingReport;
        private Visibility loadingVisibilit;
        public int LoadingIconAngle
        {
            get { return loadingIconAngle; }
            set
            {
                loadingIconAngle = value;
                OnPropertyChanged();
            }
        }
        public string LoadingReport
        {
            get { return loadingReport; }
            set
            {
                loadingReport = value;
                OnPropertyChanged();
            }
        }
        public Visibility LoadingVisibility
        {
            get { return loadingVisibilit; }
            set
            {
                loadingVisibilit = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region constructor
        public Loading()
        {
            LoadingVisibility = Visibility.Collapsed;
            timer = new System.Timers.Timer
            {
                Interval = 10,
                AutoReset = true
            };
            timer.Elapsed += Timer_Elapsed;
        }
        #endregion

        #region logic
        public void startLoading()
        {
            timer.AutoReset = true;
            LoadingIconAngle = 0;
            LoadingVisibility = Visibility.Visible;
            timer.Start();
        }

        public void stopLoading()
        {
            LoadingIconAngle = 0;
            LoadingVisibility = Visibility.Collapsed;
            timer.Stop();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            LoadingIconAngle += 10;
        }
        #endregion
    }
}
