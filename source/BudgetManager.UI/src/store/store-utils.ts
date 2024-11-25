import { getAccountRequest } from '@/api/account-requests';
import { getBalanceRequest } from '@/api/balance-requests';
import { getFundRequest } from '@/api/fund-requests';
import { MoneyOperationUtils } from '@/helpers/money-operation-utils';
import { MoneyOperation } from '@/models/money-operation';
import { AppState } from './state';

export class StoreUtils {
  static async runAsyncOperation(
    state: AppState,
    op: () => Promise<unknown>
  ): Promise<void> {
    state.isLoading = true;
    try {
      await op();
    } catch (error) {
      // TODO console.error(error);
    } finally {
      state.isLoading = false;
    }
  }

  static async createOperation<T extends MoneyOperation>(
    state: AppState, 
    create: () => Promise<string>,
    get: (id: string) => Promise<T>
  ) {
    const fromResponse = await create().then(id => get(id));
    state.budget.operations.unshift(fromResponse);
    MoneyOperationUtils.sort(state.budget.operations);
    return fromResponse;
  }

  static async updateOperation<T extends MoneyOperation>(
    state: AppState, 
    update: () => Promise<T>
  ) {
    const fromResponse = await update();
    StoreUtils.replaceInCollection(state.budget.operations, fromResponse);
    MoneyOperationUtils.sort(state.budget.operations);
    return fromResponse;
  }

  static getFromCollection<T extends { id?: string}>(collection: T[], itemId?: string) {
    if (!itemId) {
      throw new Error('Invalid operation - id must be defined.');
    }
    const fromCollection = collection.find(x => x.id === itemId);
    if (!fromCollection) {
      throw new Error('Invalid operation - item does not exist.');
    }
    return fromCollection;
  }

  static async replaceInCollection<T extends { id?: string}>(collection: T[], item: T) {
    const index = collection.indexOf(this.getFromCollection(collection, item.id));
    collection[index] = item; 
  }

  static async reloadBalance(state: AppState): Promise<void> {
    const budgetBalance = await getBalanceRequest();
    state.budget.balance = budgetBalance.balance;
    state.budget.unallocated = budgetBalance.unallocated;
  }

  static async reloadAccount(state: AppState, accountId: string): Promise<void> {
    const account = await getAccountRequest(accountId);
    const accountIndex = state.budget.accounts.findIndex(x => x.id === account.id);
    state.budget.accounts[accountIndex] = account;
  }

  static async reloadFund(state: AppState, fundId: string): Promise<void> {
    const fund = await getFundRequest(fundId);
    const fundIndex = state.budget.funds.findIndex(x => x.id === fund.id);
    state.budget.funds[fundIndex] = fund;
  }

  static ensureDefined(actionName: string, ...payload: unknown[]): boolean {
    if (
      payload === null ||
      payload === undefined ||
      payload.some((x) => x === null || x === undefined)
    )
      throw new Error(`${actionName} action payload must be defined`);
    return true;
  }
}