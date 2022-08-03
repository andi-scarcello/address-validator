using Newtonsoft.Json;

public class AddressValidatorService : IValidatorService
{
    private const string ApiKey = "av-90f9be5fa87e10d15fed11a896cfade5";

    const string ApiUrl = "https://api.address-validator.net/api/verify";

    public string Validate(ValidatorRequest request)
    {
        if (!request.RequestContent.Select(c => c.Key).Contains("CountryCode"))
        {
            // Default country to US; country code must be specified otherwise it returns status NO_COUNTRY
            request.RequestContent.Add(new KeyValuePair<string, string>("CountryCode", "US"));
        }

        request.RequestContent.Add(new KeyValuePair<string, string>("APIKey", ApiKey));

        HttpContent content = new FormUrlEncodedContent(request.RequestContent);
        HttpClient client = new HttpClient();
        HttpResponseMessage response = client.PostAsync(ApiUrl, content).Result;
        string responseContent = response.Content.ReadAsStringAsync().Result;

        ValidatorResult? validatorResult = JsonConvert.DeserializeObject<ValidatorResult>(responseContent);
        if (validatorResult == null){
            return $"{request.ItemToValidate} -> failled to deserialize response into {typeof(ValidatorResult)}";
        }

        if (validatorResult.status.Equals("VALID")) {
            return $"{request.ItemToValidate} -> {validatorResult.formattedaddress}";
        }

        else if (validatorResult.status.Equals("INVALID")){
            return $"{request.ItemToValidate} -> Invalid Address";
        }

        return $"{request.ItemToValidate} -> returned status {validatorResult.status}; diagnostics: {validatorResult.diagnostics}";
    }
}