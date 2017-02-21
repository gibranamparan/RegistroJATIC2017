using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RegistroJATICS.Models
{
    public class Institucion
    {
        [Key]
        [Display(Name = "Institución")]
        public string Nombre { get; set; }
        public virtual ICollection<ApplicationUser> Usuarios { get; set; }

        [Display(Name = "Visitantes Registrados")]
        public int visitantesRegistrados{get{
                return this.Usuarios.Where(usu => !usu.Roles.ElementAt(0).RoleId.Equals("admin")).Count();
            } }
    }
}