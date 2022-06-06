namespace Location_Service.Repositories.Contracts;

public class GenericManipulationResponse<TModel>
{
    public bool Successful { get; set; }
    public IList<string> ErrorMessages { get; set; } = new List<string>();
    public TModel? Model { get; set; }
}