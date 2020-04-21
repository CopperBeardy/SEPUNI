using AutoMapper;
using CordEstates.Areas.Identity.Data;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Helpers;
using CordEstates.Wrappers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CordEstates.Helpers.ImageUpload;

namespace CordEstates.Areas.Staff.Controllers
{
    [Area("Staff")]
    public class EmployeeController : Controller
    {


        readonly IMapper _mapper;
        readonly IRepositoryWrapper _repositoryWrapper;

        readonly IImageUploadWrapper _imageUploadWrapper;
        readonly ILoggerManager _logger;
        readonly IHostEnvironment _hostEnvironment;

        public EmployeeController(ILoggerManager logger, IRepositoryWrapper repositoryWrapper, IMapper mapper, IHostEnvironment hostEnvironment, IImageUploadWrapper imageUploadWrapper)
        {
            _hostEnvironment = hostEnvironment;
            _logger = logger;
            _imageUploadWrapper = imageUploadWrapper;
            _repositoryWrapper = repositoryWrapper;

            _mapper = mapper;

        }

        // GET: Staff/User
        public async Task<IActionResult> Index()
        {
            List<EmployeeManagementDTO>  users = _mapper.Map<List<EmployeeManagementDTO>>(await _repositoryWrapper.Employee.GetAllUsers());
 
            return View(users);
        }

        // GET: Staff/User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
          
                var userManagementDTO = _mapper.Map<EmployeeManagementDTO>(await _repositoryWrapper.Employee.GetStaffByIdAsync(id));

        

                return View(nameof(Details), userManagementDTO);

          
      
        }


        // GET: Staff/User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var usermangagementDTO = _mapper.Map<EmployeeManagementDTO>(await _repositoryWrapper.Employee.GetStaffByIdAsync(id));

            return View(nameof(Edit), usermangagementDTO);
        }

        // POST: Staff/User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EmployeeManagementDTO userManagementDTO)
        {
            if (id != userManagementDTO.Id)
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (userManagementDTO.File != null)
                    {
                        userManagementDTO.HeadShot = new Photo()
                        { ImageLink = _imageUploadWrapper.Upload(userManagementDTO.File, _hostEnvironment) };
                    }


                    _repositoryWrapper.Employee.UpdateUser(_mapper.Map<ApplicationUser>(userManagementDTO));
                    await _repositoryWrapper.SaveAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred when updating user with id {id}; {ex}");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Edit), userManagementDTO);
        }

        // GET: Staff/User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            EmployeeManagementDTO userManagementDTO = _mapper.Map<EmployeeManagementDTO>(await _repositoryWrapper.Employee.GetStaffByIdAsync(id));

            return View(nameof(Delete), userManagementDTO);
        }

        // POST: Staff/User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            ApplicationUser user = await _repositoryWrapper.Employee.GetStaffByIdAsync(id);
            _repositoryWrapper.Employee.DeleteUser(user);
            await _repositoryWrapper.SaveAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
