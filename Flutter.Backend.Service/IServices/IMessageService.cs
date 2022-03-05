using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface IMessageService
    {
         Task<string> GetMessage(string key);
    }
}
