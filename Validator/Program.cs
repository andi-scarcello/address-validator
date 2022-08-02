namespace Validator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var addressValidator = new AddressValidator(new AddressValidatorService());
            var results = addressValidator.ValidateFromCsv(args[0]); 
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }
    }
}