using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEndososCandidatos.Models
{
    class Criterios: IEquatable<Criterios>, IComparable
    {
        public string Campo { get; set; }
        public Nullable<bool> Editar { get; set; } 
        public string Desc { get; set; }
        public string Warning { get; set; }


        public override string ToString()
        {
            List<string> myOut = new List<string>()
            {
               Campo.Trim(),
               Editar.ToString(),
               Desc.Trim(),
               Warning.Trim()
            };
            string myJoined = string.Join("-", myOut);
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
        public bool Equals(Criterios other)
        {
            if (other == null) return false;
            return (this.Campo.Equals(other.Campo));
        }
        public int CompareTo(object obj)
        {
            Criterios a = this;
            Criterios b = (Criterios)obj;
            return string.Compare(a.Campo, b.Campo);
        }

    }
}
