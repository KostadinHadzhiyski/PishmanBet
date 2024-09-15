using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PishmanBet.Common
{
    public static class ValidationConstants
    {
        public static class IdentityUser
        {
            public const int UserNameMinLength = 5;
            public const int UserNameMaxLength = 50;
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;
        }
    }
}
