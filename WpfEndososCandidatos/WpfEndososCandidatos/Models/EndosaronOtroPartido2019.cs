using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEndososCandidatos.Models
{
    class EndosaronOtroPartido2019 : IEquatable<EndosaronOtroPartido2019>, IComparable
    {
        public int ID { get; set; }
        public int NumElec { get; set; }
        public String Partido { get; set; }

        public override string ToString()
        {
            List<string> myOut = new List<string>()
            {
                ID.ToString(),
                NumElec.ToString(),
                Partido.Trim()
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

        public bool Equals(EndosaronOtroPartido2019 other)
        {
            if (other == null) return false;
            return (this.NumElec.Equals(other.NumElec));
        }

        public int CompareTo(object obj)
        {
            EndosaronOtroPartido2019 a = this;
            EndosaronOtroPartido2019 b = (EndosaronOtroPartido2019)obj;
            return string.Compare(a.NumElec.ToString().Trim(), b.NumElec.ToString().Trim());
        }

    }
}
