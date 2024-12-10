import { createIncomeRequest, deleteIncomeRequest, getIncomeRequest, updateIncomeRequest } from '@/api/income-requests';
import { Income } from '@/models/income';
import { AppStore } from '../store';
import { StoreUtils } from '../store-utils';
import { IncomeAllocation } from '@/models/income-allocation';

export interface IIncomeActions { 
  createNewIncome(income: Income, incomeAllocation?: IncomeAllocation): void;
  updateIncome(income: Income): void;
  deleteIncome(incomeId: string): void;
}

export class IncomeActions {
  static async createNewIncome(store: AppStore, income: Income) {
    await StoreUtils.runAsyncOperation(store, async () => {
      const fromResponse = await StoreUtils.createOperation(
        store,
        () => createIncomeRequest(income),
        id => getIncomeRequest(id)
      );
      await this.reload(store, fromResponse);
    });
  }

  static async updateIncome(store: AppStore, income: Income) {
    await StoreUtils.runAsyncOperation(store, async () => {
      const fromResponse = await StoreUtils.updateOperation(store, () => updateIncomeRequest(income));
      await this.reload(store, fromResponse);
    });
  }

  static async deleteIncome(store: AppStore, incomeId: string) {
    await StoreUtils.runAsyncOperation(store, async () => {
      await deleteIncomeRequest(incomeId);
      await this.reload(store, StoreUtils.getFromCollection(store.incomes, incomeId));
      store.budget.operations = store.budget.operations.filter(x => x.id !== incomeId);
    });
  }

  private static async reload(store: AppStore, income: Income) {
    await StoreUtils.reloadAccount(store, income.accountId);
    await StoreUtils.reloadBalance(store);
  }
}
