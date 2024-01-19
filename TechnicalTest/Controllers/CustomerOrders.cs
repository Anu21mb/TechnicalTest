using BusinessLayer.CustomerOrderSerives;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerOrders : ControllerBase
    {
        private readonly ICustomerOrdersBusinessService _CustomerOrdersBusinessServices;
        public CustomerOrders(ICustomerOrdersBusinessService commonBusinessServices)
        {
            _CustomerOrdersBusinessServices = commonBusinessServices;
        }

        [HttpPost("GetRecentOrders")]
        public async Task<IActionResult> GetRecentOrders([FromQuery] string user, string customerId)
        {
            var result = await _CustomerOrdersBusinessServices.GetRecentOrders(user, customerId);
            return Ok(result);
        }
    }
}
