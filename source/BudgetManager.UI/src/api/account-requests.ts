import { Account } from '@/models/account';
import axios, { AxiosResponse } from 'axios';

export async function createAccountRequest(account: Account): Promise<string> {
  return axios.post<string>(
    'api/account', 
    {
      name: account.name,
      initialAmount: account.balance.amount,
      currency: account.balance.currency,
    }
  ).then((response: AxiosResponse<string>) => response.data);
}

export async function updateAccountRequest(account: Account): Promise<Account> {
  return axios.put<Account>(
    'api/account', 
    {
      accountId: account.id,
      name: account.name,
      initialBalance: account.initialBalance
    }
  ).then(res => res.data);
}

export async function getAccountRequest(account: Account): Promise<Account> {
  return axios.get<Account>(
    `api/account/${account.id}`
  ).then(res => res.data);
}

export async function deleteAccountRequest(account: Account): Promise<void> {
  return axios.delete<void>(
    `api/account/${account.id}`
  ).then(res => res.data);
}