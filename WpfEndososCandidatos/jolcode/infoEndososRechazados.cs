using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace jolcode
{
    public class infoEndososRechazados : IEquatable<infoEndososRechazados>, IComparable
    {//[LotsVoid]

        public string Partido { get; set; }
        public string Lot { get; set; }
        public string Batch { get; set; }
        public int Formulario { get; set; }
        public string Rechazo { get; set; }
        public string Causal { get; set; }

        public string NumElec { get; set; }
        public int Status { get; set; }
        public Byte[] EndosoImage { get; set; }
        public string Fecha_Endoso { get; set; }


        public override string ToString()
        {
            List<string> myout = new List<string>();
            myout.Add(Partido);
            myout.Add(Lot);
            myout.Add(Batch);
            myout.Add(Formulario.ToString());
            myout.Add(Rechazo);
            myout.Add(Causal);
            myout.Add(Status.ToString());
            myout.Add(EndosoImage.ToString());
            myout.Add(Fecha_Endoso);
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

        public bool Equals(infoEndososRechazados other)
        {
            if (other == null) return false;
            return (this.NumElec.Equals(other.NumElec));
        }
        public int CompareTo(object obj)
        {
            infoEndososRechazados a = this;
            infoEndososRechazados b = (infoEndososRechazados)obj;
            return string.Compare(a.NumElec, b.NumElec);
        }
    }
}
