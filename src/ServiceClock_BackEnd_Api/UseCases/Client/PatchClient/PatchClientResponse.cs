namespace ServiceClock_BackEnd.UseCases.Client.PatchClient;

public class PatchClientResponse : ResponseCore
{
    public PatchClientResponse(Application.Boundaries.Client.PatchClientBoundarie? boundarie)
        : base("Client")
    {
        if(boundarie !=null && boundarie.Client!=null)
        {
            this.Client = new
            {
                Name = boundarie.Client.Name, Address = boundarie.Client.Address, City = boundarie.Client.City, State = boundarie.Client.State,
                Country = boundarie.Client.Country, PostalCode = boundarie.Client.PostalCode, PhoneNumber = boundarie.Client.PhoneNumber,
                Email = boundarie.Client.Email, BirthDate = boundarie.Client.BirthDate, CreatedAt = boundarie.Client.CreatedAt,
                Image = boundarie.Client.ClientImage
            };
            this.Success = true;
        }
    }
    public object? Client { get; set; } 
    public bool Success { get; set; } = false;
}

