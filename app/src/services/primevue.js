export let toast;

const setupPrimeVueServices = (app) => {
  toast = app.config.globalProperties.$toast;
};

export default setupPrimeVueServices;
