using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaWeb.Models
{
    public class Cerveza
    {
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage ="Ingresa el Nombre de la Cerveza")]
        [Display(Name ="Nombre Cerveza")]
        public string name { get; set; }
        [Required(ErrorMessage = "Ingresa el % de Alcohol")]
        [Display(Name = "% de Alcohol")]
        public double alcohol { get; set; }
        [Display(Name ="Estilo")]
        public int idEstilo { get; set; }
        [ForeignKey("idEstilo")]
        public Estilo? Estilo { get; set;}
        [Required(ErrorMessage = "Ingresa el Precio de la Cerveza")]
        [Display(Name = "Precio")]
        public double precio { get; set; }
    }
}
