Feature: Registering New User

  Scenario: Display Register Modal
    Given I see the "LOG IN" modal
    When I click the Register link
    Then I see the "CREATE AN ACCOUNT" modal

  Scenario: Register Without Input
    Given I see the "CREATE AN ACCOUNT" modal
    And I click the Register button
    Then I should see input fields in invalid state

  Scenario: Register with Existing Username
    Given I see the "CREATE AN ACCOUNT" modal
    Then I fill registration form with "existing username"
    And I click the Register button
    Then I should see "Username not available. Try a different one." message

  Scenario: Register with Mismatched Passwords
    Given I see the "CREATE AN ACCOUNT" modal
    Then I fill registration form with "mismatched passwords"
    And I click the Register button
    Then I should see "Passwords do not match." message

  Scenario: Register with Valid Input
    Given I see the "CREATE AN ACCOUNT" modal
    Then I fill registration form with "valid input"
    And I click the Register button
    Then I am on the "chat" page
    
  Scenario: Logout
    Given I am on the "chat" page
    Then I click the user profile icon
    Then I click the "logout" option in the dropdown
    Then I see the "LOG IN" modal
