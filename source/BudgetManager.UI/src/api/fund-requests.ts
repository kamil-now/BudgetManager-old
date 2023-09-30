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

export async function getFundRequest(fundId: string): Promise<Fund> {
  return axios.get<Fund>(
    `/fund/${fundId}`
  ).then(res => res.data);
}

export async function deleteFundRequest(fundId: string): Promise<void> {
  return axios.delete<void>(
    `/fund/${fundId}`
  ).then(res => res.data);
}