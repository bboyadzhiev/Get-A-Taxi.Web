using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Models
{
    /// <summary>
    /// Possible states of a taxi
    /// </summary>
    public enum TaxiStatus
    {
        /// <summary>
        /// On duty, available to be assigned to an order
        /// </summary>
        Available = 0,

        /// <summary>
        /// On duty, assigned to an order or something else
        /// </summary>
        Busy = 1,

        /// <summary>
        /// Off-duty - availabe for decommissioning or driver assignment
        /// </summary>
        OffDuty = 2,

        /// <summary>
        /// Decommissioned and off-duty, driver can not be assigned
        /// </summary>
        Decommissioned = 3
    }

    [Table("Taxies")]
    public class Taxi
    {
        [Key]
        public int TaxiId { get; set; }

        [Required]
        [StringLength(10)]
        public string Plate { get; set; }

        public virtual District District { get; set; }

        public virtual TaxiStand TaxiStand { get; set; }

        [Required]
        public int Seats { get; set; }
        
        [Required]
        public int Luggage { get; set; }

        [DefaultValue(TaxiStatus.OffDuty)]
        public TaxiStatus Status { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        public virtual ApplicationUser Driver { get; set; }
    }
}
