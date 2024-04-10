Feature: Users Page

  Scenario: User can see all users
    Given I am on the "chat" page
    And I click the button to "users" page
    Then I am on the "user-page" page
    Then I should see all users in a table

  Scenario: Delete a user
    Given I am on the "user-page" page
    When I click the delete button for the user "new_username"
    Then I should see "User deleted." message