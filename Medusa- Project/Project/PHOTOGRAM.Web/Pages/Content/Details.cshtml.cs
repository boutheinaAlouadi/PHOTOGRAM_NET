﻿using PHOTOGRAM.ApplicationCore.Entities;
using PHOTOGRAM.DataAccess.Data;
using PHOTOGRAM.DataAccess.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PHOTOGRAM.Web.Pages.Content
{
    public class DetailsModel : PageModel
    {
        private readonly RepositoryContext repositoryContext;
        private readonly UserManager<ApplicationUser> userManager;

        public DetailsModel(RepositoryContext repositoryContext, UserManager<ApplicationUser> userManager)
        {
            this.repositoryContext = repositoryContext;
            this.userManager = userManager;
        }

        [BindProperty]
        public Post Post { get; set; }

        [BindProperty]
        public Replay Replay { get; set; }

        public async Task<IActionResult> OnGet(string id)
        {
            this.Post = await this.repositoryContext.Posts.FindAsync(Guid.Parse(id));
            return Page();
        }

        public async Task<IActionResult> OnPostReplay(string id)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await this.userManager.FindByNameAsync(User.Identity.Name);

                this.Replay.Id = Guid.NewGuid();
                this.Replay.Profile = currentUser.Profile;
                this.Replay.Post = this.repositoryContext.Posts.FirstOrDefault(x => x.Id.ToString() == id);
                this.Replay.Created = DateTime.Now;
                this.Replay.Modified = DateTime.Now;

                await this.repositoryContext.Replays.AddAsync(Replay);
                await this.repositoryContext.SaveChangesAsync();
            }

            return RedirectToPage("Details", new { Id = id });
        }
    }
}