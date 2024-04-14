using Gym_Project.Models.WorkoutsModels;
using GymProject.Common.Constants;
using GymProject.Core.DTOs.WorkoutDTOs;
using GymProject.Core.Services;
using GymProject.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Gym_Project.Areas.Identity.Pages.Account.Manage
{
    public class MyWorkouts: PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly WorkoutService _workoutService;

        public MyWorkouts(UserManager<ApplicationUser> userManager, WorkoutService workoutService)
        {
            _userManager = userManager;
            _workoutService = workoutService;
        }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public List<AllWorkoutsViewModel> Workouts { get; set; } = new List<AllWorkoutsViewModel>();
        public async Task<IActionResult> OnGetAsync(string searchString,
            string category,
            string difficultyLevelGroup,
            int pageNumber = PaginationConstants.PageNumber,
            int pageSize = PaginationConstants.PageSize)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }



            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workoutsDTOs = await _workoutService.GetAllNotDeletedWorkoutsForUser(userId);
            workoutsDTOs = _workoutService.ApplySearchFilter(workoutsDTOs, searchString);
            workoutsDTOs = _workoutService.ApplyCategoryFilter(workoutsDTOs, category);
            workoutsDTOs = _workoutService.ApplyDifficultyLevelFilter(workoutsDTOs, difficultyLevelGroup);

            var categoryNames = await _workoutService.GetCategoriesNames();
            ViewData["Categories"] = categoryNames;

            var totalRecords = workoutsDTOs.Count;
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            PageNumber = pageNumber;

              Workouts = workoutsDTOs
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new AllWorkoutsViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    DifficultyLevel = e.DifficultyLevel,
                    ImageUrl = e.ImageUrl,
                    Category = e.Category.Name,
                    Duration = e.Duration,
                    CreatorId = e.CreatorId,
                    CreatorName = e.Creator.UserName,
                    IsJoined = e.UserWorkouts.Any(uw => uw.UserId == userId &&
                                                       uw.UserId != e.CreatorId &&
                                                       !uw.IsDeleted)
                }).ToList();

            ViewData["SearchString"] = searchString ?? string.Empty;
            return Page();
        }
    }
}
