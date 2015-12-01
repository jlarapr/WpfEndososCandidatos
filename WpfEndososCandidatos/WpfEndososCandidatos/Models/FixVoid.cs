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
       
        
        
        public string  TipoDeRechazo { get; set; }//LotsVoid.Rechazo 
        public string Numelec { get; set; }//LotsEndoNumelec
        public string NotarioElec { get; set; }

        public string Precinto { get; set; }//LotsEndoPrecinto
        public DateTime? FechaNac { get; set; }//LotsEndoFechaNac
        public string Sexo { get; set; }//LotsEndoSexo
        public string Candidato { get; set; }//LotsEndoCandidato
        public string Cargo { get; set; }//LotsEndoCargo

        public string FirmaElec { get; set; }
        public string NotarioFirma { get; set; }
        public bool Firma_Pet_Inv { get; set; }
        public bool Firma_Not_Inv { get; set; }
        public DateTime? FchEndoso { get; set; }
        public DateTime? Firma_Fecha { get; set; }
        public DateTime? FchEndosoEntregada { get;set; }
        public string Batch { get; set; }//LotsEndoBatch
        public string image { get; set; }//LotsEndoimage
        

        public override string ToString()
        {
            List<string> myOut = new List<string>()
            {
              Lot,
              Formulario,
              TipoDeRechazo,
              Numelec,
              NotarioElec,
              Precinto,
              FechaNac.ToString(),
              Sexo,
              Candidato,
              Cargo,
              FirmaElec,
              NotarioFirma,
              Firma_Pet_Inv.ToString(),
              Firma_Not_Inv.ToString(),
              FchEndoso.ToString(),
              Firma_Fecha.ToString(),
              FchEndosoEntregada.ToString(),
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
