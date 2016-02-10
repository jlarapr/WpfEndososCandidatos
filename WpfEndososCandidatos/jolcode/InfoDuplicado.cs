using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jolcode
{
  public  class InfoDuplicado : IEquatable<InfoDuplicado>, IComparable
    {
        public string Lot { get; set; }
        public string Batch { get; set; }
        public string Formulario { get; set; }
        public string NumElec { get; set; }
        public string Cargo { get; set; }

        public DateTime? VerDate { get; set; }
        public DateTime? FinDate { get; set; }
        public string StatusReydi { get; set; }
        public string Status { get; set; }
        public string LotDuplicado { get; set; }
        public override string ToString()
        {
            List<string> myout = new List<string>()
            {
                Lot,
                Batch,
                Formulario,
                NumElec,
                Cargo,
                VerDate.Value.ToShortDateString(),
                FinDate.Value.ToShortDateString(),
                StatusReydi,
                Status,
                LotDuplicado
                
            };
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

        public bool Equals(InfoDuplicado other)
        {
            if (other == null) return false;
            return (this.Lot.Equals(other.Lot));
        }
        public int CompareTo(object obj)
        {
            InfoDuplicado a = this;
            InfoDuplicado b = (InfoDuplicado)obj;
            return string.Compare(a.Lot, b.Lot);
        }
    }
}
