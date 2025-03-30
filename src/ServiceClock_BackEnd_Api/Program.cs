using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Prometheus;
using ServiceClock_BackEnd.Filters;
using ServiceClock_BackEnd.UseCases.Appointment.DateAnalysisJob;
using ServiceClock_BackEnd_Api.Factory;
using ServiceClock_BackEnd_Api.Filters;
using ServiceClock_BackEnd_Api.Helpers;
using ServiceClock_BackEnd_Api.Middlewares;
using ServiceClock_BackEnd_Api.Modules.DependencyInjection;
using ServiceClock_BackEnd_Api.UseCases.Messages.ListMessage;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!);

var tokenValidationParameters = new TokenValidationParameters()
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = false,
    ValidateAudience = false,
};

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterInstance(tokenValidationParameters).AsSelf().SingleInstance();

    containerBuilder.AddAutofacRegistration();
});

builder.Services.AddLogging();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(e =>
{
    e.RequireHttpsMetadata = false;
    e.SaveToken = true;
    e.TokenValidationParameters = tokenValidationParameters;
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy("NoAuthPolicy", policy => policy.RequireAssertion(_ => true));
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<NotificationMiddleware>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    };
    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    };
    c.AddSecurityRequirement(securityRequirement);

    c.DocumentFilter<PrefixDocumentFilter>($"{Environment.GetEnvironmentVariable("PATCH_PREFIX") ?? "" }");

    c.AddSignalRDocumentation();
});



var corsPolicyAllOrigins = "AllowAllOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyAllOrigins,
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddHostedService<DateAnalysisJob>();

var app = builder.Build();

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

app.UseWebSockets(webSocketOptions);

app.Map($"{Environment.GetEnvironmentVariable("PATCH_PREFIX")}/ws", wsApp =>
{
    wsApp.Run(async context =>
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using var scope = app.Services.GetAutofacRoot().BeginLifetimeScope();
            var webSocketFactory = scope.Resolve<WebSocketFactory>();
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            await webSocketFactory.Echo(webSocket);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    });
});

app.UseCors(corsPolicyAllOrigins);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.UseMiddleware<CustomMetricAuthMiddleware>();

app.MapMetrics().RequireAuthorization("NoAuthPolicy");

app.UseMetricServer();
app.UseHttpMetrics();

var cpuMetric = Metrics.CreateGauge("api_cpu_usage", "Uso da CPU em %");
var memoryMetric = Metrics.CreateGauge("api_memory_usage", "Uso da memória em MB");

async Task ColetarMetricas()
{
    while (true)
    {
        var process = Process.GetCurrentProcess();

        cpuMetric.Set(process.TotalProcessorTime.TotalMilliseconds / 1000.0);

        memoryMetric.Set(process.WorkingSet64 / (1024.0 * 1024.0));

        await Task.Delay(5000); 
    }
}

_ = Task.Run(ColetarMetricas);

app.MapGet("/", () => "API rodando com métricas de CPU e Memória!");

app.Run();
