﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Models
{
    [Table("Taxies")]
    public class Taxi
    {
        [Key]
        public int TaxiId { get; set; }

        [Required]
        public string Plate { get; set; }

        public virtual District District { get; set; }

        public virtual TaxiStand TaxiStand { get; set; }

        [Required]
        public int Seats { get; set; }
        
        [Required]
        public int Luggage { get; set; }

        [DefaultValue(true)]
        public bool Available { get; set; }

        public double Lattitude { get; set; }
        public double Longitude { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        public virtual ApplicationUser Driver { get; set; }
    }
}
