﻿using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.ViewModels
{
    public class OrderDetailsVM //: IMapFrom<Order>, IHaveCustomMappings
    {
        [HiddenInput(DisplayValue = false)]
        public int OrderId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string CustomerId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int TaxiId { get; set; }

        public bool IsWaiting { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }

        public double OrderLattitude { get; set; }
        public double OrderLongitude { get; set; }
        public string OrderAddress { get; set; }
        
        public double DestinationLattitude { get; set; }
        public double DestinationLongitude { get; set; }
        public string DestinationAddress { get; set; }

        public DateTime OrderedAt { get; set; }

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
                    IsWaiting = (x.OrderStatus == OrderStatus.Waiting)
                };
            }
        }

        //public void CreateMappings(AutoMapper.IConfiguration configuration)
        //{
        //    configuration.CreateMap<Order, OrderDetailsVM>()
        //        .ForMember(vm=>vm.CustomerId, opt=>opt.MapFrom(m=>m.Customer.Id))
        //        .ForMember(vm=>vm.TaxiId, opt=>opt.MapFrom(m=>m.AssignedTaxi.TaxiId))
        //}
    }
}