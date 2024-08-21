
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceClock_BackEnd.Api.Interface;

namespace ServiceClock_BackEnd.Api.Validator.Http;

public class BodyValidator<T> : IHttpRequestValidator
{
    public async Task<(bool, IActionResult?, List<Claim> Claims)> Validate(HttpRequest request)
    {
        try
        {
            var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            var serializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Error
            };

            var requestBodyObject = JsonConvert.DeserializeObject<T>(requestBody, serializerSettings);

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(requestBodyObject);
            var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(requestBodyObject, validationContext, validationResults);

            if (!isValid)
            {
                return (false, new BadRequestObjectResult(validationResults), new());
            }
            else
            {
                return (true, null, new());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return (false, new BadRequestObjectResult(e.Message), new());
        }
    }



}

