@certification
Feature: Certification

As a registered user, I would like to add certification details to my profile page
so that others can see the certifications I have attained.

Background:
	Given I navigate to the profile page as a registered user

Scenario: Validate if the user is able to add certifications using external JSON file
	When I enter certification details from json file with the TestName "AddCertificationDetails_ValidInput"
	Then I should see the success message

Scenario: Validate if the user is not able to add the certification details with invalid certificate or award
	When I enter invalid certification details from json file with the TestName "AddCertificationDetails_InvalidCertificateOrAward"
	Then I should see the error message

Scenario: Validate if the user is not able to add the certification details with invalid certificate from
	When I enter invalid certification details from json file with the TestName "AddCertificationDetails_InvalidCertificateFrom"
	Then I should see the error message

Scenario: Validate if the user is not able to add the certification details with huge string as Certificate or Award
	When I enter lengthy Certificate or Award details from json file with the TestName "AddCertificationDetails_MoreThan250CharactersOfCertificateOrAward"
	Then I should see the error message

Scenario: Validate if the user is not able to add the certification details with huge string as Certificate from
	When I enter lengthy Certificate from details from json file with the TestName "AddCertificationDetails_MoreThan250CharactersOfCertificateFrom"
	Then I should see the error message

Scenario: Validate if the user is not able to add certification details by giving either one or all of the fields empty
	When I leave either one or all the fields empty and give the data from json file with the TestName "AddCertificationDetails_LeaveEitherOneOrAllTheFieldsEmpty"
	Then I should see the error message for empty fields

Scenario: Validate if the user is not able to add the certification details which already exists in the list (Duplicate data)
	When I enter same certification details twice from json file with the TestName "AddCertificationDetails_DuplicateData"
	Then I should see the error message for duplicate data

Scenario: Validate if the user is not able to add certification details when the session has expired
	When I enter certification details from json file after the session has expired with the TestName "AddCertificationDetails_WhenSessionExpired"
	Then I should see the error message for session expired

Scenario: Validate if the user is not able to add the Certificate or Award and Certified From (e.g. Adobe) combinations aren't matching
	When I enter certification details from json file with the TestName "AddCertificationDetails_CertificateOrAwardAndCertificateFromMismatch"
	Then I should see the error message for certificate and provider mismatch

Scenario: Validate if the user is able to cancel the add process
	When I enter certification details from json file and cancel the add with the TestName "AddCertificationDetails_Cancel"
	Then I should see the certification details shouldn't be added

Scenario: Validate if the user is able to update certification details using external JSON file
	When I update certification details with the existing details from json file with the TestName "UpdateCertificationDetails_ValidInput"
	Then I should see the success message for update

Scenario: Validate if the user is not able to update the certification details with invalid Certificate or Award
	When I update certification details with the existing details from json file with the TestName "UpdateCertificationDetails_InvalidCertificateOrAward"
	Then I should see the error message for update invalid data

Scenario: Validate if the user is not able to update the certification details with invalid Certificate from
	When I update certification details with the existing details from json file with the TestName "UpdateCertificationDetails_InvalidCertificateFrom"
	Then I should see the error message for update invalid data

Scenario: Validate if the user is not able to update the certification details with huge Certificate or Award
	When I update lengthy Certificate or Award details with the existing details from json file with the TestName "UpdateCertificationDetails_MoreThan250CharactersOfCertificateOrAward"
	Then I should see the error message for updating huge string

Scenario: Validate if the user is not able to update the certification details with huge Certificate from
	When I update lengthy Certificate From details with the existing details from json file with the TestName "UpdateCertificationDetails_MoreThan250CharactersOfCertificateFrom"
	Then I should see the error message for updating huge string

Scenario: Validate if the user is not able to update certification details by giving either one or all of the fields empty
	When I leave either one or all the fields empty and give the data to update from json file with the TestName "UpdateCertificationDetails_LeaveEitherOneOrAllTheFieldsEmpty"
	Then I should see the error message for empty fields

Scenario: Validate if the user is not able to update the certification details which already exists in the list (Duplicate data)
	When I update same certification details twice from json file with the TestName "UpdateCertificationDetails_DuplicateData"
	Then I should see the error message for duplicate data

Scenario: Validate if the user is able to cancel the update process
	When I enter certification details from json file and cancel the update with the TestName "UpdateCertificationDetails_Cancel"
	Then I should see the added certification not the updated one

Scenario: Validate if the user is not able to update certification details when the session has expired
	When I update certification details from json file after the session has expired with the TestName "UpdateCertificationDetails_WhenSessionExpired"
	Then I should see the error message for session expired
	Then I should log in again to perform clean up

Scenario: Validate if the user is not able to update the Certificate or Award and Certified From (e.g. Adobe) combinations aren't matching
	When I update certification details with the existing details from json file with the TestName "UpdateCertificationDetails_CertificateOrAwardAndCertificateFromMismatch"
	Then I should see the error message for certificate and provider mismatch

Scenario: Validate if the user is able to add huge data in the"Certificate or Award" and  "Certified From (e.g. Adobe)" field
	When I enter huge Certificate or Award details to perform add from json file with the TestName "AddCertificationDetails_DestructiveTesting"
	Then I should see the error message

Scenario: Validate if the user is able to add and update huge data in the"Certificate or Award" and  "Certified From (e.g. Adobe)" field
	When I enter huge Certificate or Award details to perform update from json file with the TestName "UpdateCertificationDetails_DestructiveTesting"
	Then I should see the error message

Scenario: Validate if the user is able to delete the certification details
	When I delete certification details from json file with the TestName "DeleteCertificationDetails_ValidInput"
	Then I should see the success message for delete

Scenario: Validate if the user is not able to delete the certification details during session expired
	When I delete certification details from json file after the session has expired with the TestName "DeleteCertificationDetails_WhenSessionExpired"
	Then I should see the error message for session expired
	Then I should log in again to perform clean up



	 