import App from '@/App.vue';
import { AUTH, IAuthService, MsalAuthService, MsalConfiguration } from '@/auth';
import router from '@/router';
import axios from 'axios';
import { createPinia } from 'pinia';
import { createApp } from 'vue';

if (
  !process.env.VUE_APP_AAD_REDIRECT
  || !process.env.VUE_APP_API_URL) {

  throw new Error('Invalid configuration');
}

axios.defaults.headers.common['Content-Type'] = 'application/json';
axios.defaults.headers.common['Access-Control-Allow-Origin'] = process.env.VUE_APP_AAD_REDIRECT;

const app = createApp(App);
app
  .use(createPinia())
  .use(router);

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


