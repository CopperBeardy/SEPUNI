using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using CordEstates.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Repositories
{
    public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext context) : base(context)
        {

        }

        // GET: Employees/Appointment
        public async Task<List<Appointment>> GetAllAppointmentsAsync()
         => await FindAll()
            .Include(a => a.Listing)
            .ThenInclude(a => a.Address)
            .Include(a => a.Staff).ToListAsync();

        public async Task<List<Appointment>> GetAllAppointmentsByStaffIdAsync(string Id)

          => await FindAll()
                .Where(x => x.StaffId == Id).Where(y => y.Time > DateTime.Now.AddDays(-1))
                .Include(a => a.Listing)
                .ThenInclude(a => a.Address)
                .Include(a => a.Staff)
                .ToListAsync();


        // GET: Employees/Appointment/Details/5
        public async Task<Appointment> GetAppointmentByIdAsync(int? id)

      => await FindByCondition(m => m.Id.Equals(id))
                .Include(a => a.Listing)
                .ThenInclude(a => a.Address)
                .Include(a => a.Staff)
                .FirstOrDefaultAsync();


        public void CreateAppointment(Appointment appointment) => Create(appointment);


        public void UpdateAppointment(Appointment appointment) => Update(appointment);

        public void DeleteAppointment(Appointment appointment) => Delete(appointment);

        public bool Exists(int id) =>
           _context.Appointments.Any(x => x.Id.Equals(id));


    }
}
