using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.UserFramework.Api.Models;
using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;
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
        public IActionResult PreRegister(UserInfoDTO userInfoDTO)
        {
            try
            {
                return Result.Success()
                    .ContinueAssert(userInfoDTO != null, "注册信息不能为空")
                    .Continue(userRegisterService.RegisterAction(userInfoDTO))
                    .ToJsonResultModel();
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(ex.ToString());
            }
        }

        [Route("Account/PhoneRegister")]
        public IActionResult PhoneRegister(string phone, string verificationCode)
        {
            try
            {
                return Result.Success()
                    .ContinueAssert(!string.IsNullOrEmpty(phone), "手机号不能为空")
                    .ContinueAssert(!string.IsNullOrEmpty(verificationCode), "验证码不能为空")
                    .Continue(userRegisterService.VerifyPhoneAndAccomplish(phone, verificationCode))
                    .ToJsonResultModel("注册成功");
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(ex.ToString());
            }
        }

        [Route("Account/EmailCodeRegister")]
        public IActionResult EmailCodeRegister(string email, string verificationCode)
        {
            try
            {
                return Result.Success()
                    .ContinueAssert(!string.IsNullOrEmpty(email), "邮箱不能为空")
                    .ContinueAssert(!string.IsNullOrEmpty(verificationCode), "验证码不能为空")
                    .Continue(userRegisterService.VerifyEmailCodeAndAccomplish(email, verificationCode))
                    .ToJsonResultModel("注册成功");
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(ex.ToString());
            }
        }

        [Route("Account/EmailLinkRegister")]
        public IActionResult EmailLinkRegister(string verificationCode)
        {
            try
            {
                return Result.Success()
                    .ContinueAssert(!string.IsNullOrEmpty(verificationCode), "验证码不能为空")
                    .Continue(userRegisterService.VerifyEmailLinkCodeAndAccomplish(verificationCode))
                    .ToJsonResultModel("注册成功");
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(ex.ToString());
            }
        }

        [Route("Account/Login")]
        public IActionResult Login(Account account)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(ex.ToString());
            }
        }

        [Route("Account/SignOut")]
        public IActionResult SignOut(Account account)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(ex.ToString());
            }
        }
    }
}