using Newtonsoft.Json;

public class MockAddressValidatorService : IValidatorService
{
    public string Validate(ValidatorRequest request)
    {
        return JsonConvert.SerializeObject(request);
    }
}