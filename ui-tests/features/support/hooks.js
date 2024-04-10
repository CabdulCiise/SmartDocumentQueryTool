const { BeforeAll, AfterAll, Before, After } = require("cucumber");
const { Builder, Capabilities } = require("selenium-webdriver");
require("chromedriver");

const url = "http://localhost:5173/";

let driver;

BeforeAll(async () => {
  const capabilities = Capabilities.chrome();
  capabilities.set("chromeOptions", { w3c: false });
  driver = new Builder().withCapabilities(capabilities).build();

  await driver.manage().window().maximize();
  await driver.get(url);
});

AfterAll(async () => {
  if (driver) {
    await driver.quit();
  }
});

Before(async function () {
  this.driver = driver;
});

After(async function () {
  const scenario = this.scenario;
  if (scenario && scenario.result.status === "failed") {
    const screenshot = await driver.takeScreenshot();
    this.attach(screenshot, "image/png");
  }
});
