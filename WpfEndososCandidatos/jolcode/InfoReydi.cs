using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jolcode
{
   public class InfoReydi : IEquatable<InfoReydi>, IComparable
    {
        private string _Nombre;

        public string Lot { get; set; }


        public int Num_Candidato { get; set; }
        public string Nombre
        {
            get
            {
                return _Nombre;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _Nombre = "???";
                }
                else
                    _Nombre = value;
            }
        }

        public int TotalDeEndosos { get; set; }
        public int ValidatedEndorsements { get; set; }
        public int RejectedEndorsements { get; set; }
        public string FinUser { get; set; }
        public string FinDate { get; set; }
        public string StatusReydi { get; set; }

        public override string ToString()
        {
            List<string> myOut = new List<string>()
            {
                Lot,
                Num_Candidato.ToString(),
                Nombre,
                TotalDeEndosos.ToString(),
                ValidatedEndorsements.ToString(),
                RejectedEndorsements.ToString(),
                FinUser,
                FinDate
            };

            string myJoined = string.Join(" - ", myOut);
            return myJoined;

        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public bool Equals(InfoReydi other)
        {
            if (other == null) return false;
            return (this.Lot.Equals(other.Lot));
        }
        public int CompareTo(object obj)
        {
            InfoReydi a = this;
            InfoReydi b = (InfoReydi)obj;
            return string.Compare(a.Lot, b.Lot);
        }



    }
}
