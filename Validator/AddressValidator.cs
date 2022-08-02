using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class AddressValidator : IValidator
{
    private const string ApiKey = "av-90f9be5fa87e10d15fed11a896cfade5";

    private readonly IValidatorService _validatorService;

    private string[] _addressComponentNames = new string[0];

    public AddressValidator(IValidatorService validatorService){
        _validatorService = validatorService;
    }

    public List<string> ValidateFromCsv(string filePath)
    {
        var addresses = new List<string>();
        using(var reader = new StreamReader(filePath))
        {
            var header = reader.ReadLine();
            _addressComponentNames = header.Split(',');
            for (int i = 0; i < _addressComponentNames.Length; i++){
                _addressComponentNames[i] = _addressComponentNames[i].Replace("\"", String.Empty).Replace(" ", String.Empty);
            }

            while (!reader.EndOfStream)
            {
                var item = reader.ReadLine();
                Console.WriteLine(item);
                if (string.IsNullOrWhiteSpace(item)){
                    continue;
                }
                addresses.Add(item);
            }
        }

        var results = new List<string>();
        addresses.ForEach(address => results.Add(Validate(address)));
        return results;
    }

    private string Validate(string itemToValidate)
    {
        var addressComponents = itemToValidate.Split(',');
        for (int i = 0; i < addressComponents.Length; i++){
            addressComponents[i] = addressComponents[i].Replace("\"", String.Empty).TrimStart();
        }

        var postData = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("CountryCode", addressComponents.Length > 4 ? addressComponents[4] : "US"),  // Default country to US
            new KeyValuePair<string, string>("APIKey", ApiKey)
        };

        int addressIndex = 0;
        while (addressComponents.Length > addressIndex && addressIndex < 5){
            postData.Add(new KeyValuePair<string, string>(_addressComponentNames[addressIndex], addressComponents[addressIndex]));
            addressIndex++;
        }

        return _validatorService.Validate(new ValidatorRequest(itemToValidate, postData)); 
    }
}