# address-validator

## Run the code

1. Create a CSV file containing fields `Street Address`, `City`, and `Postal Code`. </br>
For example:
```
Street Address, City, Postal Code
123 e Maine Street, Columbus, 43215
1 Empora St, Title, 11111
```

    A fourth field `Country Code` can optionally be entered, as well; otherwise, the program will assume the address is in the US. 

    Note that the program expects the header fields to occur in the first line of the file, but the fields can occur in any order.

2. Update the API key on line 5 of `AddressValidatorService.cs`. </br>
A new key can be obtained by signing up for a free trial here: https://www.address-validator.net/free-trial-registration.html

3. In a terminal, navigate to address-validator/Validator. Type `dotnet run [Path to your csv file]` and hit enter.  </br>
For example: `dotnet run ~/Documents/addresses.csv`

## Run the tests

Navigate to `ValidateFromCsvTests.cs`. Right click the `[TestClass]` attribute on line 5; then click `Run tests in context`, or `Debug tests in context` if you want to set breakpoints.

## Design notes

* `AddressValidator` implements interface `IValidator` so that the project could be more easily extended to validate other types of objects.

* `AddressValidator` takes an implementation of `IValidatorService` in its constructor so that the project could be extended to use a different type of address validation. Everything specific to the Address-Validator Online API is in `AddressValidatorService`, which implements `IValidatorService`.

* Some of the tests use `MockAddressValidatorService` to validate the program is correctly parsing the CSV input and formatting requests sent to the Address-Validator Online API without exhausting the 100 checks allowed with the free trial; the mock service simply returns a serialized version of the `ValidatorRequest` it receives, which the test methods then deserialize and verify the format of. 

* When testing locally with CSV files I created, I noticed the file content would sometimes be surrounding with additional quotations, such as: 
```
"\"Street Address, City, Postal Code
6601 Tennyson St NE, Albuquerque, 87111\""
``` 
Thus, the `AddressValidator` strips any quotes and backslashes from content before sending it to the `AddressValidatorService`.

* I tried to name variables and methods well enough to reduce the need for comments.

* There are certainly ways the code could be improved, but this felt good enough for now :)