using Dal = DAL.Models;
using Bll = BLL.Models;


namespace BLL.Mapper
{
    public static class Mapper
    {
        public static Dal.Device toDal(this Bll.Device entity)
        {
            return new Dal.Device
            {
                Id = entity.Id,
                Name = entity.Name,
                Location = entity.Location,
                Type = entity.Type,
                TopicMQTT = entity.Name + "_" + entity.Location
            };
        }

        public static Bll.Device toBll(this Dal.Device entity)
        {
            return new Bll.Device
            {
                Id = entity.Id,
                Name = entity.Name,
                Location = entity.Location,
                Type = entity.Type,
            };
        }
    }
}
