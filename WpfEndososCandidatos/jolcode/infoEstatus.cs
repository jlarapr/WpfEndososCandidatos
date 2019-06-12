using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jolcode
{
    public class infoEstatus : IEquatable<infoEstatus>, IComparable
    {
        public string Partido { get; set; }
        public int Endosos_Requeridos { get; set; }
        public int Endosos_Aceptados { get; set; }
        public int Endosos_Rechazados { get; set; }
        public int Endosos_Procesados { get; set; }
        public int Endosos_Entregados { get; set; }

        public override string ToString()
        {
            List<string> myout = new List<string>();
            myout.Add(Partido);
            myout.Add(Endosos_Requeridos.ToString());
            myout.Add(Endosos_Aceptados.ToString());
            myout.Add(Endosos_Rechazados.ToString());
            myout.Add(Endosos_Procesados.ToString());
            myout.Add(Endosos_Entregados.ToString());
            string myJoined = string.Join("|", myout);
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
        public bool Equals(infoEstatus other)
        {
            if (other == null) return false;
            return (this.Partido.Equals(other.Partido));
        }
        public int CompareTo(object obj)
        {
            infoEstatus a = this;
            infoEstatus b = (infoEstatus)obj;
            return string.Compare(a.Partido, b.Partido);
        }


    }
}
