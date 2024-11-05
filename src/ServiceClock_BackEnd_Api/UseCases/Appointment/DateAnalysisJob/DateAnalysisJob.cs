
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.UseCases.Appointment.DateAnalysisJob
{
    public class DateAnalysisJob
    {
        private readonly IRepository<Log> logRepository;
        private readonly IRepository<Domain.Models.Appointment> appointmentRepository;

        public DateAnalysisJob
            (IRepository<Log> logRepository, 
            IRepository<Domain.Models.Appointment> appointmentRepository)
        {
            this.logRepository = logRepository;
            this.appointmentRepository = appointmentRepository;
        }
        public void Run()
        {
            var appointments = this.appointmentRepository.Find(e => e.Status == AppointmentStatus.Canceled || e.Status == AppointmentStatus.Completed);
            this.appointmentRepository.DeleteRange(appointments);
            this.logRepository.Add(new Log(LogType.PROCESS, "DateAnalysisJob", $"{appointments.Count()} Appointments Canceled and Completed were deleted"));
        }
    }
}
