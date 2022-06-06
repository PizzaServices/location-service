namespace Location_Service.Models;

public class GenericResponseModel<TModel>
{
    public bool Successful { get; set; }
    public ICollection<string>? ErrorMessages { get; set; }
    public TModel? Model { get; set; }
}