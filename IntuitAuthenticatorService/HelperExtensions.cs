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
    
        internal static byte[] GetPasswordHash(this string password)
        {
            var encodedPassword = UTF8Encoding.UTF8.GetBytes(password);
            return _sha1Provider.ComputeHash(encodedPassword);
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
