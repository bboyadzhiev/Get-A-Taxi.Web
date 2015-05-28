using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Models
{
    public enum OrderStatus
    {
        /// <summary>
        /// New order waiting to be assigned to a taxi
        /// Or assigned but not yet picked up
        /// Can be cancelled by the driver (for re-assignment) or cancelled by the client
        /// </summary>
        Waiting = 0,

        /// <summary>
        /// Order assigned to a taxi and in progress (picked up)
        /// Cannot be cancelled
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// A finished order
        /// </summary>
        Finished = 2,

        /// <summary>
        /// Cancelled order
        /// </summary>
        Cancelled = 3
    }

    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public virtual ApplicationUser Customer { get; set; }

        public virtual ApplicationUser Driver { get; set; }

        public virtual Taxi AssignedTaxi { get; set; }

       [Required]
        public virtual District District { get; set; }

        [Required]
        public double OrderLatitude { get; set; }
        [Required]
        public double OrderLongitude { get; set; }
        [StringLength(100)]
        public string OrderAddress { get; set; }

        //[Required]
        public double DestinationLatitude { get; set; }
       // [Required]
        public double DestinationLongitude { get; set; }
        [StringLength(100)]
        public string DestinationAddress { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public DateTime OrderedAt { get; set; }

        public int PickupTime { get; set; }

        public int ArrivalTime { get; set; }

        public decimal Bill { get; set; }

        [StringLength(100)]
        public string UserComment { get; set; }
    }
}
