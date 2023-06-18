import { Fund } from '@/models/fund';
import axios, { AxiosResponse } from 'axios';

export async function createFundRequest(fund:Fund): Promise<string> {
  return axios.post<string>(
    'api/fund', 
    {
      name: fund.name,
      isDefault: fund.isDefault,
    }
  ).then((response: AxiosResponse<string>) => response.data);
}