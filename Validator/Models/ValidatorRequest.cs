public class ValidatorRequest
{
    public ValidatorRequest(string itemToValidate, List<KeyValuePair<string, string>> requestContent)
    {
        ItemToValidate = itemToValidate;
        RequestContent = requestContent;
    }

    public List<KeyValuePair<string, string>> RequestContent { get; set; }    

    public string ItemToValidate { get; set; }
}