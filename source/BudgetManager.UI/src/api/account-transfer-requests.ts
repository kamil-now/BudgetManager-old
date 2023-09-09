import { MoneyOperationUtils } from '@/helpers/money-operation-utils';
import { AccountTransfer } from '@/models/account-transfer';
import axios, { AxiosResponse } from 'axios';

export async function createAccountTransferRequest(accountTransfer: AccountTransfer): Promise<string> {
  return axios.post<string>('api/account-transfer', accountTransfer).then((response: AxiosResponse<string>) => response.data);
}

export async function updateAccountTransferRequest(accountTransfer: AccountTransfer): Promise<AccountTransfer> {
  return axios.put<AccountTransfer>('api/account-transfer', accountTransfer)
    .then(res => MoneyOperationUtils.parseFromResponse(res.data));
}

export async function getAccountTransferRequest(accountTransfer: AccountTransfer): Promise<AccountTransfer> {
  return axios.get<AccountTransfer>(`api/account-transfer/${accountTransfer.id}`)
    .then(res => MoneyOperationUtils.parseFromResponse(res.data));
}

export async function deleteAccountTransferRequest(accountTransfer: AccountTransfer): Promise<void> {
  return axios.delete<void>(`api/account-transfer/${accountTransfer.id}`).then(res => res.data);
}