import { MoneyOperationUtils } from '@/helpers/money-operation-utils';
import { AccountTransfer } from '@/models/account-transfer';
import axios, { AxiosResponse } from 'axios';

export async function createAccountTransferRequest(accountTransfer: AccountTransfer): Promise<string> {
  return axios.post<string>('account-transfer', accountTransfer).then((response: AxiosResponse<string>) => response.data);
}

export async function updateAccountTransferRequest(accountTransfer: AccountTransfer): Promise<AccountTransfer> {
  return axios.put<AccountTransfer>('account-transfer', accountTransfer)
    .then(res => MoneyOperationUtils.parseFromResponse(res.data));
}

export async function getAccountTransferRequest(accountTransfer: AccountTransfer): Promise<AccountTransfer> {
  return axios.get<AccountTransfer>(`/account-transfer/${accountTransfer.id}`)
    .then(res => MoneyOperationUtils.parseFromResponse(res.data));
}

export async function deleteAccountTransferRequest(accountTransferId: string): Promise<void> {
  return axios.delete<void>(`/account-transfer/${accountTransferId}`).then(res => res.data);
}