
import { Account } from '@/models/account';
import { Fund } from '@/models/fund';
import axios, { AxiosResponse } from 'axios';

export async function createBudget(
  defaultFundName: string,
  accounts: Account[],
  funds: Fund[]
): Promise<void> {
  return axios.post<void>('api/budget', { defaultFundName })
    .then(async () => {
      const createAccounts = accounts.length > 0
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
        : [Promise.resolve()];

      const createFunds = funds.length > 0
        ? funds.map(fund =>
          axios.post<string>('api/fund', {
            name: fund.name,
          }).then((response: AxiosResponse<string>) => {
            fund.id = response.data;
            return;
          }))
        : [Promise.resolve()];

      await Promise.all([...createAccounts, ...createFunds]);
    });
}