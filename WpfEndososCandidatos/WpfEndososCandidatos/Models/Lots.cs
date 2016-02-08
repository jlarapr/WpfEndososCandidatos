using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEndososCandidatos.Models
{
    public class Lots : IEquatable<Lots>, IComparable
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
        public string StatusReydi { get; set; }




        public override string ToString()
        {
            List<string> myOut = new List<string>()
            {
                Partido,
                Lot,
                Amount,
                Usercode,
                AuthDate,
                Status,
                VerDate,
                VerUser,
                FinUser,
                FinDate,
                RevDate,
                RevUser,
                conditions,
                ImportDate,
                StatusReydi,
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
        public bool Equals(Lots other)
        {
            if (other == null) return false;
            return (this.Partido.Equals(other.Partido));
        }
        public int CompareTo(object obj)
        {
            Lots a = this;
            Lots b = (Lots)obj;
            return string.Compare(a.Partido, b.Partido);
        }



    }
}
