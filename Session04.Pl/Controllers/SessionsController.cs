using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Session04.BLL.Services.Interfaces;
using Session04.BLL.ViewModels.SessionViewModels;

namespace Session04.Pl.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        #region Index
        //Get BaseURL/Sessions/Index
        //Index  __List All Sessions
        public async Task<IActionResult> Index(CancellationToken ct)
            => View(await _sessionService.GetAllSessionsAsync(ct));
        #endregion
        #region Create
        //Get BaseURL/Sessions/Create
        //Create  -- Show form to create a new session
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropdownsAsync(ct);
            return View();
        }
        //Post BaseURL/Sessions/Create
        //Create  -- Save new session data

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(ct);
                return View(model);
            }

            var result = await _sessionService.CreateSessionAsync(model, ct);
            if (result.Sucess)
            {
                TempData["SuccessMessage"] = "Session created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Error;
            await PopulateDropdownsAsync(ct);
            return View(model);
        }

        #endregion
        #region Details
        //Get BaseURL/Sessions/Details/{id}
        //Details  -- Show details of a specific session
        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var session = await _sessionService.GetSessionByIdAsync(id, ct);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }
        #endregion

        #region Edit
        //Get BaseURL/Sessions/Edit/{id}
        //Edit  -- Show form to edit an existing session
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var session = await _sessionService.GetSessionToUpdateAsync(id, ct);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session cannot be edited (not found, already started, or has bookings).";
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdownsAsync(ct);
            return View(session);
        }
        //Post BaseURL/Sessions/Edit/{id}
        //Edit  -- Save updated session data

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(ct);
                return View(model);
            }

            var result = await _sessionService.UpdateSessionAsync(id, model, ct);
            if (result.Sucess)
            {
                TempData["SuccessMessage"] = "Session updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Error;
            await PopulateDropdownsAsync(ct);
            return View(model);
        }
        #endregion

        #region Delete
        //Get BaseURL/Sessions/Delete/{id}
        //Delete  -- Show confirmation to delete a session
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var session = await _sessionService.GetSessionByIdAsync(id, ct);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }
        //Post BaseURL/Sessions/Delete/{id}
        //Delete  -- Perform the deletion of the session

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _sessionService.RemoveSessionAsync(id, ct);
            TempData[result.Sucess ? "SuccessMessage" : "ErrorMessage"] =
                result.Sucess ? "Session deleted successfully." : result.Error;
            return RedirectToAction(nameof(Index));
        }
        #endregion
        private async Task PopulateDropdownsAsync(CancellationToken ct)
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(ct), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoriesForDropDownAsync(ct), "Id", "CategoryName");
        }
    }
}
