using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WpfEndososCandidatos.Models
{
    class Precintos : IEquatable<Precintos>, IComparable
    {
       public string PrecintoKey { get; set; }
       public string Desc { get; set; }

        public override string ToString()
        {
            List<string> myOut = new List<string>()
            {
               PrecintoKey.Trim(),
               Desc.Trim()
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

        public bool Equals(Precintos other)
        {
            if (other == null) return false;
            return (this.PrecintoKey.Equals(other.PrecintoKey));
        }

        public int CompareTo(object obj)
        {
            Precintos a = this;
            Precintos b = (Precintos)obj;
            return string.Compare (a.PrecintoKey, b.PrecintoKey);
        }
    }
}
