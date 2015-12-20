using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jolcode
{
 public   class LotsToReydi : IEquatable<LotsToReydi>, IComparable
    {
        public string Partido { get; set; }
        public string Lot { get; set; }
        public string Amount { get; set; }
        public string Usercode { get; set; }
        public string AuthDate { get; set; }
        public string Status { get; set; }
        public string VerDate { get; set; }
        public string VerUser { get; set; }
        public string FinUser { get; set; }
        public string FinDate { get; set; }
        public string RevDate { get; set; }
        public string RevUser { get; set; }
        public string conditions { get; set; }
        public string ImportDate { get; set; }

        public string EndorsementGroupCode { get; set; } // == Lot
        public int ValidatedEndorsements { get; set; }
        public int RejectedEndorsements { get; set; }
        public string EndorsementValidationDate { get; set; }
        public string Num_Candidato { get; set; }

        public override string ToString()
        {
            //List<string> myOut = new List<string>()
            //{
            //    Partido,
            //    Lot,
            //    Amount,
            //    Usercode,
            //    AuthDate,
            //    Status,
            //    VerDate,
            //    VerUser,
            //    FinUser,
            //    FinDate,
            //    RevDate,
            //    RevUser,
            //    conditions,
            //    ImportDate,
            //};
            //string myJoined = string.Join(" - ", myOut);
            //return myJoined;
            return this.Lot;

        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public bool Equals(LotsToReydi other)
        {
            if (other == null) return false;
            return (this.Lot.Equals(other.Lot));
        }
        public int CompareTo(object obj)
        {
            LotsToReydi a = this;
            LotsToReydi b = (LotsToReydi)obj;
            return string.Compare(a.Lot, b.Lot);
        }
    }
}
