using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEndososCandidatos.Models
{
   



    class FixVoid : IEquatable<FixVoid>, IComparable
    {


        public string Lot { get; set; } //LotsLot
        public string Formulario { get; set; }//LotsEndoFormulario
        public string LotRechazado { get; set; } //Lots.Status

        public string FormlularioRechazado {get;set;} // LotsEndo.Status 
        public string  Rechazo_Or_Warning { get; set; }//LotsVoid.Status 
        public string  TipoDeRechazo { get; set; }//LotsVoid.Rechazo 
        public string Numelec { get; set; }//LotsEndoNumelec
        public string Precinto { get; set; }//LotsEndoPrecinto
        public string FechaNac { get; set; }//LotsEndoFechaNac
        public string Sexo { get; set; }//LotsEndoSexo
        public string Candidato { get; set; }//LotsEndoCandidato
        public string Cargo { get; set; }//LotsEndoCargo
        public string Notario { get; set; }//LotsEndoNotario
        public string Conditions { get; set; }//LotsConditions

        public string LotsEndoLot { get; set; }//LotsEndoLot
        public string Batch { get; set; }//LotsEndoBatch
        public string image { get; set; }//LotsEndoimage


        public override string ToString()
        {
            List<string> myOut = new List<string>()
            {
              Lot,
              Formulario,
              LotRechazado,
              FormlularioRechazado,
              Rechazo_Or_Warning,
              TipoDeRechazo,
              Numelec,
              Precinto,
              FechaNac,
              Sexo,
              Candidato,
              Cargo,
              Notario,
              Conditions,
              LotsEndoLot,
              Batch,
              image,
            };
            string myJoined = string.Join("|", myOut);
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
        public bool Equals(FixVoid other)
        {
            if (other == null) return false;
            return (this.Lot.Equals(other.Lot));
        }
        public int CompareTo(object obj)
        {
            FixVoid a = this;
            FixVoid b = (FixVoid)obj;
            return string.Compare(a.Lot, b.Lot);
        }
    }
}
