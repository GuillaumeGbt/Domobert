using Dal = DAL.Models;
using Bll = BLL.Models;
using Common;
using BLL.Models.MQTT;


namespace BLL.Mapper
{
    public static class Mapper
    {
        #region Device
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
        #endregion

        public static TypeAndTopic toTypeAndTopic(this Dal.Device entity)
        {
            return new TypeAndTopic
            {
                Type = entity.Type.ToEDeviceType(),
                Topic = entity.TopicMQTT
            };
        }

        #region Device
        public static Dal.TempHumiData toDal(this Bll.History.TempHumiData entity)
        {
            Dal.TempHumiData newEntity = new Dal.TempHumiData();

            foreach (var prop in entity.GetType().GetProperties())
            {
                var value = prop.GetValue(entity);
                newEntity.GetType().GetProperty(prop.Name)?.SetValue(newEntity, value, null);
            }

            return newEntity;
        }

        public static Bll.History.TempHumiData toBll(this Dal.TempHumiData entity)
        {
            Bll.History.TempHumiData newEntity = new Bll.History.TempHumiData();

            foreach (var prop in entity.GetType().GetProperties())
            {
                var value = prop.GetValue(entity);
                newEntity.GetType().GetProperty(prop.Name)?.SetValue(newEntity, value, null);
            }

            return newEntity;
        }
        #endregion


    }
}
