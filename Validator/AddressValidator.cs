public class AddressValidator : IValidator
{
    private readonly IValidatorService _validatorService;

    private string[] _addressComponentNames = new string[0];

    public AddressValidator(IValidatorService validatorService){
        _validatorService = validatorService;
    }

    public List<string> ValidateFromCsv(string filePath)
    {
        var addresses = new List<string>();
        using (var reader = new StreamReader(filePath))
        {
            var header = reader.ReadLine();
            if (header == null)
            {
                throw new Exception("file is empty");
            }

            _addressComponentNames = header.Split(',');

            while (!reader.EndOfStream)
            {
                var item = reader.ReadLine();
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
        var requestContent = FormatRequestContent(itemToValidate);
        return _validatorService.Validate(new ValidatorRequest(itemToValidate, requestContent)); 
    }

    private List<KeyValuePair<string, string>> FormatRequestContent(string itemToValidate)
    {
        var addressComponents = itemToValidate.Split(',');
        for (int i = 0; i < addressComponents.Length; i++)
        {
            // Depending on how the item was created, it may contain escaped quotes around values; remove these to avoid any issues down stream
            addressComponents[i] = addressComponents[i].Replace("\"", String.Empty).TrimStart();
            _addressComponentNames[i] = _addressComponentNames[i].Replace("\"", String.Empty).Replace(" ", String.Empty);

        }

        var requestContent = new List<KeyValuePair<string, string>>();
        var addressIndex = 0;
        while (addressComponents.Length > addressIndex)
        {
            requestContent.Add(new KeyValuePair<string, string>(_addressComponentNames[addressIndex], addressComponents[addressIndex]));
            addressIndex++;
        }

        return requestContent;
    } 
}