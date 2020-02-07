using LearningProcess.Licensing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LearningProcess.LicenseMaker
{
    public static class LicenseLogic
    {
        static LicenseLogic()
        { }

        public static void MakeLicense(License license, string fileName)
        {
            var bio = new ByteWork.ByteIO(10, Encoding.UTF8);
            int pos = 0;
            pos += bio.Add(license.ClientName, pos);
            pos += bio.Add(license.ExpiredDate, pos);
            pos += bio.Add(license.ExpiredSessionSeconds, pos);

            var bytes = bio.ToByteArray();
            
            var byteEncriptor = new Cryption.ByteEncryptor(bytes, new Random().Next(3, 7));

            var enciptedBytes = byteEncriptor.Do();

            File.WriteAllBytes(fileName, enciptedBytes);
        }

        public static License GetLicense(string fileName)
        {
            var methodInfo = typeof(Licensing.LicenseLogic).GetMethod("GetLicense", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            return methodInfo.Invoke(null, new object[] { fileName }) as License;
        }
    }
}
