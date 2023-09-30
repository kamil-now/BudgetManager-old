import { Account } from '@/models/account';
import axios, { AxiosResponse } from 'axios';

export async function createAccountRequest(account: Account): Promise<string> {
  return axios.post<string>(
    'account', 
    {
      name: account.name,
      initialBalance: account.initialBalance,
    }
  ).then((response: AxiosResponse<string>) => response.data);
}

export async function updateAccountRequest(account: Account): Promise<Account> {
  return axios.put<Account>(
    'account', 
    {
      accountId: account.id,
      name: account.name,
      initialBalance: account.initialBalance
    }
  ).then(res => res.data);
}

export async function getAccountRequest(accountId: string): Promise<Account> {
  return axios.get<Account>(
    `/account/${accountId}`
  ).then(res => res.data);
}

export async function deleteAccountRequest(accountId: string): Promise<void> {
  return axios.delete<void>(
    `/account/${accountId}`
  ).then(res => res.data);
}