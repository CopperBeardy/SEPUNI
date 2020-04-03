using CordEstates.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CordEstates.Repositories.Interfaces
{
    public interface IAppointmentRepository : IRepositoryBase<Appointment>
    {
        void CreateAppointment(Appointment appointment);
        void UpdateAppointment(Appointment appointment);
        Task<List<Appointment>> GetAllAppointmentsAsync();
        Task<List<Appointment>> GetAllAppointmentsByStaffIdAsync(string Id);
        Task<Appointment> GetAppointmentByIdAsync(int? id);
        void DeleteAppointment(Appointment appointment);
        bool Exists(int id);
    }
}
