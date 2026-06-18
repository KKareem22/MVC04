using Microsoft.AspNetCore.Mvc;
using Session04.BLL.Services.Interfaces;
using Session04.BLL.ViewModels.TrainerViewModels;

namespace Session04.Pl.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ITrainerService trainerService;

        public TrainersController(ITrainerService trainerService)
        {
            this.trainerService = trainerService;
        }
        #region Index
        //Get BaseURL/Members/Index
        //Index  -- List all members
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var trainers = await trainerService.GetAllTrainersAsync(ct: ct);
            return View(trainers);
        }

        #endregion
        #region Create
        //1]Get  Base Url/Trainers/Create
        //Create -- Show the form to create a new trainer
        public IActionResult Create() => View();

        //2]Post Base Url/Trainers/CreateTrainer
        //Create -- Handle the form submission to create a new trainer
        [HttpPost]
        public async Task<IActionResult> CreateTrainer(CreateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(nameof(Create), model);
            var Result = await trainerService.CreateTrainerAsync(model, ct: ct);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer created successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create trainer. Please try again.";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Details
        //Get BaseURL/Trainers/Details/{id}
        //Details  -- Show one trainer's details
        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var trainer = await trainerService.GetTrainerDetailsAsync(id, ct);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }
        #endregion
        #region Edit
        //1]Get  Base URL/Trainers/Edit
        //Edit  -- Show form pre-filled with the member's current data for editing
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var TrainerToUpdate = await trainerService.GetTrainerToUpdateAsync(id, ct);
            if (TrainerToUpdate is null)
            {
                TempData["ErrorMessage"] = "Not Found!";
                return View(nameof(Index));
            }
            return View(TrainerToUpdate);
        }



        //2]Post Base URL/Trainers/Edit
        //Edit  -- Handle the form submission to update the trainer's information
        [HttpPost]
        public async Task<IActionResult> Edit(int id, TrainerToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);
            var Result = await trainerService.UpdateTrainerDetailsAsync(id, model, ct: ct);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Failed to update member. Please try again.";
            return View(model);

        }
        #endregion
        #region Delete
        //1]Get  Base URL/Trainers/Delete
        //Delete  -- Show a confirmation page to delete a trainer
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var trainer = await trainerService.GetTrainerDetailsAsync(id, ct);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }


        //2]Post Base URL/Trainers/Delete
        //Delete  -- Handle the form submission to delete a trainer
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await trainerService.RemoveTrainerAsync(id, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer deleted successfully!";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete trainer. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion

    }
}
