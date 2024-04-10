const { By } = require("selenium-webdriver");
const { Given, Then } = require("cucumber");

async function login(username, password) {
  const usernameInput = await this.driver.findElement(
    By.css('input[placeholder="Enter Username"]')
  );
  await usernameInput.sendKeys(username);

  const passwordInput = await this.driver.findElement(
    By.css('input[placeholder="Password"]')
  );
  await passwordInput.sendKeys(password);

  const submitButton = await this.driver.findElement(
    By.css('button[type="submit"]')
  );
  await submitButton.click();
}

Then("I log in", async function () {
  login.call(this, "admin", "admin");

  await this.driver.sleep(500);
});
