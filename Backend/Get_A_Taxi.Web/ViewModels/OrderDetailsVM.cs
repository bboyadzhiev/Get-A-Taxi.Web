using AutoMapper;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Mapping;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.ViewModels
{
    public class OrderDetailsVM : IMapFrom<Order>, IHaveCustomMappings
    {
        [HiddenInput(DisplayValue = false)]
        [JsonProperty(PropertyName = "orderId")]
        public int OrderId { get; set; }

        [HiddenInput(DisplayValue = false)]
        [JsonProperty(PropertyName = "custmerId")]
        public string CustomerId { get; set; }

        [HiddenInput(DisplayValue = false)]
        [JsonProperty(PropertyName = "taxiId")]
        public int TaxiId { get; set; }

        [JsonProperty(PropertyName = "isWaiting")]
        public bool IsWaiting { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "custPhone")]
        public string CustomerPhoneNumber { get; set; }

        [JsonProperty(PropertyName = "startLat")]
        public double OrderLattitude { get; set; }

        [JsonProperty(PropertyName = "startLng")]
        public double OrderLongitude { get; set; }

        [JsonProperty(PropertyName = "start")]
        public string OrderAddress { get; set; }

        [JsonProperty(PropertyName = "endLat")]
        public double DestinationLattitude { get; set; }

        [JsonProperty(PropertyName = "endLng")]
        public double DestinationLongitude { get; set; }

        [JsonProperty(PropertyName = "end")]
        public string DestinationAddress { get; set; }

        [JsonProperty(PropertyName = "orderedAt")]
        public DateTime OrderedAt { get; set; }

        [JsonProperty(PropertyName = "driverPhone")]
        public string DriverPhone { get; set; }

        [JsonProperty(PropertyName = "custComment")]
        public string CustomerComment { get; set; }

        public static Expression<Func<Order, OrderDetailsVM>> FromOrderDataModel
        {
            get
            {
                return x => new OrderDetailsVM {
                    OrderId = x.OrderId,
                    CustomerId = x.Customer.Id,
                    TaxiId = (x.AssignedTaxi != null) ? x.AssignedTaxi.TaxiId : -1,
                    //FirstName = x.Customer.UserName,
                    FirstName = x.Customer.FirstName,
                    LastName = x.Customer.LastName,
                    CustomerPhoneNumber = x.Customer.PhoneNumber,
                    OrderLattitude = x.OrderLattitude,
                    OrderLongitude = x.OrderLongitude,
                    OrderAddress = x.OrderAddress,
                    DestinationLattitude = x.DestinationLattitude,
                    DestinationLongitude = x.DestinationLongitude,
                    DestinationAddress = x.DestinationAddress,
                    OrderedAt = x.OrderedAt,
                    IsWaiting = (x.OrderStatus == OrderStatus.Waiting),// ? 1 : 0,
                    DriverPhone = (x.Driver != null) ? x.Driver.PhoneNumber : "",
                    CustomerComment = x.UserComment
                };
            }
        }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Order, OrderDetailsVM>()
                .ForMember(vm => vm.CustomerId, opt => opt.MapFrom(m => m.Customer.Id))
               // .ForMember(vm => vm.FirstName, opt => opt.MapFrom(m => m.Customer.FirstName + " " + m.Customer.LastName))
               .ForMember(vm => vm.FirstName, opt => opt.MapFrom(m => m.Customer.FirstName))
               .ForMember(vm => vm.LastName, opt => opt.MapFrom(m => m.Customer.LastName))
                .ForMember(vm => vm.TaxiId, opt => opt.MapFrom(m => (m.AssignedTaxi != null) ? m.AssignedTaxi.TaxiId : -1))
                .ForMember(vm => vm.IsWaiting, opt => opt.MapFrom(m => m.OrderStatus == OrderStatus.Waiting))// ? 1 : 0))
                .ForMember(vm => vm.DriverPhone, opt => opt.MapFrom(x => (x.Driver != null) ? x.Driver.PhoneNumber : ""))
                .ForMember(vm => vm.CustomerComment, opt => opt.MapFrom(x => x.UserComment));
        }
    }
}