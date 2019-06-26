using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEndososCandidatos.Models
{
    public class LotsEndo : IEquatable<LotsEndo>, IComparable
    {
        //public String GoodNumElec { get; set; }
        //public String BadNumElec { get; set; }
        //public String GoodPartido { get; set; }
        //public String BadPartido { get; set; }
        //public String GoodLot { get; set; }
        //public String BadLot { get; set; }
        //public String GoodBatch { get; set; }
        //public String BadBatch { get; set; }
        //public String GoodFormulario { get; set; }
        //public String BadFormulario { get; set; }
        //public byte[] GoodEndosoImage { get; set; }
        //public byte[] BadEndosoImage { get; set; }
        //public String GoodPathImage { get; set; }
        //public String BadPathImage { get; set; }
        //public String GoodErrores { get; set; }
        //public String BadErrores { get; set; }
        public String NumElec   { get; set; }
        public String Partido   { get; set; }
        public String Lot       { get; set; }
        public String Batch     { get; set; }
        public String Formulario { get; set; }
        public String Image      { get; set; }
        public byte[] EndosoImage { get; set; }
        public String Errores   { get; set; }
        public String Status { get; set; }

        public override string ToString()
        {
            List<string> myout = new List<string>();
            //myout.Add(GoodNumElec);
            //myout.Add(BadNumElec);
            //myout.Add(GoodPartido);
            //myout.Add(BadPartido);
            //myout.Add(GoodLot);
            //myout.Add(BadLot);
            //myout.Add(GoodBatch);
            //myout.Add(BadBatch);
            //myout.Add(GoodFormulario);
            //myout.Add(BadFormulario);
            //myout.Add(GoodEndosoImage.ToString());
            //myout.Add(BadEndosoImage.ToString());
            //myout.Add(GoodPathImage);
            //myout.Add(BadPathImage);
            //myout.Add(GoodErrores);
            //myout.Add(BadErrores);

            myout.Add(NumElec);
            myout.Add(Partido);
            myout.Add(Lot);
            myout.Add(Batch);
            myout.Add(Formulario);
            myout.Add(Image);
            myout.Add(EndosoImage.ToString());
            myout.Add(Errores);
            myout.Add(Status);
                             

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
        public bool Equals(LotsEndo other)
        {
            if (other == null) return false;
            return (this.Lot.Equals(other.Lot));
        }
        public int CompareTo(object obj)
        {
            LotsEndo a = this;
            LotsEndo b = (LotsEndo)obj;
            return string.Compare(a.Lot, b.Lot);
        }
    }
}
