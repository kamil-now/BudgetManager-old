import { Income } from '@/models/income';
import axios, { AxiosResponse } from 'axios';

export async function createIncomeRequest(income: Income): Promise<string> {
  return axios.post<string>(
    'api/income', 
    {
      title: income.title,
      value: income.value,
      date: income.date,
      accountId: income.accountId,
      fundId: income.fundId,
      description: income.description
    }
  ).then((response: AxiosResponse<string>) => response.data);
}

export async function updateIncomeRequest(income: Income): Promise<Income> {
  return axios.put<Income>(
    'api/income', 
    {
      operationId: income.id,
      title: income.title,
      value: income.value,
      date: income.date,
      accountId: income.accountId,
      fundId: income.fundId,
      description: income.description
    }
  ).then(res => res.data);
}

export async function getIncomeRequest(income: Income): Promise<Income> {
  return axios.get<Income>(
    `api/income/${income.id}`
  ).then(res => res.data);
}

export async function deleteIncomeRequest(income: Income): Promise<void> {
  return axios.delete<void>(
    `api/income/${income.id}`
  ).then(res => res.data);
}