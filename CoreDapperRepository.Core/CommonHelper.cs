using System.Text;
using System.Security.Cryptography;
using CoreDapperRepository.Core.Domain.Security;

namespace CoreDapperRepository.Core
{
    public class CommonHelper
    {
        public static byte[] GetHash(string inputString, HashSignType hashSignType = HashSignType.SHA1)
        {
            HashAlgorithm algorithm = null;

            switch (hashSignType)
            {
                case HashSignType.SHA1:
                    algorithm = SHA1.Create();
                    break;
                case HashSignType.SHA256:
                    algorithm = SHA256.Create();
                    break;
                case HashSignType.SHA384:
                    algorithm = SHA384.Create();
                    break;
                case HashSignType.SHA512:
                    algorithm = SHA512.Create();
                    break;
            }

            if (algorithm != null)
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));

            return new byte[0];
        }

        public static string GetHashString(string inputString, HashSignType hashSignType = HashSignType.SHA1)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
