namespace Get_A_Taxi.Web.Areas.Operator.ViewModels
{
    using Get_A_Taxi.Models;
    using Get_A_Taxi.Web.Infrastructure.LocalResource;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class OrderInputVM
    {
        public int OrderId { get; set; }

        [Required]
        public double OrderLatitude { get; set; }
        [Required]
        public double OrderLongitude { get; set; }
        [StringLength(100)]
        [Display(Name = "OrderAddress", ResourceType = typeof(Resource))]
        public string OrderAddress { get; set; }

        public double DestinationLatitude { get; set; }

        public double DestinationLongitude { get; set; }

        [StringLength(100)]
        [Display(Name = "DestinationAddress", ResourceType = typeof(Resource))]
        public string DestinationAddress { get; set; }
        [Required]
        [StringLength(20)]
        [Display(Name = "FirstName", ResourceType = typeof(Resource))]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20)]
        [Display(Name = "LastName", ResourceType = typeof(Resource))]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "PhoneNumber", ResourceType = typeof(Resource))]
        public string PhoneNumber { get; set; }
        [Display(Name = "UserComment", ResourceType = typeof(Resource))]
        public string UserComment { get; set; }

        // TODO: Replace with automapper and custom mappings
        public static Func<OrderInputVM, ApplicationUser, Order> ToOrderDataModel
        {
            get
            {
                return (o, u) => new Order
                {
                    OrderAddress = o.OrderAddress,
                    OrderLatitude = o.OrderLatitude,
                    OrderLongitude = o.OrderLongitude,
                    OrderedAt = DateTime.Now,
                    DestinationAddress = o.DestinationAddress,
                    DestinationLatitude = o.DestinationLatitude,
                    DestinationLongitude = o.DestinationLongitude,
                    UserComment = o.UserComment,
                   // OrderStatus = OrderStatus.Unassigned,
                    Customer = u
                };
            }
        }

        // TODO: Replace with automapper and custom mappings
        public static void UpdateOrderFromOperator(OrderInputVM orderVm, Order order)
        {
            order.OrderAddress = orderVm.OrderAddress;
            order.OrderLatitude = orderVm.OrderLatitude;
            order.OrderLongitude = orderVm.OrderLongitude;
            order.DestinationAddress = orderVm.DestinationAddress;
            order.DestinationLatitude = orderVm.DestinationLatitude;
            order.DestinationLongitude = orderVm.DestinationLongitude;
            order.UserComment = orderVm.UserComment;
        }

    }
}