using DataAccessLayer.CustomerOrdersDataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.SharedServices.Models.CustomerOrdersModels;

namespace BusinessLayer.CustomerOrderSerives
{
    public class CustomerOdersBusinessService : ICustomerOrdersBusinessService
    {
        private ICustomerOrdersDataService _CustomerOrdersDataService = null;
        public CustomerOdersBusinessService(ICustomerOrdersDataService CustomerOrdersDataService)
        {
            _CustomerOrdersDataService = CustomerOrdersDataService;
        }

        public async Task<CustomerOrderModel> GetRecentOrders(string user, string customerId)
        {
            return await _CustomerOrdersDataService.GetRecentOrders(user, customerId);
        }
    }
}
