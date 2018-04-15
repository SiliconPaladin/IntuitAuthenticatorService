using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IntuitAuthenticatorService
{
    static class HelperExtensions
    {
        private static SHA1 _sha1Provider = new SHA1Cng();
        private static readonly byte[] _salt = new byte[20] { 0x45, 0x2a, 0x95, 0x9c, 0x21, 0xa0, 0xe5, 0x89, 0xc3, 0x17, 0x8e, 0x66, 0x1c, 0xa5, 0x2b, 0x91, 0x2a, 0x77, 0x75, 0x83 }; 
    
        internal static byte[] GetPasswordHash(this string password)
        {
            var encodedPassword = UTF8Encoding.UTF8.GetBytes(password);
            var hashedPassword = _sha1Provider.ComputeHash(encodedPassword);
            var saltedPassword = new byte[hashedPassword.Length];
            for(int i = 0; i < saltedPassword.Length; i++)
            {
                saltedPassword[i] = (byte) (hashedPassword[i] ^ _salt[i]);
            }
            return saltedPassword;
        }

        internal static bool IsEqualTo(this IEnumerable<byte> left, IEnumerable<byte> right)
        {
            if(left == null && right == null)
            {
                return true;
            }
            else if(left == null)
            {
                return false;
            }
            else if(right == null)
            {
                return false;
            }
            else if(left.Count() != right.Count())
            {
                return false;
            }

            for(int i = 0; i < left.Count(); i++)
            {
                if(left.ElementAt(i) != right.ElementAt(i))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
