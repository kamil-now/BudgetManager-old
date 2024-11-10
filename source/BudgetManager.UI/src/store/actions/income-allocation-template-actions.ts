import { createIncomeAllocationTemplateRequest, deleteIncomeAllocationTemplateRequest, getIncomeAllocationTemplateRequest, updateIncomeAllocationTemplateRequest } from '@/api/income-allocation-template-requests';
import { IncomeAllocation } from '@/models/income-allocation';
import { AppStore } from '../store';
import { StoreUtils } from '../store-utils';

export interface IIncomeAllocationTemplateActions { 
  createNewIncomeAllocationTemplate(incomeAllocation: IncomeAllocation): void,
  updateIncomeAllocationTemplate(incomeAllocation: IncomeAllocation): void;
  deleteIncomeAllocationTemplate(incomeAllocationTemplateId: string): void;
}

export class IncomeAllocationTemplateActions {
  static async createNewIncomeAllocationTemplate(store: AppStore, incomeAllocation: IncomeAllocation) {
    await StoreUtils.runAsyncOperation(store, async () => {
      const id = await createIncomeAllocationTemplateRequest(incomeAllocation);
      const fromResponse = await getIncomeAllocationTemplateRequest(id);
      store.budget.incomeAllocationTemplates.unshift(fromResponse);
      store.updateUserSettings();
    });
  }

  static async updateIncomeAllocationTemplate(store: AppStore, incomeAllocation: IncomeAllocation) {
    await StoreUtils.runAsyncOperation(store, async () => {
      const fromResponse = await updateIncomeAllocationTemplateRequest(incomeAllocation);
      StoreUtils.replaceInCollection(store.budget.incomeAllocationTemplates, fromResponse);
    });
  }

  static async deleteIncomeAllocationTemplate(store: AppStore, incomeAllocationTemplateId: string) {
    await StoreUtils.runAsyncOperation(store, async () => {
      await deleteIncomeAllocationTemplateRequest(incomeAllocationTemplateId);
      store.budget.incomeAllocationTemplates = store.budget.incomeAllocationTemplates.filter(x => x.id !== incomeAllocationTemplateId);
    });
  }
}
