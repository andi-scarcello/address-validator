using Newtonsoft.Json;

namespace AddressValidatorTests;

[TestClass]
public class ValidateFromCsvTests
{
    private AddressValidator _mockAddressValidator;
    private AddressValidator _addressValidator; 

    [TestInitialize]
    public void Startup()
    {
        _mockAddressValidator = new AddressValidator(new MockAddressValidatorService());
        _addressValidator = new AddressValidator(new AddressValidatorService());
    }

    [DataTestMethod]
    [DataRow(new string[] { "Street Address, City, Postal Code", "6601 Tennyson St NE, Albuquerque, 87111" })]
    [DataRow(new string[] { "City, Postal Code, Street Address", "Albuquerque, 87111, 6601 Tennyson St NE" })]
    [DataRow(new string[] { "Postal Code, Street Address, City", "87111, 6601 Tennyson St NE, Albuquerque" })]
    public void ValidatorRequest_IsFormattedCorrectly_RegardlessOfAddressComponentOrder(string[] fileContent)
    {
        // Set up
        var file = CreateFile(fileContent); 

        // Execute
        var results = _mockAddressValidator.ValidateFromCsv(file);

        // Validate results
        List<ValidatorRequest> requests = results.Select(result => JsonConvert.DeserializeObject<ValidatorRequest>(result)).ToList();
        foreach (var request in requests){
            Assert.AreEqual("6601 Tennyson St NE", request.RequestContent.Single(c => c.Key == "StreetAddress").Value);
            Assert.AreEqual("Albuquerque", request.RequestContent.Single(c => c.Key == "City").Value);
            Assert.AreEqual("87111", request.RequestContent.Single(c => c.Key == "PostalCode").Value);
        }

        // Clean up
        File.Delete(file);
    }

    [DataTestMethod]
    [DataRow(new string[] { "Street Address, City, Postal Code", "6601 Tennyson St NE, Albuquerque, 87111", "6601 Tennyson St NE, Albuquerque, 87111", "6601 Tennyson St NE, Albuquerque, 87111" })]
    public void ValidatorRequests_AreFormattedCorrectly_WhenMultipleAddressesEntered(string[] fileContent)
    {
        // Set up
        var file = CreateFile(fileContent); 

        // Execute
        var results = _mockAddressValidator.ValidateFromCsv(file);

        // Validate results
        List<ValidatorRequest> requests = results.Select(result => JsonConvert.DeserializeObject<ValidatorRequest>(result)).ToList();
        foreach (var request in requests){
            Assert.AreEqual("6601 Tennyson St NE", request.RequestContent.Single(c => c.Key == "StreetAddress").Value);
            Assert.AreEqual("Albuquerque", request.RequestContent.Single(c => c.Key == "City").Value);
            Assert.AreEqual("87111", request.RequestContent.Single(c => c.Key == "PostalCode").Value);
        }

        // Clean up
        File.Delete(file);
    }

    [DataTestMethod]
    [DataRow(new string[] { "\"Street Address, City, Postal Code\"", "\"6601 Tennyson St NE, Albuquerque, 87111\"" })]
    [DataRow(new string[] { "\"Street Address, City, Postal Code", "6601 Tennyson St NE, Albuquerque, 87111\"" })]
    public void ValidatorRequest_IsFormattedCorrectly_WhenAddressSurroundedByQuotes(string[] fileContent)
    {
        // Set up
        var file = CreateFile(fileContent); 

        // Execute
        var results = _mockAddressValidator.ValidateFromCsv(file);

        // Validate results
        List<ValidatorRequest> requests = results.Select(result => JsonConvert.DeserializeObject<ValidatorRequest>(result)).ToList();
        foreach (var request in requests){
            Assert.AreEqual("6601 Tennyson St NE", request.RequestContent.Single(c => c.Key == "StreetAddress").Value);
            Assert.AreEqual("Albuquerque", request.RequestContent.Single(c => c.Key == "City").Value);
            Assert.AreEqual("87111", request.RequestContent.Single(c => c.Key == "PostalCode").Value);
        }

        // Clean up
        File.Delete(file);
    }

    [DataTestMethod]
    [DataRow(new string[] { "Street Address, City, Postal Code", "1 Empora St, Title, 11111" })]
    public void Validate_ReturnsInvalidAddress_WhenAddressIsInvalid(string[] fileContent)
    {
        // Set up
        var file = CreateFile(fileContent); 

        // Execute
        var results = _addressValidator.ValidateFromCsv(file);

        // Validate results
        Assert.AreEqual($"{fileContent[1]} -> Invalid Address", results.Single());

        // Clean up
        File.Delete(file);
    }

    [DataTestMethod]
    [DataRow(new string[] { "Street Address, City, Postal Code", "6601 Tennyson St NE, Albuquerque, 87111"  })]
    public void Validate_ReturnsFormattedAddress_WhenAddressIsValid(string[] fileContent)
    {
        // Set up
        var file = CreateFile(fileContent); 

        // Execute
        var results = _addressValidator.ValidateFromCsv(file);

        // Validate results
        Assert.AreEqual($"{fileContent[1]} -> 6601 Tennyson St NE,Albuquerque NM 87111-8161", results.Single());

        // Clean up
        File.Delete(file);
    }

    [DataTestMethod]
    [DataRow(new string[] { "Street Address, City, Postal Code", "123 e Maine Street, Columbus, 43215"  })]
    public void Validate_ReturnsStatus_WhenAddressUnableToBeValidated(string[] fileContent)
    {
        // Set up
        var file = CreateFile(fileContent); 

        // Execute
        var results = _addressValidator.ValidateFromCsv(file);

        // Validate results
        Assert.AreEqual($"{fileContent[1]} -> returned status SUSPECT; diagnostics: ward mult", results.Single());

        // Clean up
        File.Delete(file);
    }

    [TestMethod]
    public void Validate_ThrowsException_WhenFileEmpty()
    {
        // Set up
        var file = CreateFile(new string[0]); 

        // Execute and validate
        Assert.ThrowsException<Exception>(() => _addressValidator.ValidateFromCsv(file));

        // Clean up
        File.Delete(file);
    }


    [DataTestMethod]
    [DataRow("/Users/andiscarcello/Documents/address-validator/Tests/Addresses.csv")]
    public void Validate_FromActualFile_ReturnsListOfResponses(string filePath)
    {
         // Execute
        var results = _addressValidator.ValidateFromCsv(filePath);

        // Validate results
        Assert.AreEqual("6601 Tennyson St NE, Albuquerque, 87111 -> 6601 Tennyson St NE,Albuquerque NM 87111-8161", results[0]);
        Assert.AreEqual("4203 239th PL SE, Issaquah, 98029 -> 4203 239th Pl SE,Sammamish WA 98029-7536", results[1]);
        Assert.AreEqual("123 e Maine Street, Columbus, 43215 -> returned status SUSPECT; diagnostics: ward mult", results[2]);
        Assert.AreEqual("1 Empora St, Title, 11111 -> Invalid Address", results[3]);
    }

    private string CreateFile(string[] content)
    {
        string fileName;
        do {
            fileName = $"{System.IO.Path.GetTempFileName()}.csv";
        } while (System.IO.File.Exists(fileName));

        File.WriteAllLines(fileName, content);
        return fileName;
    }
}