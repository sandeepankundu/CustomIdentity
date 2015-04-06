using AuthWebAppTest.Entities;
using AuthWebAppTest.Models;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNet.Identity;
using NHibernate;
using NHibernate.AspNet.Identity;
using NHibernate.AspNet.Identity.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AuthWebAppTest.AuthRelated
{
    public class AuthRepository : IDisposable
    {
        //private AuthContext _ctx;

        private UserManager<ApplicationUser> _userManager;
        

        public AuthRepository()
        {
            //this._session = session;
            //ISession session;
            //_ctx = new AuthContext();


            var usrEntities = new[] {
                typeof(ApplicationUser)
            };
            //ConfigurationManager.ConnectionStrings["connectionStringName"]
            //var configuration = Fluently.Configure()
            //   .Database()
            //   .ExposeConfiguration(cfg =>
            //   {
            //       cfg.AddDeserializedMapping(MappingHelper.GetIdentityMappings(myEntities), null);
            //   });
            string cnctnString = ConfigurationManager.ConnectionStrings["AuthContext"].ConnectionString;
            var sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(cnctnString)).CurrentSessionContext("web")
                //.Mappings(m => m.FluentMappings.AddFromAssemblyOf<SqlCommandFactory>())
                .ExposeConfiguration(cfg =>
                {
                    cfg.AddDeserializedMapping(MappingHelper.GetIdentityMappings(usrEntities), null);
                })
                .BuildSessionFactory();
            ISession session = sessionFactory.OpenSession();

            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(session));
            //userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(session);
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel, ApplicationUser user)
        {

            var result = await _userManager.CreateAsync(user, userModel.Password);

            //_userManager.ResetPasswordAsync("", "", "");

            return result;
        }

        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            ApplicationUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        //public Client FindClient(string clientId)
        //{
        //    var client = _ctx.Clients.Find(clientId);

        //    return client;
        //}

        //public async Task<bool> AddRefreshToken(RefreshToken token)
        //{

        //    var existingToken = _ctx.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

        //    if (existingToken != null)
        //    {
        //        var result = await RemoveRefreshToken(existingToken);
        //    }

        //    _ctx.RefreshTokens.Add(token);

        //    return await _ctx.SaveChangesAsync() > 0;
        //}

        //public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        //{
        //    var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

        //    if (refreshToken != null)
        //    {
        //        _ctx.RefreshTokens.Remove(refreshToken);
        //        return await _ctx.SaveChangesAsync() > 0;
        //    }

        //    return false;
        //}
        //public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        //{
        //    _ctx.RefreshTokens.Remove(refreshToken);
        //    return await _ctx.SaveChangesAsync() > 0;
        //}
        //public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        //{
        //    var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

        //    return refreshToken;
        //}

        //public List<RefreshToken> GetAllRefreshTokens()
        //{
        //    return _ctx.RefreshTokens.ToList();
        //}

        public async Task<ApplicationUser> FindAsync(UserLoginInfo loginInfo)
        {
            ApplicationUser user = await _userManager.FindAsync(loginInfo);

            return user;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            var result = await _userManager.CreateAsync(user);

            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);

            return result;
        }

        public void Dispose()
        {
            //_ctx.Dispose();
            _userManager.Dispose();

        }
       
    }
}