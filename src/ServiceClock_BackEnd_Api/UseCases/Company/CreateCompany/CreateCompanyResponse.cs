
using ServiceClock_BackEnd.Application.Boundaries.Company;

namespace ServiceClock_BackEnd.UseCases.Company.CreateCompany;

public class CreateCompanyResponse
{
    public CreateCompanyResponse(CreateCompanyBoundarie boundarie)
    {
        if(boundarie.Company!=null)
        {
            this.Id = boundarie.Company.Id;
        }
    }
    public Guid Id { get; set; }
}

