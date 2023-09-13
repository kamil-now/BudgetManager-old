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

export async function getAccountRequest(account: Account): Promise<Account> {
  return axios.get<Account>(
    `/account/${account.id}`
  ).then(res => res.data);
}

export async function deleteAccountRequest(account: Account): Promise<void> {
  return axios.delete<void>(
    `/account/${account.id}`
  ).then(res => res.data);
}