namespace Get_A_Taxi.Web.Infrastructure.Services.Base
{
    using Get_A_Taxi.Data;

    public abstract class BaseService
    {
        protected IGetATaxiData Data { get; private set; }

        public BaseService(IGetATaxiData data)
        {
            this.Data = data;
        }
    }
}