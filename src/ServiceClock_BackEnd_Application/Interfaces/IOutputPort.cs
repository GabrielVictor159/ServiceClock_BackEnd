using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd_Application.Interfaces;
public interface IOutputPort<Boundarie>
{
    public IActionResult ViewModel { get; set; }
    public void Error(string message);

    public void NotFound(string message);

    public void Standard(Boundarie request);
}
