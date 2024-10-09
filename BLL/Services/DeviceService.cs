using Common;
using Dal = DAL.Models;
using Bll = BLL.Models;
using BLL.Mapper;


namespace BLL.Services
{
    public class DeviceService : IDeviceRepository<Bll.Device>
    {

        private IDeviceRepository<Dal.Device> _repository;
        private MqttConfigService _mqttConfig;

        public DeviceService(IDeviceRepository<Dal.Device> repository, MqttConfigService mqttConfigService)
        {
            _repository = repository; 
            _mqttConfig = mqttConfigService;
        }

        public IEnumerable<Bll.Device> GetAll()
        {
            return _repository.GetAll().Select(dal => dal.toBll());
        }

        public Bll.Device GetById(int id)
        {
            return _repository.GetById(id).toBll();
        }

        public int Add(Bll.Device device)
        {
            Dal.Device newDevice = device.toDal();

            if (_repository.GetAll().Any(dal => newDevice.TopicMQTT == dal.TopicMQTT))
                throw new ArgumentException(
                    $"Combination of name and location should be unique." +
                    $"\n {newDevice.Name} already exist in {newDevice.Location}");
            _mqttConfig.AddTopic(newDevice.toTypeAndTopic());
            return _repository.Add(newDevice);
        }

        public bool Update(int id, Bll.Device device)
        {
            return _repository.Update(id, device.toDal());
        }

        public bool Delete(int id)
        {
            _mqttConfig.AddTopic(_repository.GetById(id).toTypeAndTopic());
            return _repository.Delete(id);
        }
    }
}
