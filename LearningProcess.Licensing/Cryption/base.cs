using LearningProcess.Licensing.ByteWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.Licensing.Cryption
{
    public abstract class ACryption : AByteWork
    {
        public ACryption(byte[] bytes) : base(bytes, null)
        { }

        protected const float _lengthPosition = 0.3f;

        protected const float _lengthFrom = 0.3f;

        protected const float _lengthTo = 0.6f;

        public abstract byte[] Do();

        protected int createPassLengthPosition(int length)
        { return (int)(length * _lengthPosition); }

        protected static byte encryption(byte decryptedByte, int password1, int password2)
        {
            var password3 = (password2 == 0 ? 1 : password2);

            return (byte)(decryptedByte + (password1 * password3 - password2));
        }

        protected static byte decryption(byte encryptedByte, int password1, int password2)
        {
            var password3 = (password2 == 0 ? 1 : password2);

            unchecked
            { return (byte)(encryptedByte - (password1 * password3 - password2)); }
        }
    }
}
