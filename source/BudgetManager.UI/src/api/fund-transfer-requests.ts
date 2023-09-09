import { MoneyOperationUtils } from '@/helpers/money-operation-utils';
import { FundTransfer } from '@/models/fund-transfer';
import axios, { AxiosResponse } from 'axios';

export async function createFundTransferRequest(fundTransfer: FundTransfer): Promise<string> {
  return axios.post<string>('api/fund-transfer', fundTransfer).then((response: AxiosResponse<string>) => response.data);
}

export async function updateFundTransferRequest(fundTransfer: FundTransfer): Promise<FundTransfer> {
  return axios.put<FundTransfer>('api/fund-transfer', fundTransfer)
    .then(res => MoneyOperationUtils.parseFromResponse(res.data));
}

export async function getFundTransferRequest(fundTransfer: FundTransfer): Promise<FundTransfer> {
  return axios.get<FundTransfer>(`api/fund-transfer/${fundTransfer.id}`)
    .then(res => MoneyOperationUtils.parseFromResponse(res.data));
}

export async function deleteFundTransferRequest(fundTransfer: FundTransfer): Promise<void> {
  return axios.delete<void>(`api/fund-transfer/${fundTransfer.id}`).then(res => res.data);
}