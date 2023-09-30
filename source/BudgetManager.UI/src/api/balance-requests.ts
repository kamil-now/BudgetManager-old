import { BudgetBalance } from '@/models/budget-balance';
import axios, { AxiosResponse } from 'axios';

export function getBalanceRequest(): Promise<BudgetBalance> {
  return axios.get<BudgetBalance>('/balance')
    .then((response: AxiosResponse<BudgetBalance>) => response.data);
}