
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServiceClock_BackEnd.Filters;
using ServiceClock_BackEnd.Mapper;

using ServiceClock_BackEnd.Domain.Modules;
using System.Configuration;
using System.Text;
using ServiceClock_BackEnd.Validator.Http;
using ServiceClock_BackEnd.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd_Application.Interfaces;
using ServiceClock_BackEnd.Application.Boundaries.Company;
using ServiceClock_BackEnd.UseCases.Company.PatchCompany;
using ServiceClock_BackEnd.UseCases.Client.CreateClient;
using ServiceClock_BackEnd.Application.Boundaries.Client;
using ServiceClock_BackEnd.UseCases.Client.PatchClient;
using ServiceClock_BackEnd.UseCases.Services.CreateService;
using ServiceClock_BackEnd.Application.Boundaries.Services;
using ServiceClock_BackEnd.UseCases.Services.DeleteService;
using ServiceClock_BackEnd.UseCases.Appointment.RequestAppointment;
using ServiceClock_BackEnd.Application.Boundaries.Appointment;
using ServiceClock_BackEnd.UseCases.Appointment.AlterStateAppointment;
using ServiceClock_BackEnd.UseCases.Messages.CreateMessage;
using ServiceClock_BackEnd.Application.Boundaries.Messages;
using ServiceClock_BackEnd.UseCases.Services.EditService;
using ServiceClock_BackEnd.UseCases.Client.DeleteClient;

namespace ServiceClock_BackEnd.Modules
{
    public class ApiModule : Module
    {
        public override void Configure(IServiceCollection services)
        {
            services.AddSingleton(new HttpRequestValidator());
            services.AddScoped<NotificationMiddleware>();
            services.AddAutoMapper(typeof(MapperProfile).Assembly);
            services.AddSingleton<IOutputPort<CreateCompanyBoundarie>,CreateCompanyPresenter>();
            services.AddSingleton<IOutputPort<PatchCompanyBoundarie>, PatchCompanyPresenter>();
            services.AddSingleton<IOutputPort<CreateClientBoundarie>, CreateClientPresenter>();
            services.AddSingleton<IOutputPort<PatchClientBoundarie>, PatchClientPresenter>();
            services.AddSingleton<IOutputPort<CreateServiceBoundarie>, CreateServicePresenter>();
            services.AddSingleton<IOutputPort<DeleteServiceBoundarie>, DeleteServicePresenter>();
            services.AddSingleton<IOutputPort<RequestAppointmentBoundarie>, RequestAppointmentPresenter>();
            services.AddSingleton<IOutputPort<AlterStateAppointmentBoundarie>, AlterStateAppointmentPresenter>();
            services.AddSingleton<IOutputPort<CreateMessageBoundarie>, CreateMessagePresenter>();
            services.AddSingleton<IOutputPort<EditServiceBoundarie>, EditServicePresenter>();
            services.AddSingleton<IOutputPort<DeleteClientBoundarie>, DeleteClientPresenter>();

        }
    }
}
