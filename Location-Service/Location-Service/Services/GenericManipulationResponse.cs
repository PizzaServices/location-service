namespace Location_Service.Services;

public class GenericManipulationResponse<TEntity>
{
    public bool Successful { get; init; }
    public ICollection<string> ErrorMessages { get; init; } = new List<string>();
    public TEntity? Entity { get; init; }
}