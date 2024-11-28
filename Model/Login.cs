using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace prjIcedOutWheelz.Model
{
    public class Login
    {
        public string Naam {  get; set; }
        public string Email { get; set; }
        public string Wachtwoord { get; set; }
        public string TelNummer { get; set; }
        public string Straat_Num { get; set; }
        public string Gemeente_Postc { get; set; }
    }
}
