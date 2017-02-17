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
    }
}