using System.Security.Cryptography;
using System.Text;

namespace CoreHome.Utils
{
    public class SecurityUtil
    {

        public static string SHA256Encrypt(string str)
        {
            byte[] password = SHA256.HashData(Encoding.UTF8.GetBytes(str));
            StringBuilder builder = new();
            for (int i = 0; i < password.Length; i++)
            {
                _ = builder.Append(password[i].ToString("X2"));
            }
            return builder.ToString();
        }


    }
}
