using Euro.DAL;
using Euro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Euro.Service
{
    public interface IAccountService
    {
        public User ValidateLogin(LoginModel loginModel);
        public bool SignUp(RegisterModel registerModel);

    }
}
