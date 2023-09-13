import { Fund } from '@/models/fund';
import axios, { AxiosResponse } from 'axios';

export async function createFundRequest(fund:Fund): Promise<string> {
  return axios.post<string>(
    'fund', 
    {
      name: fund.name,
    }
  ).then((response: AxiosResponse<string>) => response.data);
}

export async function updateFundRequest(fund: Fund): Promise<Fund> {
  return axios.put<Fund>(
    'fund', 
    {
      fundId: fund.id,
      name: fund.name,
    }
  ).then(res => res.data);
}

export async function getFundRequest(fund: Fund): Promise<Fund> {
  return axios.get<Fund>(
    `/fund/${fund.id}`
  ).then(res => res.data);
}

export async function deleteFundRequest(fund: Fund): Promise<void> {
  return axios.delete<void>(
    `/fund/${fund.id}`
  ).then(res => res.data);
}