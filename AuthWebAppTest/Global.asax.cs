using AuthWebAppTest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace AuthWebAppTest
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //Application["NHSessionFactory"] = CreateSessionFactory();

        }

        //private static ISessionFactory CreateSessionFactory()
        //{
        //    return Fluently.Configure()
        //        .Database(MsSqlConfiguration.MsSql2008
        //            .ConnectionString(ConfigurationManager.AppSettings["AuthContext"]))
        //        .Mappings(m => m.AutoMappings.Add(CreateAutomappings))
        //        .BuildSessionFactory();
        //}

        //private static AutoPersistenceModel CreateAutomappings()
        //{
        //    return AutoMap
        //        .AssemblyOf<AutomappingConfiguration>(new AutomappingConfiguration())
        //        .Override<UserModel>(u => u.Table("tUser"));
        //}

        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    ISessionFactory sessionFactory = (ISessionFactory)Application["NHSessionFactory"];
        //    Context.Items["NHSession"] = sessionFactory.OpenSession();
        //}

        //protected void Application_EndRequest(object sender, EventArgs e)
        //{
        //    ISession session = (ISession)Context.Items["NHSession"];
        //    session.Dispose();
        //}

        //protected void Application_End(object sender, EventArgs e)
        //{
        //    ISessionFactory sessionFactory = (ISessionFactory)Application["NHSessionFactory"];
        //    sessionFactory.Dispose();
        //}
    }
}
