import { Allocation } from '@/models/allocation';
import axios, { AxiosResponse } from 'axios';

export async function createAllocationRequest(allocation:Allocation): Promise<string> {
  return axios.post<string>('api/allocation', allocation).then((response: AxiosResponse<string>) => response.data);
}

export async function updateAllocationRequest(allocation: Allocation): Promise<Allocation> {
  return axios.put<Allocation>('api/allocation', allocation).then(res => res.data);
}

export async function getAllocationRequest(allocation: Allocation): Promise<Allocation> {
  return axios.get<Allocation>(`api/allocation/${allocation.id}`).then(res => res.data);
}

export async function deleteAllocationRequest(allocation: Allocation): Promise<void> {
  return axios.delete<void>(`api/allocation/${allocation.id}`).then(res => res.data);
}