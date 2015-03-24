﻿using AutoMapper;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Mapping;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Models
{
    public class OrderDetailsDTO : OrderDTO, IHaveCustomMappings
    {
        [JsonProperty(PropertyName = "custmerId")]
        public string CustomerId { get; set; }

        [JsonProperty(PropertyName = "isWaiting")]
        public bool IsWaiting { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "custPhone")]
        public string CustomerPhoneNumber { get; set; }

        [JsonProperty(PropertyName = "orderedAt")]
        public DateTime OrderedAt { get; set; }


        // Properties, updated by taxi driver
        [JsonProperty(PropertyName = "taxiId")]
        public int TaxiId { get; set; }

        [JsonProperty(PropertyName = "isFinished")]
        public bool IsFinished { get; set; }

        [JsonProperty(PropertyName = "pickupTime")] // in minutes
        public int PickupTime { get; set; }

        [JsonProperty(PropertyName = "arrivalTime")] // in minutes
        public int ArrivalTime { get; set; }

        [JsonProperty(PropertyName = "bill")]
        public decimal Bill { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Order, OrderDetailsDTO>()
                  .ForMember(vm => vm.CustomerId, opt => opt.MapFrom(m => m.Customer.Id))
                 .ForMember(vm => vm.FirstName, opt => opt.MapFrom(m => m.Customer.FirstName))
                 .ForMember(vm => vm.LastName, opt => opt.MapFrom(m => m.Customer.LastName))
                  .ForMember(vm => vm.TaxiId, opt => opt.MapFrom(m => (m.AssignedTaxi != null) ? m.AssignedTaxi.TaxiId : -1))
                  .ForMember(vm => vm.IsWaiting, opt => opt.MapFrom(m => m.OrderStatus == OrderStatus.Waiting))// ? 1 : 0))
                  .ForMember(vm => vm.IsFinished, opt => opt.MapFrom(m => m.OrderStatus == OrderStatus.Finished))
                  .ForMember(vm => vm.PickupTime, opt => opt.MapFrom(m => m.PickupTime));
            
           
        }



    }
}