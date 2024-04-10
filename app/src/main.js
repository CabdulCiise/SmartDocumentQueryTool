import { createApp } from 'vue';

import 'primevue/resources/primevue.min.css';
import 'primevue/resources/themes/bootstrap4-dark-blue/theme.css';
import 'primeicons/primeicons.css';

import PrimeVue from 'primevue/config';
import Tooltip from 'primevue/tooltip';

import ToastService from 'primevue/toastservice';

import moment from 'moment';
import axios from 'axios';
import setupPrimeVueServices from './services/primevue';
import router from './router';

import App from './App.vue';

axios.defaults.baseURL = 'https://localhost:44322/';
axios.defaults.withCredentials = true;
axios.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

const app = createApp(App);

app.use(router);
app.use(PrimeVue);
app.use(ToastService);
app.directive('tooltip', Tooltip);

setupPrimeVueServices(app);

app.mount('#app');

app.config.globalProperties.$filters = {
  date(date) {
    return date != null ? moment(date).format('MM-DD-YYYY HH:mm') : null;
  },
  currency(value) {
    return `$${value}`;
  },
};
