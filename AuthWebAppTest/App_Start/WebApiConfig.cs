using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace AuthWebAppTest
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();


            //config.Formatters.Clear();
            //config.Formatters.Add(new XmlMediaTypeFormatter());
            //config.Formatters.Add(new JsonMediaTypeFormatter());
            //config.Formatters.Add(new FormUrlEncodedMediaTypeFormatter());

            //i would prefer to use custom content negotiator tnad helps in simplifying the content negotialtion and also avoids the creation of formatter per request
            // refer link : http://www.strathweb.com/2013/06/supporting-only-json-in-asp-net-web-api-the-right-way/
            // refer link: http://www.strathweb.com/2012/07/everything-you-want-to-know-about-asp-net-web-api-content-negotation/
        }
    }
}
