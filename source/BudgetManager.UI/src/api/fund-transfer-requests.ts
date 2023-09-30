import { MoneyOperationUtils } from '@/helpers/money-operation-utils';
import { FundTransfer } from '@/models/fund-transfer';
import axios, { AxiosResponse } from 'axios';

export async function createFundTransferRequest(fundTransfer: FundTransfer): Promise<string> {
  return axios.post<string>('fund-transfer', fundTransfer).then((response: AxiosResponse<string>) => response.data);
}

export async function updateFundTransferRequest(fundTransfer: FundTransfer): Promise<FundTransfer> {
  return axios.put<FundTransfer>('fund-transfer', fundTransfer)
    .then(res => MoneyOperationUtils.parseFromResponse(res.data));
}

export async function getFundTransferRequest(fundTransfer: FundTransfer): Promise<FundTransfer> {
  return axios.get<FundTransfer>(`/fund-transfer/${fundTransfer.id}`)
    .then(res => MoneyOperationUtils.parseFromResponse(res.data));
}

export async function deleteFundTransferRequest(fundTransferId: string): Promise<void> {
  return axios.delete<void>(`/fund-transfer/${fundTransferId}`).then(res => res.data);
}