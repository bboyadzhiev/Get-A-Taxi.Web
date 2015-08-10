using Get_A_Taxi.Web.Models;
using System;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Get_A_Taxi.Models;
using Get_A_Taxi.Data;

namespace Get_A_Taxi.Web.Controllers.WebAPI
{
    /// <summary>
    /// Manages user's photos
    /// </summary>
    [Authorize]
    [RoutePrefix("api/Photos")]
    public class PhotosController : BaseApiController, IRESTController<PhotoDTO>
    {

        /// <summary>
        /// Constructor for the Photos WebAPI Controller
        /// </summary>
        /// <param name="data">Data provider instance</param>
        public PhotosController(IGetATaxiData data)
            : base(data)
        {
        }
        /// <summary>
        /// Get user's account photo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            var user = GetUser();

            var photo = this.Data.Photos
                .All()
                .Where(p => p.Id == user.Photo.Id)
                .Project().To<PhotoDTO>().FirstOrDefault();

            if (photo == null)
            {
                return NotFound();
            }
            return Ok(photo);
        }

        /// <summary>
        /// Get specific photo
        /// </summary>
        /// <param name="id">Photo's Id</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var photo = this.Data.Photos
                .All()
                .Where(p => p.Id == id)
                .Project().To<PhotoDTO>().FirstOrDefault();

            if (photo == null)
            {
                return NotFound();
            }
            return Ok(photo);
        }

        [HttpGet]
        public IHttpActionResult GetPaged(int page)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IHttpActionResult Post(PhotoDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state!");
            }

            var photo = Mapper.Map<Photo>(model);
            this.Data.Photos.Add(photo);

            var user = GetUser();
            user.Photo = photo;
            this.Data.SaveChanges();
            return Ok(photo.Id);
        }

        [HttpPut]
        public IHttpActionResult Put(PhotoDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state!");
            }

            var user = GetUser();
            var photo = this.Data.Photos.SearchFor(p => p.Id == model.Id).FirstOrDefault();

            photo = Mapper.Map<Photo>(model);
            this.Data.Photos.Add(photo);
            user.Photo = photo;
            this.Data.SaveChanges();

            return Ok(model);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var user = GetUser();
            var photo = this.Data.Photos.SearchFor(p => p.Id == id).FirstOrDefault();
            if (photo == null)
            {
                return NotFound();
            }

            
            if (photo.Id != user.Photo.Id)
            {
                return BadRequest("Not your photo!");
            }

            this.Data.Photos.Delete(photo);
            this.Data.Photos.SaveChanges();
            return Ok(id);
        }
    }
}
