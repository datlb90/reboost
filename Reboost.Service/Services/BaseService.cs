using Reboost.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.Service.Services
{
    public class BaseService
    {
        internal readonly IUnitOfWork _unitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
