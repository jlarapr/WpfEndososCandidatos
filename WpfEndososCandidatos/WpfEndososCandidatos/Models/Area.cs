using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEndososCandidatos.Models
{
    class Area : IEquatable<Area>, IComparable
    {

        public string AreaKey { get; set; }
        public string Desc { get; set; }
        public string Precintos { get; set; }
        public string ElectivePositionID { get; set; }
        public string DemarcationID { get; set; }
        public override string ToString()
        {
            List<string> myOut = new List<string>()
            {
               AreaKey.Trim(),
               Desc.Trim(),
               //Precintos,
               //ElectivePositionID,
               //DemarcationID
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

        public bool Equals(Area other)
        {
            if (other == null) return false;
            return (this.AreaKey.Equals(other.AreaKey));
        }

        public int CompareTo(object obj)
        {
            Area a = this;
            Area b = (Area)obj;
            return string.Compare(a.AreaKey, b.AreaKey);
        }


    }//end
}//end
