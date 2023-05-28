/* eslint-disable vue/multi-word-component-names */
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
import SpeedDial from 'primevue/speeddial';
import Tag from 'primevue/tag';
import Card from 'primevue/card';
import ProgressSpinner from 'primevue/progressspinner';
import Fieldset from 'primevue/fieldset';
import Panel from 'primevue/panel';
import Accordion from 'primevue/accordion';
import AccordionTab from 'primevue/accordiontab';
import Divider from 'primevue/divider';
// PrimeVue
import App from '@/App.vue';
import { AUTH, IAuthService, MsalAuthService, MsalConfiguration } from '@/auth';
import router from '@/router';
import axios from 'axios';
import { createPinia } from 'pinia';
import { createApp } from 'vue';
import { useAppStore } from './store/store';

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
  .component('Divider', Divider)
  .component('Accordion', Accordion)
  .component('AccordionTab', AccordionTab)
  .component('Panel', Panel)
  .component('Fieldset', Fieldset)
  .component('ProgressSpinner', ProgressSpinner)
  .component('Card', Card)
  .component('Tag', Tag)
  .component('SpeedDial', SpeedDial)
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
  const appStore = useAppStore();
  appStore.setLoggedIn(['true', null].includes(window.localStorage.getItem('isLoggedIn')));
  const mockAuthService: IAuthService = {
    login(): Promise<void> {
      window.localStorage.setItem('isLoggedIn', 'true');
      appStore.setLoggedIn(true);
      return Promise.resolve();
    },
    logout(): Promise<void> {
      window.localStorage.setItem('isLoggedIn', 'false');
      appStore.setLoggedIn(false);
      return Promise.resolve();
    },
  };
  app.provide(AUTH, mockAuthService);
  router.push({ path: '/home' });
  app.mount('#app');
}


