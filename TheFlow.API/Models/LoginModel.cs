using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFlow.API.Models
{
    [Serializable]
    public class LoginModel
    {
        [StringLength(128)]
        [Required(AllowEmptyStrings = false)]
        public string Username
        {
            get;
            set;
        }

        [StringLength(128)]
        [Required(AllowEmptyStrings = false)]
        public string Password
        {
            get;
            set;
        }

        [Required]
        public bool? RememberMe
        {
            get;
            set;
        }
    }
}