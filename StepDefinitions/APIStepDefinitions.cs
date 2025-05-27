using System;
using Reqnroll;

namespace MyReqnrollProject.StepDefinitions;

[Binding]
public sealed class APIStepDefinitions
{

    [Then("the response should contain a list of {string}")]
    public void ThenTheResponseShouldContainAListOf(string s)
    {
        Console.WriteLine("API Test 1");
    }

    [Then("the response status code should be {int}")]
    public void ThenTheResponseStatusCodeShouldBe(int i)
    {
        Console.WriteLine("API Test 2");
    }

    [When("I send a GET request to the endpoint")]
    public void WhenISendAGETRequestToTheEndpoint()
    {
        Console.WriteLine("API Test 3");
    }

    [Given("I have the API endpoint for {string} with page {string} and per_page {string}")]
    public void GivenIHaveTheAPIEndpointForWithPageAndPer_page(string s, string s2, string s3)
    {
        Console.WriteLine("API Test 4");
    }


}