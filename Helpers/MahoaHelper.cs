using System.Security.Cryptography;
using System.Text;

namespace ASM.Helpers
{
    public interface IMahoaHelper
    {
        string Mahoa(string source);
    }

    public class MahoaHelper : IMahoaHelper
    {
        public string Mahoa(string source)
        {
            string hash = "";
            using (var md5Hash = MD5.Create())
            {
                var sourceBytes = Encoding.UTF8.GetBytes(source);
                var hashBytes = md5Hash.ComputeHash(sourceBytes);
                hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            }
            return hash;
        }
    }
}