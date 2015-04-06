using AuthWebAppTest.AuthRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AuthWebAppTest.Controllers
{
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
        [GTAAuthorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(Order.CreateOrders());
        }

    }

    #region Helpers

    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public string ShipperCity { get; set; }
        public Boolean IsShipped { get; set; }

        public static List<Order> CreateOrders()
        {
            List<Order> OrderList = new List<Order> 
            {
                new Order {OrderID = 10248, CustomerName = "Sandeepan", ShipperCity = "Toronto", IsShipped = true },
                new Order {OrderID = 10249, CustomerName = "Bruce", ShipperCity = "Markham", IsShipped = false},
                new Order {OrderID = 10250,CustomerName = "Somnath", ShipperCity = "Markham", IsShipped = false },
                new Order {OrderID = 10251,CustomerName = "Tubai", ShipperCity = "Davisville", IsShipped = false},
                new Order {OrderID = 10252,CustomerName = "Karan", ShipperCity = "Brampton", IsShipped = true}
            };

            return OrderList;
        }
    }

    #endregion
}
