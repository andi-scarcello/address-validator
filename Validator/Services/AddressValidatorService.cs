using Newtonsoft.Json;

public class AddressValidatorService : IValidatorService
{
    const string ApiUrl = "https://api.address-validator.net/api/verify";

    public string Validate(ValidatorRequest request)
    {
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