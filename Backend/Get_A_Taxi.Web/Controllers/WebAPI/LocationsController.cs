using AutoMapper;
using AutoMapper.QueryableExtensions;
using Get_A_Taxi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Get_A_Taxi.Web.Models;
using Get_A_Taxi.Models;

namespace Get_A_Taxi.Web.Controllers.WebAPI
{
    [Authorize]
    [RoutePrefix("api/Locations")]
    public class LocationsController : BaseApiController, IRESTController<LocationDataModel>
    {
        public LocationsController(IGetATaxiData data)
            : base(data)
        {
        }

        /// <summary>
        /// Get user's locations
        /// </summary>
        /// <returns>List of user's locations</returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            var user = GetUser();
            var locations = user.Favorites.AsQueryable().Project().To<LocationDataModel>().ToList();
            return Ok(locations);
        }

        public IHttpActionResult Get(int id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Creates a new location
        /// </summary>
        /// <param name="model">Location's data model</param>
        /// <returns>The created location's data model</returns>
        [HttpPost]
        public IHttpActionResult Post(LocationDataModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state!");
            }

            var location = Mapper.Map<Location>(model);
            this.Data.Locations.Add(location);

            var user = GetUser();
            user.Favorites.Add(location);
            this.Data.SaveChanges();

            model.LocationId = location.LocationId;
            return Ok(model);
        }


        /// <summary>
        /// Updates location details
        /// </summary>
        /// <param name="model">The location's data model</param>
        /// <returns>The updated location's data model</returns>
        [HttpPut]
        public IHttpActionResult Put(LocationDataModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state!");
            }

            var location = this.Data.Locations.SearchFor(l => l.LocationId == model.LocationId).FirstOrDefault();
            if (location == null)
            {
                return NotFound();
            }
            var user = GetUser();
            var favoriteId = location.LocationId;

            Mapper.Map<LocationDataModel, Location>(model, location);
            this.Data.Locations.Update(location);
            this.Data.Locations.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var location = this.Data.Locations.SearchFor(l => l.LocationId == id).FirstOrDefault();
            if (location == null)
            {
                return NotFound();
            }

            this.Data.Locations.Delete(location);
            this.Data.Locations.SaveChanges();

            return Ok();
        }

      
    }
}
