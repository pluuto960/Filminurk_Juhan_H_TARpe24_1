using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Filminurk.Core.ServiceInterface;
using Microsoft.AspNetCore.Identity;

namespace Filminurk.ApplicationServices.Services
{
    internal class AccountsServices : IAccountsServices
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailsServices _emailsServices;

        public AccountsServices(UserManager<ApplicationUser> userManager, 
                                SignInManager<ApplicationUser> signInManager, 
                                IEmailsServices emailsServices)
        {
            _userManager= userManager;
            _signInManager= signInManager;
            _emailsServices= emailsServices;
        }
        public async Task<ApplicationUser>Register(ApplicationUserDTO userDTO)
        {
            var user = new ApplicationUser
            {
                Username = userDTO.Email,
                Email = userDTO.Email,
                ProfileType = userDTO.ProfileType,
                DisplayName = userDTO.DisplayName,
            };
            var result = await _userManager.CreateAsync(user, userDTO.Password);
            if (result.Succeeded) 
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //HOMEWORK LOCATION
            }
            return user;
        }
        //Homework location
        public async Task<ApplicationUser>Login(LoginDTO userDTO)
        {
            var user = await _userManager.FindByEmailAsync(userDTO.Email);
            return user;
        }
    }
}
