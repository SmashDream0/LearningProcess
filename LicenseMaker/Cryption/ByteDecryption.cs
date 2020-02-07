using LearningProcess.LicenseMaker.ByteWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.LicenseMaker.Cryption
{
    public class ByteDecryptor : ACryption
    {
        public ByteDecryptor(byte[] bytes) : base(bytes)
        { }

        public override byte[] Do()
        {
            var byteIO = new ByteIO(_byteBuffer);

            while (getDecryption(byteIO))
            { }

            return byteIO.ToByteArray();
        }

        bool getDecryption(ByteIO byteIO)
        {
            var passLengthPosition = createPassLengthPosition(byteIO.Length);

            var passLength = byteIO.GetInt(ref passLengthPosition);

            passLengthPosition -= sizeof(int);

            byteIO.RemoveBytes(passLengthPosition, sizeof(int));

            if (passLength == 0)
            { return false; }
            else
            {
                var password = getPassword(byteIO, passLength);

                byteIO.Length -= password.Length;

                int passIndex = 0;

                for (int i = 0; i < byteIO.Length; i++)
                {
                    if (passIndex >= password.Length)
                    { passIndex = 0; }

                    var bt = byteIO[i];

                    unchecked
                    {
                        bt = decryption(bt, password[passIndex], passIndex);
                    }

                    byteIO[i] = bt;

                    passIndex++;
                }

                return true;
            }
        }

        int getPasswordLength(int length)
        {
            var rnd = new Random();

            return rnd.Next((int)(length * _lengthFrom), (int)(length * _lengthTo));
        }

        static byte[] getPassword(ByteIO byteIO, int passLength)
        {
            int passPosition = byteIO.Length - passLength;

            return byteIO.GetBytes(ref passPosition, passLength);
        }

        static byte[] getPassword(int length)
        {
            var rnd = new Random();

            var bytes = new byte[length];

            int randomWindow = rnd.Next(0, length);

            byte[] buffer = new byte[length];

            Buffer.BlockCopy(buffer, randomWindow, buffer, 0, length - randomWindow);
            Buffer.BlockCopy(buffer, 0, buffer, length - randomWindow, randomWindow);

            return bytes;
        }
    }
}
