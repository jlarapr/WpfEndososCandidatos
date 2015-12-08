using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEndososCandidatos.Models
{
    class Notarios: IEquatable<Notarios>, IComparable
    {
        public string NumElec { get; set; }
        public string NumCand { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Status { get; set; }

        public bool AllColumn { get; set; }
        public override string ToString()
        {
            List<string> myOut = new List<string>();
            string myJoined;

            if (AllColumn)
            {
                myOut = new List<string>()
                {
                    NumElec.Trim(),
                    NumCand.Trim(),
                    Nombre.Trim().ToUpper(),
                    Apellido1.Trim().ToUpper(),
                    Apellido2.Trim().ToUpper(),
                    Status.Trim().ToUpper()
                };
                myJoined = string.Join("-", myOut);
            }
            else
            {
                myOut = new List<string>()
                {
                    NumElec.Trim(),
                    Nombre.Trim().ToUpper(),
                    Apellido1.Trim().ToUpper(),
                    Apellido2.Trim().ToUpper(),
                    NumCand.Trim(),
                    Status
                };
                myJoined = string.Join(" - ", myOut);
            }

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
        public bool Equals(Notarios other)
        {
            if (other == null) return false;
            return (this.NumElec.Equals(other.NumElec));
        }
        public int CompareTo(object obj)
        {
            Notarios a = this;
            Notarios b = (Notarios)obj;
            return string.Compare(a.NumElec.Trim(), b.NumElec.Trim());
        }
    }//end
}//end
