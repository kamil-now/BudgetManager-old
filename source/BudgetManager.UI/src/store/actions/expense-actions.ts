import { createExpenseRequest, deleteExpenseRequest, getExpenseRequest, updateExpenseRequest } from '@/api/expense-requests';
import { Expense } from '@/models/expense';
import { AppStore } from '../store';
import { StoreUtils } from '../store-utils';

export interface IExpenseActions { 
  createNewExpense(expense: Expense): void,
  updateExpense(expense: Expense): void;
  deleteExpense(expenseId: string): void;
}

export class ExpenseActions {
  static async createNewExpense(store: AppStore, expense: Expense) {
    await StoreUtils.runAsyncOperation(store, async () => {
      const fromResponse = await StoreUtils.createOperation(
        store,
        () => createExpenseRequest(expense),
        id => getExpenseRequest(id)
      );
      await this.reload(store, fromResponse);
    });
  }

  static async updateExpense(store: AppStore, expense: Expense) {
    await StoreUtils.runAsyncOperation(store, async () => {
      const fromResponse = await StoreUtils.updateOperation(store, () => updateExpenseRequest(expense));
      await this.reload(store, fromResponse);
    });
  }

  static async deleteExpense(store: AppStore, expenseId: string) {
    await StoreUtils.runAsyncOperation(store, async () => {
      await deleteExpenseRequest(expenseId);
      await this.reload(store, StoreUtils.getFromCollection(store.expenses, expenseId));
      store.budget.operations = store.budget.operations.filter(x => x.id !== expenseId);
    });
  }

  private static async reload(store: AppStore, expense: Expense) {
    await StoreUtils.reloadAccount(store, expense.accountId);
    await StoreUtils.reloadFund(store, expense.fundId);
    await StoreUtils.reloadBalance(store);
  }
}
