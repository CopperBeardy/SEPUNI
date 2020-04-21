using AutoMapper;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Helpers;
using CordEstates.Wrappers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize(Roles = "Staff")]
    public class AgentDashboardController : Controller
    {
        readonly IMapper _mapper;

        readonly ILoggerManager _logger;
        readonly IRepositoryWrapper _repositoryWrapper;

        public AgentDashboardController(ILoggerManager logger, IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _mapper = mapper;

        }


        // GET: Employees/Appointment
        public async Task<IActionResult> Index()
        {
            string id = await _repositoryWrapper.Employee.GetUserId(User);
            List<AppointmentDTO> appointments =
                _mapper.Map<List<AppointmentDTO>>(await _repositoryWrapper.Appointment.GetAllAppointmentsByStaffIdAsync(id));
            appointments = appointments.OrderBy(o => o.Time).ToList();

            return View(nameof(Index), appointments);
        }



    }
}