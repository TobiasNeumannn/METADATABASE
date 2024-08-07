﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using METADATABASE.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace METADATABASE.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        public IndexModel(
            UserManager<Users> userManager,
            SignInManager<Users> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DisplayName("Profile Picture Name")]
            public string PfpName { get; set; }

            [Required]
            [NotMapped]
            public IFormFile PfpFile { get; set; }

            [Required]
            [StringLength(255)]
            [Display(Name = "Project Name")]
            public string ProjName { get; set; }

            [Required]
            [DisplayName("Project Thumbnail Image Name")]
            public string ThumbName { get; set; }

            [Required]
            [NotMapped]
            public IFormFile ThumbFile { get; set; }

            [Required]
            [StringLength(10000, ErrorMessage = "Do not enter more than ten thousand characters")]
            [Display(Name = "Project Description")]
            public string ProjDesc { get; set; }

        }

        private async Task LoadAsync(Users user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PfpName = user.PfpName,
                PfpFile = user.PfpFile,
                ProjName = user.ProjName,
                ThumbName = user.ThumbName,
                ThumbFile = user.ThumbFile,
                ProjDesc = user.ProjDesc
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (Input.PfpName != user.PfpName)
            {
                user.PfpName = Input.PfpName;
            }
            if (Input.PfpFile != user.PfpFile)
            {
                user.PfpFile = Input.PfpFile;
            }
            if (Input.ProjName != user.ProjName)
            {
                user.ProjName = Input.ProjName;
            }
            if (Input.ThumbName != user.ThumbName)
            {
                user.ThumbName = Input.ThumbName;
            }
            if (Input.ThumbFile != user.ThumbFile)
            {
                user.ThumbFile = Input.ThumbFile;
            }
            if (Input.ProjDesc != user.ProjDesc)
            {
                user.ProjDesc = Input.ProjDesc;
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
