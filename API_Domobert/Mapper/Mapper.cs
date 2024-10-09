using Api = API_Domobert.Models;
using Bll = BLL.Models;
using Common;
using API_Domobert.Models;
using BLL.Models.History;

namespace API_Domobert.Mapper
{
    public static class Mapper
    {
        public static Bll.Device ToBll(this Api.AddDevice entity)
        {
            return new Bll.Device
            {
                Id = 0,
                Name = entity.Name,
                Location = entity.Location,
                Type = entity.Type.ToEDeviceType(),
            };
        }

        public static InfoDevice ToInfoDevice(this Bll.Device entity)
        {
            return new InfoDevice
            {
                Id = entity.Id,
                Name = entity.Name,
                Location = entity.Location,
                Type = entity.Type.ToStringType(),
                TypeCode = (int)entity.Type,
                TopicMQTT = entity.TopicMQTT
            };
        }

        public static DataTempHumi toDataTempHumi(this TempHumiData entity)
        {
            DataTempHumi newEntity = new DataTempHumi();

            foreach (var prop in entity.GetType().GetProperties())
            {
                var value = prop.GetValue(entity);
                newEntity.GetType().GetProperty(prop.Name)?.SetValue(newEntity, value, null);
            }

            return newEntity;
        }
    }
}
