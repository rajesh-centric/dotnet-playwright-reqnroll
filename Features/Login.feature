Feature: Login

Feature: Login
  As a user
  I want to log into the application
  So that I can access my account

  Scenario: Successful login
    Given I navigate to the login page with url AppUrl
    When I enter username UserName
	And  I enter password admin123
    And I click login
    Then I should see the dashboard

Scenario: Login with incorrect password
    Given I navigate to the login page with url AppUrl
    When I enter username UserName
	And  I enter password incorrectPassword
    And I click login
    Then I should see login error message Invalid Credential
    #And I should remain on the login page


