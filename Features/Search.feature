Feature: Search

A short summary of the feature

@failing
Scenario: Search after login on OrangeHRM - failing assertion
	
	Given I navigate to the login page with url AppUrl
	When I enter username UserName
	And I enter password Password
	And I click login
	Then I should see the dashboard
	When I perform a search for "this-text-will-not-exist"
	Then I should see search results contain "this-text-will-not-exist"

