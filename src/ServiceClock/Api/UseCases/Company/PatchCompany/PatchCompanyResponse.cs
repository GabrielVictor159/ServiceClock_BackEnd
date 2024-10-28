
namespace ServiceClock_BackEnd.Api.UseCases.Company.PatchCompany;

public class PatchCompanyResponse: ResponseCore
{
    public PatchCompanyResponse(Application.Boundaries.Company.PatchCompanyBoundarie? boundarie)
        : base("Company")
    {
        if(boundarie !=null && boundarie.Company!=null)
        {
            this.Company = new
            {
                Id = boundarie.Company.Id, Password = boundarie.Company.Password, Name = boundarie.Company.Name, RegistrationNumber = boundarie.Company.RegistrationNumber, 
                Address = boundarie.Company.Address, City = boundarie.Company.City, State = boundarie.Company.State,
                Country = boundarie.Company.Country, PostalCode = boundarie.Company.PostalCode, PhoneNumber = boundarie.Company.PhoneNumber, Email = boundarie.Company.Email, 
                Image = boundarie.Company.CompanyImage
            };
            this.Success = true;
        }
    }
    public object? Company { get; set; }
    public bool Success { get; set; } = false;
}

