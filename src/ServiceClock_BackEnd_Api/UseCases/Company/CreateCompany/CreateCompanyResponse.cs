
using ServiceClock_BackEnd.Application.Boundaries.Company;

namespace ServiceClock_BackEnd.UseCases.Company.CreateCompany;

public class CreateCompanyResponse : ResponseCore
{
    public CreateCompanyResponse(CreateCompanyBoundarie boundarie)
        : base ("Company")
    {
        if(boundarie.Company!=null)
        {
            this.Id = boundarie.Company.Id;
        }
    }
    public Guid Id { get; set; }
}

