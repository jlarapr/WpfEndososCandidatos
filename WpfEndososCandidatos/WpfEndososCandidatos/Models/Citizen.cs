using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEndososCandidatos.Models
{
    class Citizen: IEquatable<Citizen>, IComparable
    {
        public string CitizenID { get; set; }
        public string FirstName { get; set; }
        public string LastName1 { get; set; }
        public string LastName2 { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string FirstGeoCode { get; set; }
        public string SecondGeoCode { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }

        public override string ToString()
        {
            List<string> myOut = new List<string>()
            {
                CitizenID ,
                FirstName ,
                LastName1 ,
                LastName2,
                FatherName ,
                MotherName ,
                FirstGeoCode ,
                SecondGeoCode ,
                DateOfBirth ,
                Gender
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
        public bool Equals(Citizen other)
        {
            if (other == null) return false;
            return (this.CitizenID.Equals(other.CitizenID));
        }
        public int CompareTo(object obj)
        {
            Citizen a = this;
            Citizen b = (Citizen)obj;
            return string.Compare(a.CitizenID, b.CitizenID);
        }

    }
}
