
namespace ServiceClock_BackEnd.Api.UseCases.Company.PatchCompany;

public class PatchCompanyResponse: ResponseCore
{
    public PatchCompanyResponse(Application.Boundaries.Company.PatchCompanyBoundarie? boundarie)
        : base("Company")
    {
        if(boundarie !=null && boundarie.Company!=null)
        {
            this.Success = true;
        }
    }
    public bool Success { get; set; } = false;
}

