using Euro.DAL;
using Euro.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Euro.Service
{
    public class AccountService : IAccountService
    {
        private EuroContext _dbContext;
        public AccountService(DbContext dbContext)
        {
            _dbContext = dbContext as EuroContext;
        }
        public bool SignUp(RegisterModel registerModel)
        {
            try
            {
                User user = new User()
                {
                    FirstName = registerModel.FirstName,
                    Password = registerModel.Password,
                    Email = registerModel.Email,
                    LastName = registerModel.LastName,
                    UserName = registerModel.UserName
                };

                _dbContext.User.Add(user);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User ValidateLogin(LoginModel loginModel)
        {
            try
            {
                var user = _dbContext.User.Where(e => e.UserName == loginModel.UserName 
                && e.Password == loginModel.Password).FirstOrDefault();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
