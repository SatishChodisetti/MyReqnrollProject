using System;
using Reqnroll;

namespace MyReqnrollProject.StepDefinitions;

[Binding]
public sealed class CalculatorStepDefinitions
{

    [Given("the first number is {int}")]
    public void GivenTheFirstNumberIs(int number)
    {
        //TODO: implement arrange (precondition) logic
        // For storing and retrieving scenario-specific data see https://go.reqnroll.net/doc-sharingdata
        // To use the multiline text or the table argument of the scenario,
        // additional string/DataTable parameters can be defined on the step definition
        // method. 

        Console.WriteLine("This is the first step");
    }

    [Given("the second number is {int}")]
    public void GivenTheSecondNumberIs(int number)
    {

        Console.WriteLine("This is the second step");
    }

    [When("the two numbers are added")]
    public void WhenTheTwoNumbersAreAdded()
    {

        Console.WriteLine("This is the Third step");
    }

    [Then("the result should be {int}")]
    public void ThenTheResultShouldBe(int result)
    {

         Console.WriteLine("This is the fourth step");
    }

    
   
}
