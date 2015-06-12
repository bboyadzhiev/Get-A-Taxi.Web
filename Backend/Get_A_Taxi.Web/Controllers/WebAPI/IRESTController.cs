namespace Get_A_Taxi.Web.Controllers.WebAPI
{
    using System.Web.Http;

    /// <summary>
    /// Common interface for the WebAPI controllers of the system
    /// </summary>
    /// <typeparam name="T">The Data Transfer Object's (DTO's) model type</typeparam>
    public interface IRESTController<T>
    {
        IHttpActionResult Get();

        IHttpActionResult Get(int id);

        IHttpActionResult GetPaged(int page);

        IHttpActionResult Post([FromBody]T model);

        IHttpActionResult Put([FromBody]T model);

        IHttpActionResult Delete(int id);
    }
}
