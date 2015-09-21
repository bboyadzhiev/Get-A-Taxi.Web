using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.LocalResource;
using Get_A_Taxi.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Get_A_Taxi.Web.ViewModels
{
    public class TaxiDetailsVM: TaxiVM, IHaveCustomMappings
    {
        [Required]
        [Range(1, 8)]
        [Display(Name = "SeatsCount", ResourceType = typeof(Resource))]
        public int Seats { get; set; }

        [Required]
        [Range(0, 10)]
        [Display(Name = "LuggageSize", ResourceType = typeof(Resource))]
        public int Luggage { get; set; }

        public UserDetailsVM Driver { get; set; }

        [Display(Name = "Latitude", ResourceType = typeof(Resource))]
        public double Latitude { get; set; }

         [Display(Name = "Longitude", ResourceType = typeof(Resource))]
        public double Longitude { get; set; }

        [Required]
        [Display(Name = "District", ResourceType = typeof(Resource))]
        public int AssignDistrictId { get; set; }

        public new void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            base.CreateMappings(configuration);
            configuration.CreateMap<Taxi, TaxiDetailsVM>()
                .ForMember(vm => vm.Driver, opt => opt.MapFrom(x => new UserDetailsVM()
                    {
                        Id = x.Driver.Id,
                        District = x.Driver.District.Title,
                        DistritId = x.Driver == null ? 0 : x.Driver.District.DistrictId, // !!!!!!!!!!!!
                        Email = x.Driver.Email,
                        FullName = x.Driver.FirstName + " " + x.Driver.LastName,
                        PhoneNumber = x.Driver.PhoneNumber,
                        Photo = x.Driver.Photo
                    }));
        }

        //public static Expression<Func<Taxi, TaxiDetailsVM>> FromTaxiDataModel
        //{
        //    get
        //    {
        //        return x => new TaxiDetailsVM
        //        {
        //            TaxiId = x.TaxiId,
        //            Plate = x.Plate,
        //            Seats = x.Seats,
        //            Luggage = x.Luggage,
        //            Available = x.Available,
        //            Latitude = x.Latitude,
        //            Longitude = x.Longitude,
        //            Driver = new UserDetailsVM()
        //            {
        //                Id = x.Driver.Id,
        //                District = x.Driver.District.Title,
        //                DistritId = x.Driver == null ? 0 : x.Driver.District.DistrictId, // !!!!!!!!!!!!
        //                Email = x.Driver.Email,
        //                FullName = x.Driver.FirstName + " " + x.Driver.LastName,
        //                PhoneNumber = x.Driver.PhoneNumber,
        //                Photo = x.Driver.Photo
        //            }
        //        };
        //    }
        //}

        public static Func<TaxiDetailsVM, District, Taxi> FromTaxiDetailsVM
        {
            get
            {
                return (t, d) => new Taxi
                {
                    Plate = t.Plate,
                    Seats = t.Seats,
                    Luggage = t.Luggage,
                    Status = TaxiStatus.Unassigned,
                    District = d
                };
            }
        }
    }
}