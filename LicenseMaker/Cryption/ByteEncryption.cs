using LearningProcess.LicenseMaker.ByteWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.LicenseMaker.Cryption
{
    public class ByteEncryptor : ACryption
    {
        public ByteEncryptor(byte[] bytes, int levelCount) : base(bytes)
        {
            this._levelCount = levelCount;
        }

        int _levelCount;

        public override byte[] Do()
        {
            var byteIO = new ByteIO(_byteBuffer);

            int nullPosition = createPassLengthPosition(byteIO.Length + sizeof(int));

            byteIO.Add(0, nullPosition);

            for (int i = 0; i < _levelCount; i++)
            { getEncription(byteIO); }

            return byteIO.ToByteArray();
        }

        void getEncription(ByteIO byteIO)
        {
            int passLength = getPasswordLength(byteIO.Length);

            var pass = getPassword(passLength);

            {
                int passIndex = 0;

                for (int i = 0; i < byteIO.Length; i++)
                {
                    if (passIndex >= pass.Length)
                    { passIndex = 0; }

                    var bt = byteIO[i];

                    unchecked
                    {
                        bt = encryption(bt, pass[passIndex], passIndex);
                    }

                    byteIO[i] = bt;

                    passIndex++;
                }
            }

            int passLengthPosition = createPassLengthPosition(passLength + sizeof(int) + byteIO.Length);

            byteIO.Add(pass.Length, passLengthPosition);

            byteIO.AddOnlyBytes(pass, byteIO.Length);
        }

        int getPasswordLength(int length)
        {
            var rnd = new Random();

            return rnd.Next((int)(length * _lengthFrom), (int)(length * _lengthTo));
        }

        static byte[] getPassword(int length)
        {
            var rnd = new Random();

            var bytes = new byte[length];

            for (int i = 0; i < bytes.Length; i++)
            { bytes[i] = (byte)rnd.Next(); }

            return bytes;
        }
    }
}
