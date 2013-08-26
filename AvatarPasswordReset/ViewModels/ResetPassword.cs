using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AvatarPasswordReset.ViewModels
{
    public class ResetPassword
    {
        [Required]
        [DisplayName("Avatar Username")]
        public string AvatarUserId { get; set; }

        [Required]
        [DisplayName("Avatar Name")]
        public string AvatarUserDescription { get; set; }

        [Required]
        [DisplayName("E-mail")]
        public string ADEmail { get; set; }

        [Required]
        [DisplayName("Computer Username")]
        public string ADUsername { get; set; }
    }
}