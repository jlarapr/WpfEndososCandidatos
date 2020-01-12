using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEndososCandidatos.Models
{
    class Modo : IEquatable<Modo>, IComparable
    {
        public int ModoId { get; set; }
        public string Desc { get; set; }

        public override string ToString()
        {
            List<string> myOut = new List<string>()
            {
                ModoId.ToString(),
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
        public bool Equals(Modo other)
        {
            if (other == null) return false;
            return (this.ModoId.Equals(other.ModoId));
        }
        public int CompareTo(object obj)
        {
            Modo a = this;
            Modo b = (Modo)obj;
            return string.Compare(a.ModoId.ToString(), b.ModoId.ToString());
        }
    }
}
