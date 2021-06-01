using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Core.Configuration
{
    public class JwtAuthenticationOptions
    {
        public const string JwtAuthentication = "JwtAuthentication";

        public string SecretKey { get; set; }
    }
}
