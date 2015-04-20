namespace Get_A_Taxi.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TaxiStands")]
    public class TaxiStand
    {
        [Key]
        public int TaxiStandId { get; set; }

        //[Required]
        //public virtual Location Location { get; set; }
        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Alias { get; set; }

       // public int DistrictId { get; set; }
        public virtual District District { get; set; }

        [Required]
        public bool Active { get; set; }

        private ICollection<Taxi> taxies;

        public virtual ICollection<Taxi> Taxies
        {
            get { return taxies; }
            set { taxies = value; }
        }

        public TaxiStand()
        {
            this.Taxies = new HashSet<Taxi>();
        }

    }
}
