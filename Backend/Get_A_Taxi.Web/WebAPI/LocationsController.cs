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

namespace Get_A_Taxi.Web.WebAPI
{
    /// <summary>
    /// Manages user's favorite locations
    /// </summary>
    [Authorize]
    [RoutePrefix("api/Locations")]
    public class LocationsController : BaseApiController, IRESTController<LocationDTO>
    {
        private const int RESULTS_COUNT = 10;
        /// <summary>
        /// Constructor for the Location WebAPI Controller
        /// </summary>
        /// <param name="data">Data provider instance</param>
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
            var locations = user.Favorites.AsQueryable()
                .Take(RESULTS_COUNT)
                .Project().To<LocationDTO>().ToList();
            return Ok(locations);
        }

        /// <summary>
        /// Get a page of all the locations
        /// </summary>
        /// <param name="page">The page number</param>
        /// <returns>List of user's locations</returns>
        [HttpGet]
        public IHttpActionResult GetPaged(int page)
        {
            var user = GetUser();
            var locations = user.Favorites.AsQueryable()
                .Skip(page * RESULTS_COUNT)
                .Take(RESULTS_COUNT)
                .Project().To<LocationDTO>().ToList();
            return Ok(locations);
        }

        /// <summary>
        /// Get location by id
        /// </summary>
        /// <param name="id">Location's identifier</param>
        /// <returns>A location data model</returns>
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var user = GetUser();
            var location = user.Favorites.AsQueryable().Where(l => l.LocationId == id).Project().To<LocationDTO>().FirstOrDefault();
            if (location == null)
            {
                return NotFound();
            }
            return Ok(location);
        }

        /// <summary>
        /// Creates a new location
        /// </summary>
        /// <param name="model">Location's data model</param>
        /// <returns>The created location's data model</returns>
        [HttpPost]
        public IHttpActionResult Post(LocationDTO model)
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
        /// Update user location
        /// </summary>
        /// <param name="model">The location's data model</param>
        /// <returns>The updated location's data model</returns>
        [HttpPut]
        public IHttpActionResult Put(LocationDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state!");
            }

            var user = GetUser();
            var location = this.Data.Locations.SearchFor(l => l.LocationId == model.LocationId).FirstOrDefault();
            if (location == null)
            {
                // The user has changed its location and is reporting to server
                user.Latitude = model.Latitude;
                user.Longitude = model.Longitude;
                this.Data.Users.Update(user);
                this.Data.Users.SaveChanges();

                //TODO: Notify taxi about user location change



                return Ok(model);
            }
            
            var favoriteId = location.LocationId;

            var found = user.Favorites.AsQueryable().Where(l => l.LocationId == favoriteId).FirstOrDefault();
            if (found == null)
            {
                return BadRequest("Not your location!");
            }

            Mapper.Map<LocationDTO, Location>(model, location);
            this.Data.Locations.Update(location);
            this.Data.Locations.SaveChanges();

            return Ok(model);
        }

        /// <summary>
        /// Removes a location from the user locations
        /// </summary>
        /// <param name="id">The location's id</param>
        /// <returns>The removed location's data model</returns>
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var user = GetUser();

            var location = this.Data.Locations.SearchFor(l => l.LocationId == id).FirstOrDefault();
            if (location == null)
            {
                return NotFound();
            }

            var found = user.Favorites.AsQueryable().Where(l => l.LocationId == id).FirstOrDefault();
            if (found == null)
            {
                return BadRequest("Not your location!");
            }

            var deletedLocation = Mapper.Map<LocationDTO>(location);

            this.Data.Locations.Delete(location);
            this.Data.Locations.SaveChanges();
            return Ok(deletedLocation);
        }
       
    }
}
