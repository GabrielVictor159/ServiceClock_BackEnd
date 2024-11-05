using ServiceClock_BackEnd.Application.Boundaries.Client;
using ServiceClock_BackEnd.Application.Boundaries.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.UseCases.Client.CreateClient;
public class CreateClientResponse : ResponseCore
{
    public CreateClientResponse(CreateClientBoundarie boundarie)
        : base ("Client")
    {
        if (boundarie.Client != null)
        {
            this.Id = boundarie.Client.Id;
        }
    }
    public Guid Id { get; set; }
}
