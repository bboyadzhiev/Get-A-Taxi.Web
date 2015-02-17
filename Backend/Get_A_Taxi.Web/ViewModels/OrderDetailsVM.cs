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

        [JsonProperty(PropertyName = "custName")]
        public string CustomerName { get; set; }

        [JsonProperty(PropertyName = "custPhone")]
        public string CustomerPhoneNumber { get; set; }

        [JsonProperty(PropertyName = "startLat")]
        public double OrderLattitude { get; set; }

        [JsonProperty(PropertyName = "startLon")]
        public double OrderLongitude { get; set; }

        [JsonProperty(PropertyName = "start")]
        public string OrderAddress { get; set; }

        [JsonProperty(PropertyName = "endLat")]
        public double DestinationLattitude { get; set; }

        [JsonProperty(PropertyName = "endLon")]
        public double DestinationLongitude { get; set; }

        [JsonProperty(PropertyName = "end")]
        public string DestinationAddress { get; set; }

        [JsonProperty(PropertyName = "orderedAt")]
        public DateTime OrderedAt { get; set; }

        [JsonProperty(PropertyName = "driverPhone")]
        public string DriverPhone { get; set; }

        public static Expression<Func<Order, OrderDetailsVM>> FromOrderDataModel
        {
            get
            {
                return x => new OrderDetailsVM {
                    OrderId = x.OrderId,
                    CustomerId = x.Customer.Id,
                    TaxiId = x.AssignedTaxi.TaxiId,
                    CustomerName = x.Customer.UserName,
                    CustomerPhoneNumber = x.Customer.PhoneNumber,
                    OrderLattitude = x.OrderLattitude,
                    OrderLongitude = x.OrderLongitude,
                    OrderAddress = x.OrderAddress,
                    DestinationLattitude = x.DestinationLattitude,
                    DestinationLongitude = x.DestinationLongitude,
                    DestinationAddress = x.DestinationAddress,
                    OrderedAt = x.OrderedAt,
                    IsWaiting = (x.OrderStatus == OrderStatus.Waiting),
                    DriverPhone = (x.Driver != null) ? x.Driver.PhoneNumber : ""
                };
            }
        }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Order, OrderDetailsVM>()
                .ForMember(vm => vm.CustomerId, opt => opt.MapFrom(m => m.Customer.Id))
                .ForMember(vm => vm.TaxiId, opt => opt.MapFrom(m => m.AssignedTaxi.TaxiId))
                .ForMember(vm => vm.IsWaiting, opt => opt.MapFrom(m => m.OrderStatus == OrderStatus.Waiting))
                .ForMember(vm => vm.DriverPhone, opt => opt.MapFrom(x => (x.Driver != null) ? x.Driver.PhoneNumber : ""));
        }
    }
}