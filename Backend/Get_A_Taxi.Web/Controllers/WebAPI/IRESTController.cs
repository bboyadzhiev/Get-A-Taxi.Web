namespace Get_A_Taxi.Web.Controllers.WebAPI
{
    using System.Web.Http;
    public interface IRESTController<T>
    {
        IHttpActionResult Get();

        IHttpActionResult Get(int id);

        IHttpActionResult Post([FromBody]T model);

        IHttpActionResult Put([FromBody]T model);

        IHttpActionResult Delete(int id);
    }
}
