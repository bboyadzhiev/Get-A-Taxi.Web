namespace Get_A_Taxi.Web.Areas.Operator.ViewModels
{
    using Get_A_Taxi.Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class OrderInputVM
    {
        public int OrderId { get; set; }

        [Required]
        public double OrderLattitude { get; set; }
        [Required]
        public double OrderLongitude { get; set; }
        [StringLength(100)]
        [Display(Name="Order address")]
        public string OrderAddress { get; set; }

        public double DestinationLattitude { get; set; }

        public double DestinationLongitude { get; set; }

        [StringLength(100)]
        [Display(Name = "Destination address")]
        public string DestinationAddress { get; set; }
        [Required]
        [StringLength(20)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "User comment")]
        public string UserComment { get; set; }

        // TODO: Replace with automapper and custom mappings
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

        // TODO: Replace with automapper and custom mappings
        public static void UpdateOrderFromOperator(OrderInputVM orderVm, Order order)
        {
            order.OrderAddress = orderVm.OrderAddress;
            order.OrderLattitude = orderVm.OrderLattitude;
            order.OrderLongitude = orderVm.OrderLongitude;
            order.DestinationAddress = orderVm.DestinationAddress;
            order.DestinationLattitude = orderVm.DestinationLattitude;
            order.DestinationLongitude = orderVm.DestinationLongitude;
            order.UserComment = orderVm.UserComment;
        }

    }
}