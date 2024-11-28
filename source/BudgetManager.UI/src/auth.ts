import {
  AccountInfo,
  PublicClientApplication,
  RedirectRequest
} from '@azure/msal-browser';
import axios from 'axios';
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
    const msalAcc =  this.msal.getActiveAccount() ;
    if (msalAcc) {
      return msalAcc;
    }
    
    const acc = localStorage.getItem('acc');
    return acc ? JSON.parse(acc) : null;
  }

  private readonly msal: PublicClientApplication;
  private redirectRequest?: RedirectRequest;
  private config: MsalConfiguration;
  private readonly appStore: AppStore;

  constructor(config: MsalConfiguration) {
    this.appStore = useAppStore();
    this.config = config;
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
        storeAuthStateInCookie: false,
      },
    });
    
    this.redirectRequest = {
      scopes: this.config.scopeNames.map(
        (scope) => `api://${this.config.clientId}/${scope}`
      ),
    };
  }

  async initialize(): Promise<void> {
    await this.msal.initialize();
    this.initializeInterceptors();
    const response = await this.msal.handleRedirectPromise();
  
    if (response) {
      this.msal.setActiveAccount(response.account);
      console.warn('Logged in as', this.activeAccount?.username);
      localStorage.setItem('acc', JSON.stringify(response.account));
    } else {
      if (this.activeAccount) {
        this.msal.setActiveAccount(this.activeAccount);
        console.warn('Already logged in as', this.activeAccount?.username);
      } else {
        console.warn('No active account found, redirecting to login.');
        await this.msal.loginRedirect(this.redirectRequest);
      }
    }
  }

  logout(): Promise<void> {
    return this.msal.logoutRedirect();
  }

  async login(): Promise<void> {
    const accessToken = await this.acquireTokenSilent();
    if (!accessToken) {
      this.msal.loginRedirect(this.redirectRequest);
    } else {
      this.appStore.setLoggedIn(true);
    }
  }

  private async acquireTokenSilent(): Promise<string | null> {
    if (this.activeAccount) {
      const request = {
        ...this.redirectRequest!,
        account: this.activeAccount
      };
      try {
        const tokenResponse = await this.msal.acquireTokenSilent(request);
        return tokenResponse.accessToken;
      } catch (error) {
        console.error(error);
        return null;
      }
    } else {
      return null;
    }
  }

  private initializeInterceptors(): void {
    axios.interceptors.request.clear();
    axios.interceptors.request.use(
      async (config) => {
        try {
          const accessToken = await this.acquireTokenSilent();
          if (accessToken) {
            config.headers['Authorization'] = `Bearer ${accessToken}`;
          } else {
            await this.msal.loginRedirect(this.redirectRequest);
          }
        } catch {
          await this.msal.loginRedirect(this.redirectRequest);
        }
        return config;
      },
      (error) => Promise.reject(error)
    );   
  }
}
