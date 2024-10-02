using Api = API_Domobert.Models;
using Bll = BLL.Models;


namespace API_Domobert.Mapper
{
    public static class Mapper
    {
        public static Bll.Device toBll(this Api.AddDevice entity)
        {
            return new Bll.Device
            {
                Id = 0,
                Name = entity.Name,
                Location = entity.Location,
                Type = entity.Type,
            };
        }
    }
}
