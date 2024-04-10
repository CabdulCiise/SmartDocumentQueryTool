Feature: Chat

  Scenario: Chat History Present
    Given I click the button to "chat" page
    Then I am on the "chat" page
    Then I should see the chat history
    And I click on a different chat

  Scenario: Delete Chat
    Given I am on the "chat" page
    Then I should see chat messages
    When I click the "delete" chat button
    Then I should see "Chat deleted." message

  Scenario: Reset Chat
    Given I am on the "chat" page
    Then I should see chat messages
    When I click the "reset" chat button
    Then I should see no chat messages

  Scenario: Chat Prompting
    Given I am on the "chat" page
    Then I should see the chat prompt
    When I submit "What can you tell me about redis?" in the chat prompt
    Then I should see 2 messages total in the chat