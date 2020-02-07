using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LearningProcess.Licensing
{
    public static class LicenseLogic
    {
        static LicenseLogic()
        { }

        public static event Action<string> OnLicenseExpired;

        public static void Start()
        {
            var licenseFilename = @".\license.lp";

            if (File.Exists(licenseFilename))
            {
                _startWork = DateTime.Now;
                _license = GetLicense(licenseFilename);

                _timer = new Timer(1000);
                _timer.Elapsed += TimerElapsed;
                _timer.Enabled = true;
            }
            else
            {
                OnLicenseExpired?.Invoke("Can't find license file");
                Environment.Exit(0);
            }
        }

        private static License _license;
        private static Timer _timer;
        private static DateTime _startWork;

        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_license.ExpiredDate != null && _license.ExpiredDate.Date < DateTime.Now.Date)
            {
                _timer.Dispose();

                _timer = new Timer(5000);
                _timer.Elapsed += (s, er) => Environment.Exit(0);
                _timer.Enabled = true;

                OnLicenseExpired?.Invoke("License period expired");
            }

            if (_license.ExpiredSessionSeconds > 0)
            {
                var dateDifference = (DateTime.Now - _startWork).TotalSeconds;

                if (dateDifference > _license.ExpiredSessionSeconds)
                {
                    _timer.Dispose();

                    _timer = new Timer(5000);
                    _timer.Elapsed += (s, er) => Environment.Exit(0);
                    _timer.Enabled = true;

                    OnLicenseExpired?.Invoke("Maximum work time achieved");
                }
            }
        }

        public static License GetLicense()
        {
            return new License()
            {
                ClientName = _license.ClientName,
                ExpiredDate = _license.ExpiredDate,
                ExpiredSessionSeconds = _license.ExpiredSessionSeconds
            };
        }

        private static License GetLicense(string fileName)
        {
            var fileBytes = File.ReadAllBytes(fileName);

            var decryption = new Cryption.ByteDecryptor(fileBytes);

            var decryptedBytes = decryption.Do();

            var license = GetData(decryptedBytes);

            return license;
        }

        private static License GetData(byte[] bytes)
        {
            var br = new ByteWork.ByteReader(bytes, Encoding.UTF8);

            int pos = 0;

            var license = new License();

            license.ClientName = br.GetString(ref pos);
            license.ExpiredDate = br.GetDateTime(ref pos);
            license.ExpiredSessionSeconds = br.GetInt(ref pos);

            return license;
        }
    }
}