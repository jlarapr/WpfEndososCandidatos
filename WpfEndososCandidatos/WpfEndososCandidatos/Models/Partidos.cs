using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEndososCandidatos.Models
{
    public class Partidos : IEquatable<Partidos>, IComparable
    {
        public int Id { get; set; }
        public string PartidoKey { get; set; }
        public string Desc { get; set; }
        public int EndoReq { get; set; }
        public string Area { get; set; }
        public int Modo { get; set; }

        public override string ToString()
        {
            List<string> myOut = new List<string>()
            {
                PartidoKey.Trim(),
                Desc.Trim(),
                EndoReq.ToString(),
                Area.ToString(),
                Id.ToString(),
                Modo.ToString()
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
        public bool Equals(Partidos other)
        {
            if (other == null) return false;
            return (this.PartidoKey.Equals(other.PartidoKey));
        }
        public int CompareTo(object obj)
        {
            Partidos a = this;
            Partidos b = (Partidos)obj;
            return string.Compare(a.PartidoKey, b.PartidoKey);
        }


    }
}
