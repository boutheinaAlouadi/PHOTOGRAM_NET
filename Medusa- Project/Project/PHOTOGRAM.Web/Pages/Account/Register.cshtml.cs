﻿using PHOTOGRAM.ApplicationCore.Entities;
using PHOTOGRAM.DataAccess.Data;
using PHOTOGRAM.DataAccess.Identity;
using PHOTOGRAM.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace PHOTOGRAM.Web.Pages.Account
{
    [ValidateAntiForgeryToken]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RepositoryContext repositoryContext;

        public RegisterModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RepositoryContext repositoryContext)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.repositoryContext = repositoryContext;
        }

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if ((RegisterViewModel.Email.IndexOf("@") > -1) && (RegisterViewModel.Password.Equals(RegisterViewModel.ConfirmPassword)))
            {
                var user = await this.userManager.FindByEmailAsync(RegisterViewModel.Email);

                if (user == null)
                {
                    Profile profile = new Profile
                    {
                        UserName = RegisterViewModel.UserName,
                        DisplayName = RegisterViewModel.UserName,
                        Bio = RegisterViewModel.UserName + "´s Bio",
                        Image = await System.IO.File.ReadAllBytesAsync(".//wwwroot//Images//User.PNG"),
                        Created = DateTime.Now,
                        Modified = DateTime.Now
                    };

                    await repositoryContext.Profiles.AddAsync(profile);
                    await repositoryContext.SaveChangesAsync();

                    ApplicationUser applicationUser = new ApplicationUser
                    {
                        Email = RegisterViewModel.Email,
                        UserName = RegisterViewModel.UserName,
                        Profile = profile
                    };

                    var result = await this.userManager.CreateAsync(applicationUser, RegisterViewModel.Password);

                    if (result.Succeeded)
                    {
                        await this.signInManager.PasswordSignInAsync(applicationUser, RegisterViewModel.Password, true, true);
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Something dosen´t work");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User already exists");
                }
            }
            else
            {
                ModelState.AddModelError("", "Passwords don´t match");
            }

            return Page();
        }
    }
}