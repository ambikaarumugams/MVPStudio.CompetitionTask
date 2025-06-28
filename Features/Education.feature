@education
Feature: Education

As a registered user, I would like to add education details into my profile page
So that others can see about my educational background

Background:
	Given I navigate to the profile page as a registered user

Scenario: Validate if the user is able to add education details using external JSON file
	When I enter education details from json file with the TestName "AddEducationDetails_ValidInput"
	Then I should see the success message

Scenario: Validate if the user is not able to add the education details with invalid College/University Name
	When I enter invalid education details from json file with the TestName "AddEducationDetails_InvalidCollegeUniversityName"
	Then I should see the error message

Scenario: Validate if the user is not able to add the education details with invalid Degree
	When I enter invalid education details from json file with the TestName "AddEducationDetails_InvalidDegree"
	Then I should see the error message

Scenario: Validate if the user is not able to add the education details with huge College/University Name
	When I enter education details from json file with the TestName "AddEducationDetails_MoreThan250CharactersOfCollegeUniversityName"
	Then I should see the error message for adding huge string

Scenario: Validate if the user is not able to add the education details with huge Degree Name
	When I enter education details from json file with the TestName "AddEducationDetails_MoreThan250CharactersOfDegreeName"
	Then I should see the error message for adding huge string

Scenario: Validate if the user is not able to add education details by giving either one or all of the fields empty
	When I leave either one or all the fields empty and give the data from json file with the TestName "AddEducationDetails_LeaveEitherOneOrAllTheFieldsEmpty"
	Then I should see the error message for empty fields

Scenario: Validate if the user is not able add same education details multiple times
	When I enter same education details twice from json file with the TestName "AddEducationDetails_DuplicateData"
	Then I should see the error message for duplicate data

Scenario: Validate if the user is not able to add education details when the session has expired
	When I enter education details from json file after the session has expired with the TestName "AddEducationDetails_WhenSessionExpired"
	Then I should see the error message for session expired

Scenario: Validate if the user is not able add education details with valid input
	When I enter education details from json file with the TestName "AddEducationDetails_NegativeTestingWithValidInput"
	Then I should see the error message for adding education details 

Scenario: Validate if the user is able to cancel the add process
	When I enter education details from the Json file with the test name "AddEducationDetails_Cancel"
	Then I should see the education details shouldn't be added

Scenario: Validate if the user is able to update education details using external JSON file
	When I update education details with the existing details from json file with the TestName "UpdateEducationDetails_ValidInput"
	Then I should see the success message for update

Scenario: Validate if the user is not able to update the education details with invalid College/University Name
	When I enter invalid education details to update from json file with the TestName "UpdateEducationDetails_InvalidCollegeUniversityName"
	Then I should see the error message for update invalid data

Scenario: Validate if the user is not able to update the education details with invalid Degree
	When I enter invalid education details to update from json file with the TestName "UpdateEducationDetails_InvalidDegreeName"
	Then I should see the error message for update invalid data

Scenario: Validate if the user is not able to update the education details with huge College/University Name
	When I update education details with the existing details from json file with the TestName "UpdateEducationDetails_MoreThan250CharactersOfCollegeUniversityName"
	Then I should see the error message for updating huge string

Scenario: Validate if the user is not able to update the education details with huge Degree Name
	When I update education details with the existing details from json file with the TestName "UpdateEducationDetails_MoreThan250CharactersOfDegreeName"
	Then I should see the error message for updating huge string

Scenario: Validate if the user is not able to update education details by giving either one or all of the fields empty
	When I leave either one or all the fields empty and give the data to update from json file with the TestName "UpdateEducationDetails_LeaveEitherOneOrAllTheFieldsEmpty"
	Then I should see the error message for empty fields

Scenario: Validate if the user is not able to update education details when the session has expired
	When I update education details from json file after the session has expired with the TestName "UpdateEducationDetails_WhenSessionExpired"
	Then I should see the error message to update for session expired
	Then I should login again to perform cleanup

Scenario:  Validate if the user is able to delete the education details 
    When I delete education details from json file with the TestName "DeleteEducationDetails_ValidInput"
	Then I should see the success message for delete

Scenario: Validate if the user is not able to delete education details when the session has expired
	When I delete education details from json file after the session has expired with the TestName "DeleteEducationDetails_WhenSessionExpired"
	Then I should see the error message to delete for session expired
	Then I should login again to perform cleanup

Scenario: Validate if the user is not able to update the education details which already exists in the list (Duplicate data)
	When I update education details with the same existing details from json file with the TestName "UpdateEducationDetails_DuplicateData"
	Then I should see the error message for duplicate data

Scenario:Validate if the user is not able to update education details with valid input
	When I update education details with the existing details from json file with the TestName "UpdateEducationDetails_NegativeTestingWithValidInput"
	Then I should see the error message for updating education details 

Scenario:Validate if the user is not able to add huge data in the education field
	When I enter education details for destructive testing from json file with the TestName "AddEducationDetails_DestructiveTesting"
	Then I should see the error message for huge data

Scenario: Validate if the user is not able to update education details with huge data
	When I update education details with the existing details for destructive testing from json file with the TestName "UpdateEducationDetails_DestructiveTesting"
	Then I should see the error message for huge data