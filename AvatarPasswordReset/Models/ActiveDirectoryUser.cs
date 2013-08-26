using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvatarPasswordReset.Models
{
    public class ActiveDirectoryUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
    }
}