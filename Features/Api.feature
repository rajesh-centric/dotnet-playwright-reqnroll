Feature: Apifeature

  Scenario: Successful login on API
    When I requested the get user API status code is 200
    When I enter username UserName
	And  I enter password Password
    And I click login
    Then I should see the dashboard