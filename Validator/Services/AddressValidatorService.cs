using Newtonsoft.Json;

public class AddressValidatorService : IValidatorService
{
    private const string ApiKey = "av-90f9be5fa87e10d15fed11a896cfade5";

    const string ApiUrl = "https://api.address-validator.net/api/verify";

    public string Validate(ValidatorRequest request)
    {
        // Modify request with APIKey and CountryCode (if needed)
        request.RequestContent.Add(new KeyValuePair<string, string>("APIKey", ApiKey));
        if (!request.RequestContent.Select(c => c.Key).Contains("CountryCode"))
        {
            // Default country to US; CountryCode must be specified otherwise the API returns status NO_COUNTRY
            request.RequestContent.Add(new KeyValuePair<string, string>("CountryCode", "US"));
        }

        // Execute the request
        HttpContent content = new FormUrlEncodedContent(request.RequestContent);
        HttpClient client = new HttpClient();
        HttpResponseMessage response = client.PostAsync(ApiUrl, content).Result;

        string responseContent = response.Content.ReadAsStringAsync().Result;
        ValidatorResult? validatorResult = JsonConvert.DeserializeObject<ValidatorResult>(responseContent);

        if (validatorResult == null){
            return $"{request.ItemToValidate} -> failed to deserialize response into {typeof(ValidatorResult)}";
        }

        if (validatorResult.status.Equals("VALID")) {
            return $"{request.ItemToValidate} -> {String.Join(", ", validatorResult.formattedaddress.Split(","))}";
        }

        else if (validatorResult.status.Equals("INVALID")){
            return $"{request.ItemToValidate} -> Invalid Address";
        }

        return $"{request.ItemToValidate} -> returned status {validatorResult.status}; diagnostics: {validatorResult.diagnostics}";
    }
}