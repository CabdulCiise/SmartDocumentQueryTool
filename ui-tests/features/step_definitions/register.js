const { By } = require("selenium-webdriver");
const { When, Then } = require("cucumber");
const assert = require("assert");

async function fillUserRegistrationForm(username, password, confirmPassword) {
  const usernameInput = await this.driver.findElement(
    By.css('input[placeholder="Enter Username"]')
  );
  const passwordInput = await this.driver.findElement(
    By.css('input[placeholder="Password"]')
  );
  const confirmPasswordInput = await this.driver.findElement(
    By.css('input[placeholder="Repeat your password"]')
  );

  await usernameInput.clear();
  await passwordInput.clear();
  await confirmPasswordInput.clear();

  await usernameInput.sendKeys(username);
  await passwordInput.sendKeys(password);
  await confirmPasswordInput.sendKeys(confirmPassword);
}

When("I click the Register link", async function () {
  const registerLink = await this.driver.findElement(
    By.css(".login-dialog__register a")
  );
  await registerLink.click();

  await this.driver.sleep(1000);
});

Then("I click the Register button", async function () {
  const registerButton = await this.driver.findElement(
    By.css('button[type="submit"]')
  );
  await registerButton.click();
  await this.driver.sleep(250);
});

Then("I should see input fields in invalid state", async function () {
  const inputsWithError = await this.driver.findElements(
    By.css("input[required]:invalid")
  );

  assert(
    inputsWithError.length > 0,
    "Expected to find input fields in invalid state, but none were found."
  );
});

Then(
  "I fill registration form with {string}",
  async function (registrationType) {
    let username = "admin";
    let password = "admin";
    let confirmPassword = "admin";

    if (registrationType === "existing username") {
    } else if (registrationType === "mismatched passwords") {
      username = "new_username";
      password = "password1";
      confirmPassword = "password2";
    } else if (registrationType === "valid input") {
      username = "new_username";
      password = "password";
      confirmPassword = "password";
    } else {
      throw new Error(`Unknown registration type: ${registrationType}`);
    }

    await fillUserRegistrationForm.call(
      this,
      username,
      password,
      confirmPassword
    );
    await this.driver.sleep(250);
  }
);
