import App from '@/App.vue';
import { MSAL, MsalAuthService, MsalConfiguration } from '@/auth';
import router from '@/router';
import axios from 'axios';
import { createPinia } from 'pinia';
import { createApp } from 'vue';

if (!process.env.VUE_APP_AAD_CLIENT_ID
  || !process.env.VUE_APP_AAD_TENANT
  || !process.env.VUE_APP_AAD_REDIRECT
  || !process.env.VUE_APP_API_URL) {

  throw new Error('Invalid configuration');
}

const msalConfig: MsalConfiguration = {
  clientId: process.env.VUE_APP_AAD_CLIENT_ID,
  tenantId: process.env.VUE_APP_AAD_TENANT,
  redirectUri: process.env.VUE_APP_AAD_REDIRECT,
  scopeNames: ['full'],
};
axios.defaults.baseURL = process.env.VUE_APP_API_URL;

const app = createApp(App);
app
  .use(createPinia())
  .use(router);

const service = new MsalAuthService(msalConfig, router);
service.acquireTokenSilent()
  .then(success => {
    axios.defaults.headers.common.Authorization = 'Bearer ' + service.accessToken;
    app.provide(MSAL, service);
    router.push({ path: success ? '/home' : '/login' });

    app.mount('#app');
  });