﻿using PHOTOGRAM.ApplicationCore.Entities;
using PHOTOGRAM.DataAccess.Data;
using PHOTOGRAM.DataAccess.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PHOTOGRAM.Web.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly RepositoryContext repositoryContext;
        private readonly UserManager<ApplicationUser> userManager;

        public ProfileModel(RepositoryContext repositoryContext, UserManager<ApplicationUser> userManager)
        {
            this.repositoryContext = repositoryContext;
            this.userManager = userManager;
        }

        [BindProperty]
        public Profile Profile { get; set; }

        public async Task<IActionResult> OnGet(string id)
        {
            this.Profile = await this.repositoryContext.Profiles.FindAsync(Guid.Parse(id));
            return Page();
        }

        public async Task<IActionResult> OnGetFollow(string id)
        {
            var currentUser = await this.userManager.FindByNameAsync(User.Identity.Name);

            var result = currentUser.Profile.Following
                .FirstOrDefault(x => x.FollowingId == currentUser.ProfileId && x.FollowerId == Guid.Parse(id));

            if (result == null)
            {
                await this.repositoryContext.Follows.AddAsync(new Follow
                {
                    Id = Guid.NewGuid(),
                    FollowerId = Guid.Parse(id),
                    FollowingId = currentUser.ProfileId,
                });
            }
            else
            {
                this.repositoryContext.Follows.Remove(result);
            }

            await this.repositoryContext.SaveChangesAsync();

            return RedirectToPage("/Account/Profile", new { Id = id });
        }
    }
}