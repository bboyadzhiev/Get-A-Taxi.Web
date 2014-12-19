using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public virtual ApplicationUser Customer { get; set; }

        public virtual Taxi AssignedTaxi { get; set; }

        //[Required]
        //public virtual Location OrderLocation { get; set; }
        [Required]
        public double OrderLattitude { get; set; }
        [Required]
        public double OrderLongitude { get; set; }
        public string OrderAddress { get; set; }
        //public virtual Location Destination { get; set; }
        [Required]
        public double DestinationLattitude { get; set; }
        [Required]
        public double DestinationLongitude { get; set; }
        public string DestinationAddress { get; set; }
        public bool Finished { get; set; }
        public int ArrivalTime { get; set; }

        public decimal Bill { get; set; }

    }
}
