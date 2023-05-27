/* eslint-disable vue/multi-word-component-names */
import App from '@/App.vue';
import { AUTH, IAuthService, MsalAuthService, MsalConfiguration } from '@/auth';
import router from '@/router';
import axios from 'axios';
import { createPinia } from 'pinia';
import { createApp } from 'vue';
// PrimeVue
import 'primeicons/primeicons.css';
import Button from 'primevue/button';
import PrimeVue from 'primevue/config';
import ConfirmationService from 'primevue/confirmationservice';
import ConfirmPopup from 'primevue/confirmpopup';
import Dropdown from 'primevue/dropdown';
import InputNumber from 'primevue/inputnumber';
import InputText from 'primevue/inputtext';
import 'primevue/resources/primevue.min.css';
import 'primevue/resources/themes/lara-light-indigo/theme.css';
// PrimeVue

if (
  !process.env.VUE_APP_AAD_REDIRECT
  || !process.env.VUE_APP_API_URL) {

  throw new Error('Invalid configuration');
}

axios.defaults.headers.common['Content-Type'] = 'application/json';
axios.defaults.headers.common['Access-Control-Allow-Origin'] = process.env.VUE_APP_AAD_REDIRECT;

const app = createApp(App);
app
  .use(PrimeVue)
  .use(ConfirmationService)
  .use(createPinia())
  .use(router);


app
  .component('ConfirmPopup', ConfirmPopup)
  .component('Dropdown', Dropdown)
  .component('Button', Button)
  .component('InputText', InputText)
  .component('InputNumber', InputNumber);

if (process.env.VUE_APP_ENV === 'production') {
  if (!process.env.VUE_APP_AAD_CLIENT_ID
    || !process.env.VUE_APP_AAD_TENANT) {
    throw new Error('Invalid AAD configuration');
  }
  const msalConfig: MsalConfiguration = {
    clientId: process.env.VUE_APP_AAD_CLIENT_ID,
    tenantId: process.env.VUE_APP_AAD_TENANT,
    redirectUri: process.env.VUE_APP_AAD_REDIRECT,
    scopeNames: ['full'],
  };

  const service = new MsalAuthService(msalConfig, router);
  service.acquireTokenSilent()
    .then(success => {
      axios.defaults.headers.common.Authorization = 'Bearer ' + service.accessToken;
      app.provide(AUTH, service);
      router.push({ path: success ? '/home' : '/login' });

      app.mount('#app');
    });
} else {
  const mockAuthService: IAuthService = {
    get isLoggedIn(): boolean {
      return true;
    },
    login(): Promise<void> {
      this.isLoggedIn = true;
      return Promise.resolve();
    },
    logout(): Promise<void> {
      this.isLoggedIn = true;
      return Promise.resolve();
    }
  };
  app.provide(AUTH, mockAuthService);
  router.push({ path: '/home' });
  app.mount('#app');
}


