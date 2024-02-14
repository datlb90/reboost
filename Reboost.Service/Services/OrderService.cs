using System;
using Microsoft.Extensions.Configuration;
using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Configuration;

namespace Reboost.Service.Services
{
    public interface IOrderService
    {
        Task<Orders> GetById(int id);
        Task<Orders> Update(Orders order);
        Task<Orders> Create(Orders order);
    }
    public class OrderService : BaseService, IOrderService
    {
        private IConfiguration _configuration;
        public OrderService(IUnitOfWork unitOfWork, IConfiguration configuration) : base(unitOfWork)
        {
            _configuration = configuration;
        }

        public async Task<Orders> GetById(int id)
        {
            return await _unitOfWork.Orders.GetByIdAsync(id);
        }

        public async Task<Orders> Update(Orders order)
        {
            return await _unitOfWork.Orders.Update(order);
        }

        public async Task<Orders> Create(Orders order)
        {
            return await _unitOfWork.Orders.Create(order);
        }

    }
}

