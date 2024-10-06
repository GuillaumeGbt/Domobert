using Dal = DAL.Models;
using Bll = BLL.Models;
using Common;


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
                Type = entity.Type.ToStringType(),
                TopicMQTT = entity.Location + "/" + 
                            entity.Name.Replace(" ", "_") + "/" +
                            entity.Type.ToStringType()
            };
        }

        public static Bll.Device toBll(this Dal.Device entity)
        {
            return new Bll.Device
            {
                Id = entity.Id,
                Name = entity.Name,
                Location = entity.Location,
                Type = entity.Type.ToEDeviceType(),
                TopicMQTT = entity.TopicMQTT
            };
        }
    }
}
