Feature: Custom Instruction
  
  Scenario: Show User Profile Modal
    When I click the user profile icon
    And I click the "profile" option in the dropdown
    Given I see the "USER PROFILE" modal

  Scenario: Update Custom Instruction
    Given I see the "USER PROFILE" modal
    When I edit user custom instruction to "Be as short as possible."
    And I click on the save button
    Then The modal should close

  Scenario: Check Persistance of Custom Instruction
    Given I am on the "chat" page
    When I refresh the page
    And I click the user profile icon
    Then I click the "profile" option in the dropdown
    And I should see that the user custom instruction are "Be as short as possible."
    Then I click the modal close button
    And The modal should close
