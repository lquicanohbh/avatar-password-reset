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
    public class ActiveDirectoryUserApiController : ApiController
    {
        private readonly ActiveDirectoryUserRepository _activeDirectoryRepository;
        public ActiveDirectoryUserApiController()
        {
            _activeDirectoryRepository = new ActiveDirectoryUserRepository();
        }
        public ActiveDirectoryUserApiController(ActiveDirectoryUserRepository adRepository)
        {
            _activeDirectoryRepository = adRepository;
        }
        [HttpGet]
        public ActiveDirectoryUser Get(string Id)
        {
            var ADUser = _activeDirectoryRepository.GetUserById(Id);
            return ADUser;
        }
    }
}
