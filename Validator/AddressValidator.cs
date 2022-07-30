using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class AddressValidator : IValidator
{
    const string ApiKey = "av-90f9be5fa87e10d15fed11a896cfade5";
    const string ApiUrl = "https://api.address-validator.net/api/verify";
    private string[] AddressElementNames = new string[] { "StreetAddress", "City", "PostalCode", "State" };

    public string Validate(string itemToValidate)
    {
        var addressElements = itemToValidate.Split(',');

        var postData = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("CountryCode", addressElements.Length > 4 ? addressElements[4] : "US"),  // Default country to US
            new KeyValuePair<string, string>("APIKey", ApiKey)
        };
        int addressIndex = 0;
        while (addressElements.Length > addressIndex && addressIndex < 5){
            postData.Add(new KeyValuePair<string, string>(AddressElementNames[addressIndex], addressElements[addressIndex]));
            addressIndex++;
        }

        HttpContent content = new FormUrlEncodedContent(postData);
        HttpClient client = new HttpClient();
        HttpResponseMessage result = client.PostAsync(ApiUrl, content).Result;
        string resultContent = result.Content.ReadAsStringAsync().Result;

        ValidatorResult res = JsonConvert.DeserializeObject<ValidatorResult>(resultContent);

        if (res.status.Equals("VALID")) {
            return $"{itemToValidate} -> {res.formattedaddress}";
        }

        else if (res.status.Equals("INVALID")){
            return $"{itemToValidate} -> Invalid Address";
        }

        return $"{itemToValidate} -> returned status {res.status}; diagnostics: {res.diagnostics}";
    }
}