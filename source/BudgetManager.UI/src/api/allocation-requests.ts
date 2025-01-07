import { MoneyOperationUtils } from '@/helpers/money-operation-utils';
import { Allocation } from '@/models/allocation';
import axios, { AxiosResponse } from 'axios';

export async function createAllocationRequest(allocation: Allocation): Promise<string> {
  return axios.post<string>('allocation', allocation).then((response: AxiosResponse<string>) => response.data);
}

export async function createManyAllocationsequest(allocations: Allocation[]): Promise<string> {
  return axios.post<string>('allocations', { allocations }).then((response: AxiosResponse<string>) => response.data);
}

export async function updateAllocationRequest(allocation: Allocation): Promise<Allocation> {
  return axios.put<Allocation>('allocation', allocation)
    .then(res => MoneyOperationUtils.parseFromResponse(res.data));
}

export async function getAllocationRequest(allocationId: string): Promise<Allocation> {
  return axios.get<Allocation>(`/allocation/${allocationId}`)
    .then(res => MoneyOperationUtils.parseFromResponse(res.data));
}

export async function deleteAllocationRequest(allocationId: string): Promise<void> {
  return axios.delete<void>(`/allocation/${allocationId}`).then(res => res.data);
}