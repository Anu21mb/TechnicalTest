using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.SharedServices.Models.CustomerOrdersModels;

namespace DataAccessLayer.CustomerOrdersDataServices
{
    public interface ICustomerOrdersDataService
    {
        Task<CustomerOrderModel> GetRecentOrders(string user, string customerId);
    }
}
