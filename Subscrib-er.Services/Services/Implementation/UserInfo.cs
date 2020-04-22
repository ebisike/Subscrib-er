using Subscrib_er.Data;
using Subscrib_er.Services.Interface;
using Subscrib_er.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscrib_er.Services.Services.Implementation
{
    public class UserInfo : IUserInfo
    {
        private readonly IRepository<ApplicationUser> repository;

        public UserInfo(IRepository<ApplicationUser> repository)
        {
            this.repository = repository;
        }
        ApplicationUser IUserInfo.UserInfo(string id)
        {
            return repository.GetUserById(id);
        }
    }
}
