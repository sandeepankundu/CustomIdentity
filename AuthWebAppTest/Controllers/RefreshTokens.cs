//using AuthWebAppTest.AuthRelated;
//using NHibernate;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Http;

//namespace AuthWebAppTest.Controllers
//{
//    [RoutePrefix("api/RefreshTokens")]
//    public class RefreshTokens : ApiController
//    {
//        private AuthRepository _repo = null;

//        private readonly ISession _session;

//        public RefreshTokens(ISession session)
//        {
//            this._session = session;
//            _repo = new AuthRepository(this._session);
//        }

//        [Authorize(Users = "Admin")]
//        [Route("")]
//        public IHttpActionResult Get()
//        {
//            return Ok(_repo.GetAllRefreshTokens());
//        }

//        //[Authorize(Users = "Admin")]
//        [AllowAnonymous]
//        [Route("")]
//        public async Task<IHttpActionResult> Delete(string tokenId)
//        {
//            var result = await _repo.RemoveRefreshToken(tokenId);
//            if (result)
//            {
//                return Ok();
//            }
//            return BadRequest("Token Id does not exist");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                _repo.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}