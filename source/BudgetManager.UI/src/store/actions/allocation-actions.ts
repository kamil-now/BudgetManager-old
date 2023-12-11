import { createAllocationRequest, deleteAllocationRequest, getAllocationRequest, updateAllocationRequest } from '@/api/allocation-requests';
import { Allocation } from '@/models/allocation';
import { AppStore } from '../store';
import { StoreUtils } from '../store-utils';

export interface IAllocationActions { 
  createNewAllocation(allocation: Allocation): void,
  updateAllocation(allocation: Allocation): void;
  deleteAllocation(allocationId: string): void;
}

export class AllocationActions {
  static async createNewAllocation(store: AppStore, allocation: Allocation) {
    await StoreUtils.runAsyncOperation(store, async (state) => {
      const fromResponse = await StoreUtils.createOperation(
        state,
        () => createAllocationRequest(allocation),
        id => getAllocationRequest(id)
      );
      await this.reload(store, fromResponse);
    });
  }

  static async updateAllocation(store: AppStore, allocation: Allocation) {
    await StoreUtils.runAsyncOperation(store, async (state) => {
      const fromResponse = await StoreUtils.updateOperation(state, () => updateAllocationRequest(allocation));
      await this.reload(store, fromResponse);
    });
  }

  static async deleteAllocation(store: AppStore, allocationId: string) {
    await StoreUtils.runAsyncOperation(store, async () => {
      await deleteAllocationRequest(allocationId);
      await this.reload(store, StoreUtils.getFromCollection(store.allocations, allocationId));
      store.budget.operations = store.budget.operations.filter(x => x.id !== allocationId);
    });
  }

  private static async reload(store: AppStore, allocation: Allocation) {
    await StoreUtils.reloadFund(store, allocation.targetFundId);
    await StoreUtils.reloadBalance(store);
  }
}
