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

        public virtual ICollection<ApplicationUser> Usuarios { get; set; }
    }
}