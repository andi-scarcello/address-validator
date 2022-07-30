public class ValidatorService
{
    private readonly IValidator _validator;

    public ValidatorService(IValidator validator){
        _validator = validator;
    }

    public List<string> ValidateFromCsv(string filePath)
    {
        List<string> results = new List<string>();
        using(var reader = new StreamReader(filePath))
        {
            // ignore header
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var item = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(item)){
                    continue;
                }
                var result = _validator.Validate(item);
                results.Add(result);
            }
        }

        return results;
    }
}