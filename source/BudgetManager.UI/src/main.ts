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
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Calendar from 'primevue/calendar';
import TabView from 'primevue/tabview';
import TabPanel from 'primevue/tabpanel';
import Toast from 'primevue/toast';
import ToastService from 'primevue/toastservice';
import DialogService from 'primevue/dialogservice';
import DynamicDialog from 'primevue/dynamicdialog';
import ProgressSpinner from 'primevue/progressspinner';
// PrimeVue
import App from '@/App.vue';
import { AUTH, IAuthService, MsalAuthService, MsalConfiguration } from '@/auth';
import router from '@/router';
import axios from 'axios';
import { createPinia } from 'pinia';
import { createApp } from 'vue';
import { useAppStore } from './store/store';
// eslint-disable-next-line @typescript-eslint/no-unused-vars
import Colada, { PiniaColadaPlugin } from 'colada-plugin';

if (
  !process.env.VUE_APP_AAD_REDIRECT
  || !process.env.VUE_APP_API_URL
  || !process.env.VUE_APP_API_URL) {

  throw new Error('Invalid configuration');
}

axios.defaults.headers.common['Content-Type'] = 'application/json';
axios.defaults.headers.common['Access-Control-Allow-Origin'] = process.env.VUE_APP_AAD_REDIRECT;
axios.defaults.baseURL = process.env.VUE_APP_API_URL;

const app = createApp(App);
const pinia = createPinia();

app
  .use(PrimeVue)
  .use(ToastService)
  .use(ConfirmationService)
  .use(DialogService)
  .use(pinia)
  .use(router);
// pinia.use(PiniaColadaPlugin);
// app.use(Colada);

app
  .component('TabView', TabView)
  .component('TabPanel', TabPanel)
  .component('Calendar', Calendar)
  .component('DataTable', DataTable)
  .component('Column', Column)
  .component('ProgressSpinner', ProgressSpinner)
  .component('ConfirmPopup', ConfirmPopup)
  .component('Dropdown', Dropdown)
  .component('Button', Button)
  .component('InputText', InputText)
  .component('InputNumber', InputNumber)
  .component('Toast', Toast)
  .component('DynamicDialog', DynamicDialog);
  

const appStore = useAppStore();
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
  service.initialize().then(() => {
    service.acquireTokenSilent()
      .then(success => {
        axios.defaults.headers.common.Authorization = 'Bearer ' + service.accessToken;
        app.provide(AUTH, service);
        if (success) {
          appStore.setLoggedIn(true);
          router.push({ path: success ? '/home' : '/login' });
        }

        app.mount('#app');
      });
  });
} else {
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
  // router.push({ path: '/home' });
  app.mount('#app');
}


