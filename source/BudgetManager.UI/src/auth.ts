import {
  AccountInfo,
  AuthenticationResult,
  PublicClientApplication,
  RedirectRequest
} from '@azure/msal-browser';
import { Router } from 'vue-router';
import { AppStore, useAppStore } from './store/store';

export const AUTH = Symbol();

export interface IAuthService {
  logout(): Promise<void>;
  login(): Promise<void>;
}

export type MsalConfiguration = {
  tenantId: string;
  clientId: string;
  redirectUri: string;
  scopeNames: string[];
};

export class MsalAuthService implements IAuthService {
  get activeAccount(): AccountInfo | null {
    return this.msal.getActiveAccount();
  }

  get accessToken(): string | undefined {
    return this._accessToken;
  }

  private readonly msal: PublicClientApplication;
  private readonly redirectRequest: RedirectRequest;
  private readonly appStore: AppStore;

  private _accessToken: string | undefined;

  constructor(config: MsalConfiguration, router: Router) {
    this.appStore = useAppStore();
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
      if (!to.path.includes('/login') && !this.appStore.isLoggedIn) {
        console.warn(
          `Prevented unauthorized access to ${to.path}, redirecting to /login`
        );
      } else if (to.path.includes('/login') && this.appStore.isLoggedIn) {
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
    const loggedIn = await this.acquireTokenSilent()
      .then(loggedIn => {
        this.appStore.setLoggedIn(loggedIn);
        return loggedIn;
      });
    if (!loggedIn) {
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
