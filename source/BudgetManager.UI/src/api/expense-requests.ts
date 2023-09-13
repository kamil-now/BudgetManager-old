import { MoneyOperationUtils } from '@/helpers/money-operation-utils';
import { Expense } from '@/models/expense';
import axios, { AxiosResponse } from 'axios';

export async function createExpenseRequest(expense: Expense): Promise<string> {
  return axios.post<string>(
    'expense', 
    {
      title: expense.title,
      value: expense.value,
      date: expense.date,
      accountId: expense.accountId,
      fundId: expense.fundId,
      description: expense.description
    }
  ).then((response: AxiosResponse<string>) => response.data);
}

export async function updateExpenseRequest(expense: Expense): Promise<Expense> {
  return axios.put<Expense>(
    'expense', 
    {
      operationId: expense.id,
      title: expense.title,
      value: expense.value,
      date: expense.date,
      accountId: expense.accountId,
      fundId: expense.fundId,
      description: expense.description
    }
  ).then(res => MoneyOperationUtils.parseFromResponse(res.data));
}

export async function getExpenseRequest(expense: Expense): Promise<Expense> {
  return axios.get<Expense>(
    `/expense/${expense.id}`
  ).then(res => MoneyOperationUtils.parseFromResponse(res.data));
}

export async function deleteExpenseRequest(expense: Expense): Promise<void> {
  return axios.delete<void>(
    `/expense/${expense.id}`
  ).then(res => res.data);
}