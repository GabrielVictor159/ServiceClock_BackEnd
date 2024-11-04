
using FluentValidation.Results;
using FluentValidator;

namespace ServiceClock_BackEnd.Application.Interfaces.Services;

public interface INotificationService
{
    List<Notification> Notifications { get; set; }
    bool HasNotifications { get; }
    void AddNotification(string key, string message);
    void AddNotifications(ValidationResult? validationResult);
}

