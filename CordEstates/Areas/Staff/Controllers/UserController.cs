using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CordEstates.Areas.Identity.Data;
using CordEstates.Models.DTOs;
using AutoMapper;
using CordEstates.Wrappers.Interfaces;
using CordEstates.Helpers;
using CordEstates.Areas.Staff.Models.DTOs;


namespace CordEstates.Areas.Staff.Controllers
{
    [Area("Staff")]
    public class UserController : Controller
    {


        readonly IMapper _mapper;
        readonly IRepositoryWrapper _repositoryWrapper;

        readonly ILoggerManager _logger;


        public UserController(ILoggerManager logger, IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _logger = logger;
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;

        }

        // GET: Staff/User
        public async Task<IActionResult> Index()
        {

            var users = _mapper.Map<List<UserManagementDTO>>(await _repositoryWrapper.User.GetAllStaffAsync());
            return View(users);
        }

        // GET: Staff/User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index)); 
            }
            
            var userManagementDTO = _mapper.Map<UserManagementDTO>( await _repositoryWrapper.User.GetStaffByIdAsync(id));
            

            return View(nameof(Details),userManagementDTO);
        }


        // GET: Staff/User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var usermangagementDTO = _mapper.Map<UserManagementDTO>( await _repositoryWrapper.User.GetStaffByIdAsync(id));
         
            return View(nameof(Edit),usermangagementDTO);
        }

        // POST: Staff/User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,UserManagementDTO userManagementDTO)
        {
            if (id != userManagementDTO.Id)
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //todo upload image of headshot for user and delete any current ones
                    _repositoryWrapper.User.UpdateUser(_mapper.Map<ApplicationUser>(userManagementDTO));
                    await _repositoryWrapper.SaveAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred when updating user with id {id}; {ex}");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Edit),userManagementDTO);
        }

        // GET: Staff/User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            UserManagementDTO userManagementDTO = _mapper.Map<UserManagementDTO>(await _repositoryWrapper.User.GetStaffByIdAsync(id));
            
            return View(nameof(Delete),userManagementDTO);
        }

        // POST: Staff/User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            ApplicationUser user = await _repositoryWrapper.User.GetStaffByIdAsync(id);
            _repositoryWrapper.User.DeleteUser(user);
            await _repositoryWrapper.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

      
    }
}
