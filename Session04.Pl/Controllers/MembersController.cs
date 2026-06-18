using Microsoft.AspNetCore.Mvc;
using Session04.BLL.Services.Classes;
using Session04.BLL.Services.Interfaces;
using Session04.BLL.ViewModels.MemberViewModels;

namespace Session04.Pl.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService memberService;

        public MembersController(IMemberService memberService)
        {
            this.memberService = memberService;
        }
        #region Index
        //Get BaseURL/Members/Index
        //Index  -- List all members

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await memberService.GetAllMembersAsync(ct: ct);
            return View(members);
        }

        #endregion
        #region GetHealthRecord
        //Get BaseURL/Members/HealthRecordsDetails/{id}
        //HealthRecordsDetails  -- Show one member's details
        [HttpGet]
        public async Task<IActionResult> MemberHealthRecord(int id, CancellationToken ct)
        {
            var healthRecord = await memberService.GetHealthRecordAsync(id, ct);
            if (healthRecord == null)
            {
                TempData["ErrorMessage"] = "Health record not found for the specified member.";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);
        }
        #endregion
        #region MemberDetails
        //Get BaseURL/Members/MemberDetails/{id}
        //MemberDetails  -- Show one member's details

        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var member = await memberService.GetMemberDetails(id, ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        #endregion
        #region Create

        //Get BaseURL/Members/Create
        //Create  -- Show the form to create a new member
        public IActionResult Create() => View();
        //Post BaseURL/Members/Create/{Member}
        //Create  -- Handle the form submission to create a new member
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);


            var result = await memberService.CreateMemberAsync(model, ct);
            if (result)
                TempData["SuccessMessage"] = "Member created successfully!";
            else
                TempData["ErrorMessage"] = "Failed to create member. Please try again.";

            return RedirectToAction(nameof(Index));

        }
        #endregion
        #region Edit

        //Get BaseURL/Members/Edit/{id}
        //Edit  -- Show form pre-filled with the member's current data for editing
        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            var memberToUpdate = await memberService.GetMemberToUpdateViewModel(id, ct);
            if (memberToUpdate is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            else
                return View(memberToUpdate);
        }
        //Post BaseURL/Members/Edit/{Member}
        //Update  -- Save edits to the member's data
        [HttpPost]
        public async Task<IActionResult> EditMember(int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await memberService.UpdateMemberAsync(id, model, ct: ct);
            if (result == true)
            {
                TempData["SuccessMessage"] = "Member updated successfully!";
                return RedirectToAction(nameof(Index));

            }
            TempData["ErrorMessage"] = "Failed to update member. Please try again.";
            return View(model);



        }
        #endregion
        #region Delete

        //Get BaseURL/Members/Delete/{id}
        //Delete  --  Show Form 
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var member = await memberService.GetMemberDetails(id, ct);
            if (member is null)
            {
                TempData["Error Message"] = "Member not found !";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        //Post BaseURL/Members/DeleteConfirmed/{Member}
        //DeleteConfirmed  --  Submit the deletion of the member
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var Result = await memberService.RemoveMemberAsync(id, ct);
            if (Result)
                TempData["SuccessMessage"] = "Member deleted successfully!";
            else
                TempData["ErrorMessage"] = "Failed to delete member. Please try again.";
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}
