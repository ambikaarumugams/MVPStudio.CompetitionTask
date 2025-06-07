@certifications
Feature: Certifications

As a registered user, I would like to add certification details to my profile page
so that others can see the certifications I have attained.

Background:
	Given I navigate to the profile page as a registered user

Scenario: Validate if the user is able to add certifications using external JSON file
	When I enter certification details from json file with the TestName "Add certification details with valid input"
	Then I should see the success message

Scenario: Validate if the user is not able to add the certification details with invalid certificate or award 
	When I enter invalid certification details from json file with the TestName "Add certification details with invalid Certificate or Award"
	Then I should see the error message

Scenario: Validate if the user is not able to add the certification details with invalid certificate from
	When I enter invalid certification details from json file with the TestName "Add certification details with invalid Certificate from"
	Then I should see the error message

Scenario: Validate if the user is not able to add the certification details with huge string as Certificate or Award 
	When I enter lengthy Certificate or Award details from json file with the TestName ">250 characters of Certificate or Award"
	Then I should see the error message 

Scenario: Validate if the user is not able to add the certification details with huge string as Certificate from
	When I enter lengthy Certificate from details from json file with the TestName ">250 characters of Certificate from"
	Then I should see the error message