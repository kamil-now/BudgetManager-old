
import { Account } from '@/models/account';
import { Balance } from '@/models/balance';
import { Fund } from '@/models/fund';
import axios, { AxiosResponse } from 'axios';

export async function createBudgetRequest(
  accounts: Account[],
  funds: Fund[]
): Promise<Balance> {
  // order is important
  return axios.post<void>('api/budget')
    .then(() =>
      funds.length > 0
        ? funds.map(fund =>
          axios.post<string>('api/fund', {
            name: fund.name,
            isDefault: fund.isDefault,
          }).then((response: AxiosResponse<string>) => {
            fund.id = response.data;
            return;
          }))
        : [Promise.resolve()])
    .then(() =>
      accounts.length > 0
        ? accounts
          .map(account =>
            axios.post<string>('api/account', {
              name: account.name,
              initialAmount: account.balance.amount,
              currency: account.balance.currency
            }).then((response: AxiosResponse<string>) => {
              account.id = response.data;
              return;
            }))
        : [Promise.resolve()])
    .then(() => axios.get<Balance>('/api/balance'))
    .then(res => res.data);
}