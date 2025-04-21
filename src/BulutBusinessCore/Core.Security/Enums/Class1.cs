using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulutBusinessCore.Core.Security.Enums;
public enum AuthenticatorType
{
    None = 0,
    Email = 1,
    Otp = 2,
    Sms = 3
}