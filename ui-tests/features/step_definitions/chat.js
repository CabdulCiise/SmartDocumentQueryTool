const assert = require("assert");
const { When, Then } = require("cucumber");
const { By, Key } = require("selenium-webdriver");
const { ConsoleLogEntry } = require("selenium-webdriver/bidi/logEntries");

Then("I should see the chat history", async function () {
  const chatHistory = await this.driver.findElement(
    By.css(".home__chat-history")
  );
});

Then("I click on a different chat", async function () {
  const chatHistory = await this.driver.findElement(
    By.css(".home__chat-history")
  );
  const chats = await chatHistory.findElements(By.css("button"));

  await chats[chats.length - 1].click();
});

Then("I should see chat messages", async function () {
  const chatMessages = await this.driver.findElements(
    By.css(".chat__inner .chat__bubble")
  );

  assert(chatMessages.length > 0, "There should be chat messages.");
});

Then("I click the {string} chat button", async function (buttonName) {
  const buttons = await this.driver.findElements(By.css(".chat__bar button"));

  let button;

  if (buttonName === "delete") {
    button = buttons[0];
  } else if (buttonName === "reset") {
    button = buttons[1];
  } else {
    throw new Error("Invalid chat action name.");
  }

  button.click();
  await this.driver.sleep(500);
});

Then("I should see no chat messages", async function () {
  const chatMessages = await this.driver.findElements(
    By.css(".chat__inner .chat__bubble")
  );

  assert(chatMessages.length === 0, "There should be no chat messages.");
});

Then("I should see the chat prompt", async function () {
  const chatPrompt = await this.driver.findElement(By.css(".chat__bar"));
});

When(
  "I submit {string} in the chat prompt",
  { timeout: 30000 },
  async function (message) {
    const chatPrompt = await this.driver.findElement(
      By.css(".chat__bar input")
    );
    chatPrompt.sendKeys(message);
    await this.driver.sleep(500);

    chatPrompt.sendKeys(Key.ENTER);
    await this.driver.sleep(5000);
  }
);

Then(
  "I should see {int} messages total in the chat",
  async function (expectedMessagesCount) {
    const chatMessages = await this.driver.findElements(
      By.css(".chat__inner .chat__bubble")
    );

    const actualCount = chatMessages.length;
    assert.equal(
      actualCount,
      expectedMessagesCount,
      `There should be ${expectedMessagesCount} chat messages.`
    );
  }
);
