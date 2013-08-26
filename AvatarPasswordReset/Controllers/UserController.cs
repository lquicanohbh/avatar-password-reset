using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AvatarPasswordReset.Helpers;
using AvatarPasswordReset.Repositories;
using AvatarPasswordReset.ViewModels;

namespace AvatarPasswordReset.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        private readonly AvatarUserRepository _avatarUserRepository;
        public UserController()
        {
            _avatarUserRepository = new AvatarUserRepository();
        }
        public UserController(AvatarUserRepository avatarUserRepository)
        {
            _avatarUserRepository = avatarUserRepository;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(ResetPassword resetPasswordViewModel)
        {
            try
            {
                if (resetPasswordViewModel.ADEmail != null)
                {
                    var response = _avatarUserRepository.ResetPassword(resetPasswordViewModel.AvatarUserId);
                    if (response != null)
                    {
                        if (response.Status == 1)
                        {
                            _avatarUserRepository.LogReset(Request, resetPasswordViewModel);
                            Helper.EmailPassword(resetPasswordViewModel.ADEmail, response.Message);
                            ViewBag.Message = String.Format("Your new password was e-mailed to {0}.",
                                resetPasswordViewModel.ADEmail);
                        }
                        else
                        {
                            ViewBag.Message = String.Format("Password could not be reset: {0}, please try again, " +
                                            "if this problem persists contact the IT department.", response.Message);

                        }
                    }
                }
                else
                {
                    ViewBag.Message = String.Format("E-mail not found");
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
    }
}
