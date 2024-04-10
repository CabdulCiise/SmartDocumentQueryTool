const { By } = require("selenium-webdriver");
const { When, Then } = require("cucumber");
const assert = require("assert");

Then("I click the button to {string} page", async function (pageName) {
  const buttons = await this.driver.findElements(
    By.css(".home__sidebar button")
  );

  let button;

  if (pageName === "chat") {
    button = buttons[0];
  } else if (pageName === "users") {
    button = buttons[2];
  } else if (pageName === "document-editor") {
    button = buttons[2];
  } else if (pageName === "user-feedback") {
    button = buttons[2];
  } else {
    throw new Error(`Unknown page name: ${pageName}`);
  }

  await button.click();
});

Then("I should see all users in a table", async function () {
  const tableBody = await this.driver.findElement(
    By.css(".user-page table tbody")
  );
  const rows = await tableBody.findElements(By.css("tr"));

  assert(
    rows.length > 1,
    "Expected to find users in the table, but none were found."
  );
});

When(
  "I click the delete button for the user {string}",
  async function (username) {
    const tableBody = await this.driver.findElement(
      By.css(".user-page table tbody")
    );
    const rows = await tableBody.findElements(By.css("tr"));

    let deleteButton;
    for (let row of rows) {
      const cells = await row.findElements(By.css("td"));
      const cellText = await cells[0].getText();

      if (cellText === username) {
        deleteButton = await row.findElement(By.css("button"));
        break;
      }
    }

    if (!deleteButton) {
      throw new Error(`User ${username} not found.`);
    }

    await deleteButton.click();
    await this.driver.sleep(500);
  }
);
