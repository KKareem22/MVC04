using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Session04.BLL.Services.Interfaces;
using Session04.BLL.ViewModels.MemberShipViewModels;

namespace Session04.Pl.Controllers
{
    public class MemberShipsController : Controller
    {
        private readonly IMemberShipsService memberService;

        public MemberShipsController(IMemberShipsService memberService)
        {
            this.memberService = memberService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var memberships = await memberService.GetAllMemberShipsAsync(ct: ct);
            return View(memberships);
        }
        #region Create
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropdownsAsync(ct);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberShipViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(ct);
                return View(model);
            }

            var result = await memberService.CreateMembershipAsync(model, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "MemberShip created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Failed";
            await PopulateDropdownsAsync(ct);
            return View(model);
        }
        #endregion

        private async Task PopulateDropdownsAsync(CancellationToken ct)
        {
            ViewBag.Plans = new SelectList(await memberService.GetPlansForDropDownAsync(ct), "Id", "Name");
            ViewBag.Members = new SelectList(await memberService.GetMembersForDropDownAsync(ct), "Id", "Name");
        }
    }
}
