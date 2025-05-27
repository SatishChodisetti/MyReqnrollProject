using System;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Reqnroll;
using RestSharp;

namespace MyReqnrollProject.StepDefinitions;

[Binding]
public sealed class APIStepDefinitions
{
    private RestRequest _request;
    private RestClient _client;
    private RestResponse _response;
    [Then("the response should contain a list of {string}")]
    public void ThenTheResponseShouldContainAListOf(string s)
    {
        var json = JObject.Parse(_response.Content);
        Assert.That(json.ContainsKey("data"), Is.True, "Response does not contain 'data'");
        var data = json["data"];
        Assert.That(data.Type == JTokenType.Array, "Expected 'data' to be an array");
        Console.WriteLine($"Resource data count: {data.Count()}");
    }

    [Then("the response status code should be {int}")]
    public void ThenTheResponseStatusCodeShouldBe(int i)
    {
        Assert.That((int)_response.StatusCode, Is.EqualTo(200), "Expected status code 200");
    }

    [When("I send a GET request to the endpoint")]
    public void WhenISendAGETRequestToTheEndpoint()
    {
        _response = _client.Execute(_request);
    }

    [Given("I have the API endpoint for {string} with page {string} and per_page {string}")]
    public void GivenIHaveTheAPIEndpointForWithPageAndPer_page(string s, string s2, string s3)
    {
        _client = new RestClient("https://reqres.in/");
        _request = new RestRequest("api/users", Method.Get);
        _request.AddParameter("page", 1).AddParameter("per_page", 2).AddHeader("x-api-key", "reqres-free-v1");
    }


}