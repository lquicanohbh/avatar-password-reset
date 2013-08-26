using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AvatarPasswordReset.Models;
using AvatarPasswordReset.Repositories;

namespace AvatarPasswordReset.Controllers
{
    public class AvatarUserApiController : ApiController
    {
        private readonly AvatarUserRepository _userRepository;
        public AvatarUserApiController()
        {
            _userRepository = new AvatarUserRepository();
        }
        public AvatarUserApiController(AvatarUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet]
        public IEnumerable<AvatarUser> Get(string query = "")
        {
            var AvatarUsers = _userRepository.SearchUser(query);
            return AvatarUsers.ToArray();
        }
    }
}
