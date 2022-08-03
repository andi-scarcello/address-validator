# address-validator

## Run the code

1. Create a CSV file containing fields `Street Address`, `City`, and `Postal Code`, such as in the examples below. A fourth field `Country Code` can optionally be entered, as well; otherwise, the program will assume the address is in the US. Note the code expects the header fields to occur in the first line of the file, but the fields can occur in any order. </br>
```
Street Address, City, Postal Code
123 e Maine Street, Columbus, 43215
1 Empora St, Title, 11111
```

```
City, Street Address, Postal Code
Columbus, 123 e Maine Street, 43215
Title,1 Empora St,11111
```

2. Update the API key on line 5 of `AddressValidatorService.cs`. </br>
A new key can be obtained by signing up for a free trial here: https://www.address-validator.net/free-trial-registration.html

3. In a terminal, navigate to address-validator/Validator. Type `dotnet run [Path to your csv file]` and hit enter.  </br>
For example: `dotnet run ~/Documents/addresses.csv`

4. The output will include the original address and either the corrected address, "Invalid Address", or another status output by the Address-Validator API. For example:
```
6601 Tennyson St NE, Albuquerque, 87111 -> 6601 Tennyson St NE,Albuquerque NM 87111-8161
123 e Maine Street, Columbus, 43215 -> 123 E Main St, Columbus, 43215-5207
1 Empora St, Title, 11111 -> Invalid Address
``` 

## Run the tests

Navigate to `ValidateFromCsvTests.cs`. Right click the `[TestClass]` attribute on line 5; then click `Run tests in context`, or `Debug tests in context` if you want to set breakpoints.

## Design notes

* `AddressValidator` implements interface `IValidator` so that the project could be more easily extended to validate other types of objects.

* `AddressValidator` takes an implementation of `IValidatorService` in its constructor so that the project could be extended to use a different type of address validation. Everything specific to the Address-Validator Online API is in `AddressValidatorService`, which implements `IValidatorService`.

* I separated the CSV parsing from the general `Validate` logic in `AddressValidator` so that the `Validate` logic could be reused if the program was extended to receive addresses from a different type of input.

* Some of the tests use `MockAddressValidatorService` to validate the program is correctly parsing the CSV input and formatting requests sent to the Address-Validator Online API without exhausting the 100 checks allowed with the free trial; the mock service simply returns a serialized version of the `ValidatorRequest` it receives, which the test methods then deserialize and verify the format of. 

* When testing locally with CSV files I created, I noticed the file content would sometimes be surrounding with additional quotations, such as shown below. Thus, the `AddressValidator` strips any quotes and backslashes from content before sending it to the `AddressValidatorService`.
```
"\"Street Address, City, Postal Code
6601 Tennyson St NE, Albuquerque, 87111\""
``` 

* I tried to name variables and methods well enough to reduce the need for comments.

* There are certainly ways the code could be improved, but this felt good enough for now :)