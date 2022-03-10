using Flutter.Backend.DAL.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface IUploadImageService
    {
        Task<AppActionResultMessage<string>> UploadImage(string image); 
    }
}
