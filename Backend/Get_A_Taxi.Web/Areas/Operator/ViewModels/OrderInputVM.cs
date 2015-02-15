namespace Get_A_Taxi.Web.Areas.Operator.ViewModels
{
    using Get_A_Taxi.Models;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OrderInputVM
    {
        [Required]
        public double OrderLattitude { get; set; }
        [Required]
        public double OrderLongitude { get; set; }
        [StringLength(50)]
        public string OrderAddress { get; set; }

        public double DestinationLattitude { get; set; }

        public double DestinationLongitude { get; set; }

        [StringLength(50)]
        public string DestinationAddress { get; set; }
        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20)]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string UserComment { get; set; }

        public static Func<OrderInputVM, ApplicationUser, Order> ToOrderDataModel
        {
            get
            {
                return (o, u) => new Order
                {
                    OrderAddress = o.OrderAddress,
                    OrderLattitude = o.OrderLattitude,
                    OrderLongitude = o.OrderLongitude,
                    OrderedAt = DateTime.Now,
                    DestinationAddress = o.DestinationAddress,
                    DestinationLattitude = o.DestinationLattitude,
                    DestinationLongitude = o.DestinationLongitude,
                    UserComment = o.UserComment,
                    OrderStatus = OrderStatus.Waiting,
                    Customer = u
                };
            }
        }
    }
}