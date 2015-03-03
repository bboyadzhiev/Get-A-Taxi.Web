namespace Get_A_Taxi.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Districts")]
    public class District
    {
        [Key]
        public int DistrictId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public double CenterLattitude { get; set; }
        [Required]
        public double CenterLongitude { get; set; }

        [DefaultValue(10)]
        public float MapZoom { get; set; }

        private ICollection<TaxiStand> taxiStands;

        public virtual ICollection<TaxiStand> TaxiStands
        {
            get { return taxiStands; }
            set { taxiStands = value; }
        }

        private ICollection<Taxi> taxies;

        public ICollection<Taxi> Taxies
        {
            get { return taxies; }
            set { taxies = value; }
        }

        public ICollection<Order> Orders { get; set; }
        
        public District()
        {
            this.TaxiStands = new HashSet<TaxiStand>();
            this.Taxies = new HashSet<Taxi>();
            this.Orders = new HashSet<Order>();
        }

       
    }
}
