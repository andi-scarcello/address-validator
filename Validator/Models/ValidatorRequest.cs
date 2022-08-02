public class ValidatorRequest
{
    public ValidatorRequest(string item, List<KeyValuePair<string, string>> content)
    {
        ItemToValidate = item;
        RequestContent = content;
    }

    public List<KeyValuePair<string, string>> RequestContent { get; set; }    

    public string ItemToValidate { get; set; }
}