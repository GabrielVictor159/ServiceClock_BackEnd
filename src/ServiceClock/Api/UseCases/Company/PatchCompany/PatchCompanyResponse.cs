
namespace ServiceClock_BackEnd.Api.UseCases.Company.PatchCompany;

public class PatchCompanyResponse
{
    public PatchCompanyResponse(Application.Boundaries.Company.PatchCompanyBoundarie? boundarie)
    {
        if(boundarie !=null && boundarie.Company!=null)
        {
            this.Success = true;
        }
    }
    public bool Success { get; set; } = false;
}

