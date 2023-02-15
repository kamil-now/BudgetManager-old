import {
  AccountInfo,
  AuthenticationResult,
  PublicClientApplication,
  RedirectRequest,
} from '@azure/msal-browser';
import axios from 'axios';
import { App, Plugin } from 'vue';
import { Router } from 'vue-router';

export const MSAL = Symbol();

export type MsalConfiguration = {
  tenantId: string;
  clientId: string;
  redirectUri: string;
  scopeNames: string[];
};

export const msal: Plugin = {
  async install(app: App, options: MsalConfiguration, router: Router) {
    const service = new MsalAuthService(options, router);
    app.provide(MSAL, service);

    router.push({
      path: (await service.acquireTokenSilent()) ? '/home' : '/login',
    });

    axios.interceptors.request.use(
      (config) => {
        if (!config.headers) {
          config.headers = {};
        }
        config.headers['Authorization'] = 'Bearer ' + service.accessToken;
        config.headers['Access-Control-Allow-Origin'] = 'http://localhost:8200';
        config.headers['Content-Type'] = 'application/json';
        return config;
      },
      (error) => Promise.reject(error)
    );
  },
};

export class MsalAuthService {
  get activeAccount(): AccountInfo | null {
    return this.msal.getActiveAccount();
  }

  get isLoggedIn(): boolean {
    return this.activeAccount != null;
  }

  get accessToken(): string | undefined {
    return this._accessToken;
  }

  private readonly msal: PublicClientApplication;
  private readonly redirectRequest: RedirectRequest;

  private _accessToken: string | undefined;

  constructor(config: MsalConfiguration, router: Router) {
    this.msal = new PublicClientApplication({
      auth: {
        clientId: config.clientId,
        authority: `https://login.microsoftonline.com/${config.tenantId}`,
        knownAuthorities: [`${config.tenantId}.b2clogin.com`],
        redirectUri: config.redirectUri,
        navigateToLoginRequestUrl: false,
        postLogoutRedirectUri: config.redirectUri,
      },
      cache: {
        cacheLocation: 'localStorage',
        storeAuthStateInCookie: true,
      },
    });

    this.msal.initialize();

    router.beforeEach((to, _, next) => {
      if (!to.path.includes('/login') && !this.isLoggedIn) {
        console.warn(
          `Prevented unauthorized access to ${to.path}, redirecting to /login`
        );
      } else if (to.path.includes('/login') && this.isLoggedIn) {
        next({ path: '/home' });
      } else next();
    });
    this.redirectRequest = {
      scopes: config.scopeNames.map(
        (scope) => `api://${config.clientId}/${scope}`
      ),
    };
  }

  logout(): Promise<void> {
    return this.msal.logoutRedirect();
  }

  async login(): Promise<void> {
    if (!(await this.acquireTokenSilent())) {
      this.msal.loginRedirect(this.redirectRequest);
    }
  }

  acquireTokenSilent(): Promise<boolean> {
    return new Promise((resolve) => {
      this.msal.handleRedirectPromise()
        .then(() => {
          const accounts = this.msal.getAllAccounts();

          if (accounts.length > 0) {
            this.msal.setActiveAccount(accounts[0]);

            const request = {
              ...this.redirectRequest,
              account: accounts[0],
            };

            this.msal
              .acquireTokenSilent(request)
              .then((tokenResponse: AuthenticationResult) => {
                console.warn('Logged in as', this.activeAccount?.username);
                this._accessToken = tokenResponse.accessToken;
                resolve(true);
              })
              .catch((error) => {
                console.error(error);
                resolve(false);
              });
          } else {
            resolve(false);
          }
        });
    });
  }
}
