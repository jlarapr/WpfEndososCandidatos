using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEndososCandidatos.Models
{
    class Candidatos : IEquatable<Candidatos>, IComparable
    {
        public string Partido { get; set; }
        public string NumCand { get; set; }
        public string Nombre { get; set; }
        public string Area { get; set; }
        public string Cargo { get; set; }
        public string EndoReq { get; set; }

        public override string ToString()
        {
            List<string> myOut = new List<string>()
            {
                Partido,
                NumCand,
                Nombre,
                Area,
                Cargo,
                EndoReq
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
        public bool Equals(Candidatos other)
        {
            if (other == null) return false;
            return (this.NumCand.Equals(other.NumCand));
        }
        public int CompareTo(object obj)
        {
            Candidatos a = this;
            Candidatos b = (Candidatos)obj;
            return string.Compare(a.NumCand, b.NumCand);
        }


    }
}
