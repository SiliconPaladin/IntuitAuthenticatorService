using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntuitAuthenticatorService
{
    sealed class PasswordResetRequest
    {
        static Random _rng = new Random();

        internal int RequestId { get; }

        internal DateTime RequestTime { get; }

        internal PasswordResetRequest()
        {
            RequestId = _rng.Next(int.MinValue, int.MaxValue);
            RequestTime = DateTime.UtcNow;
        }
    }
}
