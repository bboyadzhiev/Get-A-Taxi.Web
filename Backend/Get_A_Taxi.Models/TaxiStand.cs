using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Models
{
    [Table("TaxiStands")]
    public class TaxiStand
    {
        [Key]
        public int TaxiStandId { get; set; }

        //[Required]
        //public virtual Location Location { get; set; }
        [Required]
        public double Lattitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        public string Address { get; set; }

        public string Alias { get; set; }
    }
}
