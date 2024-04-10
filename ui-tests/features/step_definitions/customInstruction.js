const { By } = require("selenium-webdriver");
const { When, Then } = require("cucumber");
const assert = require("assert");

When(
  "I edit user custom instruction to {string}",
  async function (instruction) {
    const customInstructionTextArea = await this.driver.findElement(
      By.css(".profile-editor textarea")
    );
    await customInstructionTextArea.clear();
    await customInstructionTextArea.sendKeys(instruction);
  }
);

Then("I click on the save button", async function () {
  const saveButton = await this.driver.findElement(
    By.css(".profile-editor button")
  );
  await saveButton.click();
  await this.driver.sleep(500);
});

Then(
  "I should see that the user custom instruction are {string}",
  async function (instruction) {
    const textArea = await this.driver.findElement(
      By.css(".profile-editor textarea")
    );

    const userInstruction = await textArea.getAttribute("value");
    assert.equal(userInstruction, instruction);
  }
);
