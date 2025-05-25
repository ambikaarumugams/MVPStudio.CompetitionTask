Feature: Education

As a registered user, I would like to add education details into my profile page
So that others can see about my educational background

Background:
	Given I navigate to the profile page as a registered user

@tag1
Scenario: Validate if the user is able to add education details using external JSON file
	When I click the "Add New" button
	And I enter education details from JSON file
	And I click the "Add" button
	Then I should see the success message
	