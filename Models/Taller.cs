using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RegistroJATICS.Models
{
    public class Taller
    {
        [Key]
        public int ID_Taller { get; set; }
        [Required(ErrorMessage="Debes de ingresar el nombre del taller")]
        [Display(Name="Nombre del Taller")]
        public string Nombre_Taller { get; set; }
        [Display(Name="Descripción")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Ingrese el límite de participantes en el taller")]
        [Display(Name = "Cupo Máximo")]
        public int CantidadParticipantes { get; set; }
        public virtual ICollection<ApplicationUser> Usuarios { get; set; }

        [Display(Name = "Participantes Registrados")]
        public int cantRegistrados { get { return this.Usuarios != null ? 
                    this.Usuarios.Where(usu => !usu.Roles.ElementAt(0).RoleId.Equals("admin")).Count() : 0; } }

        [Display(Name = "Descripción")]
        public string resumenDescripcion { get {
                return String.IsNullOrEmpty(this.Descripcion) ? "No hay descripción disponible" :
                    (this.Descripcion.Length >= 100 ? this.Descripcion.Substring(0, 100)+"..." : this.Descripcion);
            } }
    }
}