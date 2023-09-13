import { MoneyOperationUtils } from '@/helpers/money-operation-utils';
import { Allocation } from '@/models/allocation';
import axios, { AxiosResponse } from 'axios';

export async function createAllocationRequest(allocation:Allocation): Promise<string> {
  return axios.post<string>('allocation', allocation).then((response: AxiosResponse<string>) => response.data);
}

export async function updateAllocationRequest(allocation: Allocation): Promise<Allocation> {
  return axios.put<Allocation>('allocation', allocation)
    .then(res => MoneyOperationUtils.parseFromResponse(res.data));
}

export async function getAllocationRequest(allocation: Allocation): Promise<Allocation> {
  return axios.get<Allocation>(`/allocation/${allocation.id}`)
    .then(res => MoneyOperationUtils.parseFromResponse(res.data));
}

export async function deleteAllocationRequest(allocation: Allocation): Promise<void> {
  return axios.delete<void>(`/allocation/${allocation.id}`).then(res => res.data);
}