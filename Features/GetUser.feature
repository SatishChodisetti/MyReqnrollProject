Feature: Get paginated resource data from Reqres API

  Scenario Outline: Verify successful response from Reqres for <resource> with pagination
    Given I have the API endpoint for "<resource>" with page "<page>" and per_page "<per_page>"
    When I send a GET request to the endpoint
    Then the response status code should be 200
    And the response should contain a list of "<resource>"

    Examples:
      | resource | page | per_page |
      | users    | 1    | 2        |
      | unknown  | 1    | 2        |