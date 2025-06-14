@education
Feature: Education

As a registered user, I would like to add education details into my profile page
So that others can see about my educational background

Background:
	Given I navigate to the profile page as a registered user

Scenario: Validate if the user is able to add education details using external JSON file
	When I enter education details from json file with the TestName "Add education details with valid input"
	Then I should see the success message

Scenario: Validate if the user is not able to add the education details with invalid College/University Name
	When I enter invalid education details from json file with the TestName "Add education details with invalid CollegeUniversityName"
	Then I should see the error message

Scenario: Validate if the user is not able to add the education details with invalid Degree
	When I enter invalid education details from json file with the TestName "Add education details with invalid Degree"
	Then I should see the error message

Scenario: Validate if the user is not able to add the education details with huge College/University Name
	When I enter education details from json file with the TestName ">250 characters of College/University Name"
	Then I should see the error message for adding huge string

Scenario: Validate if the user is not able to add the education details with huge Degree Name
	When I enter education details from json file with the TestName ">250 characters of Degree Name"
	Then I should see the error message for adding huge string

Scenario: Validate if the user is not able to add education details by giving either one or all of the fields empty
	When I leave either one or all the fields empty and give the data from json file with the TestName "Leave either one or all the fields empty"
	Then I should see the error message for empty fields

Scenario: Validate if the user is not able add same education details multiple times
	When I enter same education details twice from json file with the TestName "Add same education details multiple times"
	Then I should see the error message for duplicate data

Scenario: Validate if the user is not able to add education details when the session has expired
	When I enter education details from json file after the session has expired with the TestName "Add education details when the session has expired"
	Then I should see the error message for session expired

Scenario: Validate if the user is not able add education details with valid input
	When I enter education details from json file with the TestName "Negative testing with valid input"
	Then I should see the error message for adding education details 

Scenario: Validate if the user is able to cancel the add process
	When I enter education details from the Json file with the test name "Cancel add process"
	Then I should see the education details shouldn't be added

Scenario: Validate if the user is able to update education details using external JSON file
	When I update education details with the existing details from json file with the TestName "Update education details with valid input"
	Then I should see the success message for update

Scenario: Validate if the user is not able to update the education details with invalid College/University Name
	When I enter invalid education details to update from json file with the TestName "Update education details with invalid College/University Name"
	Then I should see the error message for update invalid data

Scenario: Validate if the user is not able to update the education details with invalid Degree
	When I enter invalid education details to update from json file with the TestName "Update education details with invalid Degree"
	Then I should see the error message for update invalid data

Scenario: Validate if the user is not able to update the education details with huge College/University Name
	When I update education details with the existing details from json file with the TestName ">250 characters of College/University Name for update"
	Then I should see the error message for updating huge string

Scenario: Validate if the user is not able to update the education details with huge Degree Name
	When I update education details with the existing details from json file with the TestName ">250 characters of Degree Name for update"
	Then I should see the error message for updating huge string

Scenario: Validate if the user is not able to update education details by giving either one or all of the fields empty
	When I leave either one or all the fields empty and give the data to update from json file with the TestName "Leave either one or all the fields empty for update"
	Then I should see the error message for empty fields

Scenario: Validate if the user is not able to update education details when the session has expired
	When I update education details from json file after the session has expired with the TestName "Update education details when the session has expired"
	Then I should see the error message to update for session expired

Scenario: Validate if the user is not able to delete education details when the session has expired
	When I delete education details from json file after the session has expired with the TestName "Delete education details when the session has expired"
	Then I should see the error message to delete for session expired

Scenario: Validate if the user is not able to update the education details which already exists in the list (Duplicate data)
	When I update education details with the same existing details from json file with the TestName "Update the education details as same as the existing one"
	Then I should see the error message for duplicate data

	Scenario:Validate if the user is not able to update education details with valid input
	When I update education details with the existing details from json file with the TestName "Negative testing with valid input for update"
	Then I should see the error message for updating education details 

	Scenario:Validate if the user is able to add huge data in the education field
	When I enter education details for destructive testing from json file with the TestName "Add education details with huge text"
	Then I should see the error message for huge data

	Scenario: Validate if the user is able to update education details with huge data
	When I update education details with the existing details for destructive testing from json file with the TestName "Update education details with huge text"
	Then I should see the error message for huge data