using BLL.Models.History;
using Common;
using mDal = DAL.Models;
using sDal = DAL.Services;
using mBll = BLL.Models.History;
using sBll = BLL.Services;
using BLL.Mapper;

namespace BLL.Services
{
    public class HistoryTempService
    {
        private sDal.HistoryTempService _service;

        public HistoryTempService(sDal.HistoryTempService service)
        {
            _service = service;
        }

        public IEnumerable<TempHumiData> GetAll(int deviceId)
        {
            return _service.GetAll(deviceId).Select(e => e.toBll());
        }

        public int Add(mBll.TempHumiData data)
        {
            return _service.Add(data.toDal());
        }

        public bool Delete(int id)
        {
            return _service.Delete(id);
        }
    }
}

