using LearningProcess.LicenseMaker.Misc;
using LearningProcess.Licensing;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearningProcess.LicenseMaker
{
    public class MainViewModel:Misc.ANotifier
    {
        public MainViewModel()
        {
            ExpiredDate = DateTime.Now;

            SaveCommand = new Command(Save);
            OpenCommand = new Command(Open);
        }

        private string _clientName;
        private DateTime _expiredDate;
        private int _expiredSessionSeconds;
        private bool _useExpiredDate;

        public string ClientName
        {
            get => _clientName;
            set
            {
                _clientName = value;
                propertyChanged("ClientName");
            }
        }     
        
        public bool UseExpiredDate
        {
            get => _useExpiredDate;
            set
            {
                _useExpiredDate = value;
                propertyChanged("UseExpiredDate");
            }
        }

        public DateTime ExpiredDate
        {
            get => _expiredDate;
            set
            {
                _expiredDate = value;
                propertyChanged("ExpiredDate");
            }
        }

        public int ExpiredSessionSeconds
        {
            get => _expiredSessionSeconds;
            set
            {
                _expiredSessionSeconds = value;
                propertyChanged("ExpiredSessionSeconds");
            }
        }

        public ICommand SaveCommand
        { get; private set; }
        public ICommand OpenCommand
        { get; private set; }

        private void Save()
        {
            var sfd = new SaveFileDialog();

            sfd.Title = "Выберите куда сохранить файл лицензии";

            sfd.Filter = "License files|*.lp";

            if (sfd.ShowDialog().Value)
            {
                LicenseLogic.MakeLicense(new License()
                {
                    ClientName = ClientName,
                    ExpiredDate = (UseExpiredDate ? ExpiredDate : default(DateTime)),
                    ExpiredSessionSeconds = ExpiredSessionSeconds,
                },
                sfd.FileName);

                MessageBox.Show("Файл лицензии сохранен");
            }
        }

        private void Open()
        {
            var ofd = new OpenFileDialog();

            ofd.Title = "Выберите файл лицензии";

            ofd.Filter = "License files|*.lp";

            if (ofd.ShowDialog().Value)
            {
                var licenseData = LicenseLogic.GetLicense(ofd.FileName);

                ClientName = licenseData.ClientName;

                UseExpiredDate = licenseData.ExpiredDate != default(DateTime);

                if (UseExpiredDate)
                { ExpiredDate = licenseData.ExpiredDate; }
                else
                { ExpiredDate = DateTime.Now; }

                ExpiredSessionSeconds = licenseData.ExpiredSessionSeconds;
            }
        }
    }
}
