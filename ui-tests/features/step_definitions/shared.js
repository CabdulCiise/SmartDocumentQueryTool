const { By } = require("selenium-webdriver");
const { Given, When, Then } = require("cucumber");
const assert = require("assert");

async function checkForModal(expectedModalHeader) {
  const modalHeader = await this.driver.findElement(
    By.css(".p-dialog-header span")
  );

  const modalHeaderText = await modalHeader.getText();

  assert.equal(modalHeaderText, expectedModalHeader);
}

Given("I see the {string} modal", async function (expectedModalHeader) {
  await this.driver.sleep(500);

  checkForModal.call(this, expectedModalHeader);
});

Given("I am on the {string} page", async function (pageName) {
  await this.driver.findElement(By.css(`.${pageName}`));
});

When("I refresh the page", async function () {
  await this.driver.navigate().refresh();
  await this.driver.sleep(500);
});

When("I click the user profile icon", async function () {
  const profileIcon = await this.driver.findElement(
    By.css(".home__profile button")
  );
  await profileIcon.click();
});

When("I click the {string} option in the dropdown", async function (option) {
  const userOptions = await this.driver.findElements(By.css("#overlay_menu a"));

  if (option === "logout") {
    await userOptions[1].click();
  } else if (option === "profile") {
    await userOptions[0].click();
  } else {
    assert.fail("Option not found.");
  }

  await this.driver.sleep(500);
});

Then("I should see {string} message", async function (message) {
  const messageElement = await this.driver.findElement(
    By.css(".p-toast .p-toast-detail")
  );
  const displayedMessage = await messageElement.getText();
  assert.equal(displayedMessage, message);

  await this.driver.sleep(2000);
});

Then("The {string} modal is displayed", async function (modalName) {
  const modalHeader = await this.driver.findElement(
    By.css(".p-dialog-header span")
  );
  const modalHeaderText = await modalHeader.getText();
  assert.equal(modalHeaderText, modalName);
});

Then("I click the modal close button", async function () {
  const closeButton = await this.driver.findElement(
    By.css(".p-dialog-header-icons button")
  );
  await closeButton.click();
  await this.driver.sleep(500);
});

Then("The modal should close", async function () {
  const modals = await this.driver.findElements(By.css(".p-dialog-header"));

  if (modals.length > 0) {
    assert.fail("Modal is still open.");
  }
});
