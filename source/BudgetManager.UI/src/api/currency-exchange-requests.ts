import { MoneyOperationUtils } from '@/helpers/money-operation-utils';
import { CurrencyExchange } from '@/models/currency-exchange';
import axios, { AxiosResponse } from 'axios';

export async function createCurrencyExchangeRequest(currencyExchange: CurrencyExchange): Promise<string> {
  return axios.post<string>(
    'currency-exchange', 
    {
      title: currencyExchange.title,
      value: currencyExchange.value,
      date: currencyExchange.date,
      accountId: currencyExchange.accountId,
      targetCurrency: currencyExchange.targetCurrency,
      exchangeRate: currencyExchange.exchangeRate,
      description: currencyExchange.description
    }
  ).then((response: AxiosResponse<string>) => response.data);
}

export async function updateCurrencyExchangeRequest(currencyExchange: CurrencyExchange): Promise<CurrencyExchange> {
  return axios.put<CurrencyExchange>(
    'currency-exchange', 
    {
      operationId: currencyExchange.id,
      title: currencyExchange.title,
      value: currencyExchange.value,
      date: currencyExchange.date,
      accountId: currencyExchange.accountId,
      targetCurrency: currencyExchange.targetCurrency,
      exchangeRate: currencyExchange.exchangeRate,
      description: currencyExchange.description
    }
  ).then(res => MoneyOperationUtils.parseFromResponse(res.data));
}

export async function getCurrencyExchangeRequest(currencyExchange: CurrencyExchange): Promise<CurrencyExchange> {
  return axios.get<CurrencyExchange>(
    `/currency-exchange/${currencyExchange.id}`
  ).then(res => MoneyOperationUtils.parseFromResponse(res.data));
}

export async function deleteCurrencyExchangeRequest(currencyExchangeId: string): Promise<void> {
  return axios.delete<void>(
    `/currency-exchange/${currencyExchangeId}`
  ).then(res => res.data);
}