using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEndososCandidatos.Models
{
    public class Users: IEquatable<Users>, IComparable
    {
        public System.Guid UserId { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string AreasDeAcceso { get; set; }

        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public override string ToString()
        {
            List<string> myOut = new List<string>();
            string myJoined;

            myOut.Add(UserId.ToString().Trim());
            myOut.Add(UserName);
            myOut.Add(PasswordHash);
            myOut.Add(SecurityStamp);
            myOut.Add(AreasDeAcceso);
            myJoined = string.Join("-", myOut);

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
        public bool Equals(Users other)
        {
            if (other == null) return false;
            return (this.UserName.Equals(other.UserName));
        }
        public int CompareTo(object obj)
        {
            Users a = this;
            Users b = (Users)obj;
            return string.Compare(a.UserName.Trim(), b.UserName.Trim());
        }


    }
}
