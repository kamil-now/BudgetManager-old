import App from "@/App.vue";
import { msal, MsalConfiguration } from "@/plugins/msal-plugin";
import router from "@/router";
import axios from "axios";
import { createPinia } from "pinia";
import { createApp } from "vue";

const msalConfig: MsalConfiguration = {
  clientId: process.env.VUE_APP_AAD_CLIENT_ID,
  tenantId: process.env.VUE_APP_AAD_TENANT,
  redirectUri: process.env.VUE_APP_AAD_REDIRECT,
  scopeNames: ["full"],
};
axios.defaults.baseURL = process.env.VUE_APP_API_URL;

const app = createApp(App);
app.use(msal, msalConfig, router)
  .use(createPinia())
  .use(router)
  .mount("#app");
