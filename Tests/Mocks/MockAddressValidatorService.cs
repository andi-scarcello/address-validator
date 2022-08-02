using Newtonsoft.Json;

public class MockAddressValidatorService : IValidatorService
{
    public string Validate(ValidatorRequest request)
    {
        var validrequestComponents = new List<string> { "StreetAddress", "City", "PostalCode", "CountryCode", "APIKey" };

        if (request.RequestContent.Any(c => !validrequestComponents.Contains(c.Key))
            || request.RequestContent.Any(c => string.IsNullOrEmpty(c.Value)))
        {
            return false.ToString();
        }

        return JsonConvert.SerializeObject(request);
    }
}