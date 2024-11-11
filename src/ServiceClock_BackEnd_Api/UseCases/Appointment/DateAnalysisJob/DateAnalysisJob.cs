using Microsoft.Extensions.Hosting;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.UseCases.Appointment.DateAnalysisJob;

public class DateAnalysisJob : IHostedService, IDisposable
{
    private readonly IRepository<Log> logRepository;
    private readonly IRepository<Domain.Models.Appointment> appointmentRepository;
    private Timer timer;

    public DateAnalysisJob(
        IRepository<Log> logRepository,
        IRepository<Domain.Models.Appointment> appointmentRepository)
    {
        this.logRepository = logRepository;
        this.appointmentRepository = appointmentRepository;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        var appointments = this.appointmentRepository.Find(e => e.Status == AppointmentStatus.Canceled || e.Status == AppointmentStatus.Completed);
        if (appointments.Any())
        {
            this.appointmentRepository.DeleteRange(appointments);
            this.logRepository.Add(new Log(LogType.PROCESS, "DateAnalysisJob", $"{appointments.Count()} Appointments Canceled and Completed were deleted"));
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        timer?.Dispose();
    }
}
