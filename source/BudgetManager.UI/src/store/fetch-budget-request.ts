
import { Account } from '@/models/account';
import { Balance } from '@/models/balance';
import { Budget } from '@/models/budget';
import { Fund } from '@/models/fund';
import axios, { AxiosError } from 'axios';

export function fetchBudget(): Promise<Budget | null> {
  return axios.get<Balance>('/api/balance')
    .then(
      async response => {
        const accounts = await axios.get<Account[]>('api/accounts').then(res => res.data);
        const funds = await axios.get<Fund[]>('api/funds').then(res => res.data);
        return {
          defaultFundName: funds.find(x => x.isDefault)?.name ?? '', // TODO
          defaultCurrency: accounts[0].balance.currency, // TODO
          accounts,
          funds,
          balance: response.data
        };
      },
      (error: AxiosError<string[]>) => {
        if (error.response?.data.some(x => x.includes('does not exist'))) {
          return null;
        } else {
          // TODO
          return null;
        }
      }
    );
}
