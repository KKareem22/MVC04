using Microsoft.AspNetCore.Mvc;
using Session04.BLL.Services.Interfaces;
using Session04.BLL.ViewModels.PlanViewModels;

namespace Session04.Pl.Controllers
{
    public class PlansController : Controller
    {
        private readonly IPlanService planService;

        public PlansController(IPlanService planService)
        {
            this.planService = planService;
        }
        #region Index
        //Get BaseURL/Plans/Index
        //Index  __List All Plans
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await planService.GetAllPlansAsync(ct);
            return View(plans);
        }

        #endregion
        #region Details
        //Get BaseURL/Plans/Details/{id}
        //Details __Show one Plan's Details
        [HttpGet]
        public async Task<IActionResult>Details(int id,CancellationToken ct)
        {
            var plan = await planService.GetPlanByIdAsync(id, ct);
            if(plan==null)
            {
                TempData["Error Message"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        #endregion
        #region Edit
        //Get BaseURL/Plans/Edit/{id}
        //Edit  -- Show form pre-filled with the plan's current data for editing
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var planToUpdate = await planService.GetPlanToUpdateAsync(id, ct);
            if (planToUpdate is null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(planToUpdate);
        }
        //Post BaseURL/Plans/Edit/{Member}
        //Update  -- Save edits to the plan's data

        [HttpPost]
        public async Task<IActionResult> Edit(int id,UpdatePlanViewModel  model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await planService.UpdatePlanAsync(id, model, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan updated successfully.";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update the plan.";
                return View(model);
            }
        }
        #endregion
        [HttpPost]
        public async Task<IActionResult> Activate(int id, CancellationToken ct)
        {
            var result = await planService.ToggleActivationAsync(id, ct);
            if (result)
                TempData["SucessMessage"] = "Plan activation status changed successfully.";
            else
                TempData["ErrorMessage"] = "Failed to change the plan activation status.";
            return RedirectToAction(nameof(Index));
        }


    }
}
