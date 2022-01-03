using System;
using System.Threading.Tasks;

namespace Module3HW7.Service.Abstract
{
    public interface IJsonService
    {
        public Task<object> DeserializationAsync(string jsonObj, Type type);
    }
}
