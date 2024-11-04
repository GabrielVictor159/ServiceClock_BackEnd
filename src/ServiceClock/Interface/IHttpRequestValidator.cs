
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ServiceClock_BackEnd.Interface;

public interface IHttpRequestValidator
{
    Task<(bool, IActionResult?, List<Claim> Claims)> Validate(HttpRequest request);
}

