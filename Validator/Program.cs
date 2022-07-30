using System;
using System.IO;
using System.Collections.Generic;

namespace Validator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var service = new AddressValidatorService();
            var results = service.ValidateFromCsv(args[0]); 
            foreach (var item in results){
                Console.WriteLine(item);
            }
        }
    }
}