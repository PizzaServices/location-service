namespace Location_Service.Repositories.Validations;

public class ValidationResponse
{
    public bool Successful { get; set; }

    public IList<string> ErrorMessages { get; set; } = new List<string>();
}