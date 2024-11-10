import { IncomeAllocation } from '@/models/income-allocation';
import axios, { AxiosResponse } from 'axios';

export async function createIncomeAllocationTemplateRequest(incomeAllocation: IncomeAllocation): Promise<string> {
  return axios.post<string>(
    'income-allocation-template', 
    {
      name: incomeAllocation.name,
      defaultFundId: incomeAllocation.defaultFundId,
      rules: incomeAllocation.rules
    }
  ).then((response: AxiosResponse<string>) => response.data);
}

export async function updateIncomeAllocationTemplateRequest(incomeAllocation: IncomeAllocation): Promise<IncomeAllocation> {
  return axios.put<IncomeAllocation>(
    'income-allocation-template', 
    {
      name: incomeAllocation.name,
      defaultFundId: incomeAllocation.defaultFundId,
      rules: incomeAllocation.rules
    }
  ).then(res => res.data);
}

export async function getIncomeAllocationTemplateRequest(incomeAllocationTemplateId: string): Promise<IncomeAllocation> {
  return axios.get<IncomeAllocation>(`/income-allocation-template/${incomeAllocationTemplateId}`).then(res => res.data);
}

export async function getIncomeAllocationTemplatesRequest(): Promise<IncomeAllocation[]> {
  return axios.get<IncomeAllocation[]>('/income-allocation-templates').then(res => res.data);
}

export async function deleteIncomeAllocationTemplateRequest(incomeAllocationTemplateId: string): Promise<void> {
  return axios.delete<void>(`/income-allocation-template/${incomeAllocationTemplateId}`).then(res => res.data);
}