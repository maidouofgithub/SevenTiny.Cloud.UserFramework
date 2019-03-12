using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.UserFramework.Api.Models;
using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.UserManagement.ServiceContract;
using SevenTiny.Cloud.UserFramework.UserManagement.ValueObject;

namespace SevenTiny.Cloud.UserFramework.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController(
            IUserRegisterService _userRegisterService
            )
        {
            userRegisterService = _userRegisterService;
        }

        private readonly IUserRegisterService userRegisterService;

        [Route("Account/PreRegister")]
        public IActionResult PreRegister(Account account)
        {
            try
            {
                return userRegisterService.RegisterAction(account).ToJsonResultModel();
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(ex.ToString());
            }
        }

        [Route("Account/Register")]
        public IActionResult Register(UserInfoDTO userInfoDTO)
        {
            try
            {
                return userRegisterService.VerifyRegisterInfoAndAccomplish(userInfoDTO)
                    .ToJsonResultModel("注册成功");
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(ex.ToString());
            }
        }
    }
}