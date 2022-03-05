using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.DAL.Domains
{
    public class AppActionResult<TData, TDetail> where TDetail : class
    {
        public bool IsSuccess { get; set; }

        public TData Data { get; set; }

        public TDetail Detail { get; set; }


        public AppActionResult()
        {
            IsSuccess = false;
            Data = default(TData);
            Detail = null;
        }

        public AppActionResult(TData data)
        {
            BuildResult(data);
        }

        public AppActionResult<TData, TDetail> BuildResult(TData data, TDetail detail = null)
        {
            SetInfo(success: true, detail);
            Data = data;
            return this;
        }

        public AppActionResult<TData, TDetail> BuildError(TDetail error)
        {
            SetInfo(success: false, error);
            return this;
        }

        public AppActionResult<TData, TDetail> SetInfo(bool success, TDetail detail = null)
        {
            IsSuccess = success;
            Detail = detail;
            return this;
        }
    }
}