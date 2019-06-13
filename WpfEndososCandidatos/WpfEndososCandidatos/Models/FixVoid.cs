using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEndososCandidatos.Models
{
   



    class FixVoid : IEquatable<FixVoid>, IComparable
    {

        public int i { get; set; }
        public string CurrElect { get; set; }
        public string Lot { get; set; } //LotsLot
        public string Formulario { get; set; }//LotsEndoFormulario
        public string Nombre { get; set; }
        public string FirstName { get; set; }


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
        public bool chkOtraRazonDeRechazo { get; set; }
        public string txtOtraRazonDeRechazo { get; set; }
        //  public DateTime? FchEndoso { get; set; }

        public DateTime? Firma_Fecha_Notario { get; set; }
        public DateTime? Fecha_Recibo_CEE { get;set; } //endosos entregado a la cee
        public DateTime? Firma_Fecha_Elector { get; set; }

        public string Batch { get; set; }//LotsEndoBatch
        public string image { get; set; }//LotsEndoimage
        public byte[] EndosoImage { get; set; }
        public int? Leer_Inv { get; set; }

        public override string ToString()
        {
            List<string> myOut = new List<string>()
            {
                i.ToString(),
                CurrElect,
                Nombre,
                FirstName,
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
              chkOtraRazonDeRechazo.ToString(),
              txtOtraRazonDeRechazo,
              Firma_Fecha_Notario.ToString(),
              Firma_Fecha_Elector.ToString(),
              Fecha_Recibo_CEE.ToString(),
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
            return (this.i.Equals(other.i));
        }
        public int CompareTo(object obj)
        {
            FixVoid a = this;
            FixVoid b = (FixVoid)obj;
            return string.Compare(a.i.ToString(), b.i.ToString());
        }
    }
}
