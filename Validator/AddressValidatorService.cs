public class AddressValidatorService : ValidatorService
{
    public AddressValidatorService()
    : base (new AddressValidator())
    {
    }
}