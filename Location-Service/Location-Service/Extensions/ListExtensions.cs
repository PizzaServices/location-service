using Location_Service.Repositories.Validations;

namespace Location_Service.Extensions;

public static class ListExtensions
{
    public static void AddRuleSetValidationResponseIfNotSuccessful(this IList<string> list, RuleSetValidationResponse response)
    {
        if(!response.Successful)
            list.Add(response.ErrorMessage!);
    }
}