import axios from "axios";
import { defineStore, DefineStoreOptions, Store } from "pinia";

export type AppState = {
  isLoading: boolean;
  undoStack: ((state: AppState) => void)[];
};
export type AppGetters = {
  // findIndexById: (state: AppState) => (id: string) => number;
};
export type AppActions = {
  save(): void;
  undo(): void;
};
export type AppStore = Store<string, AppState, AppGetters, AppActions>;

export const getInitialAppState: () => AppState = () => ({
  isLoading: false,
  undoStack: [],
});

export const APP_STORE: DefineStoreOptions<
  string,
  AppState,
  AppGetters,
  AppActions
> = {
  id: "app",
  state: () => getInitialAppState(),
  actions: {
    async save() {
      await Utils.runAsyncOperation(this, (state) =>
        axios.patch("api/", { state })
      );
    },

    undo() {
      const action = this.undoStack.pop();
      if (action) {
        action(this);
        this.save();
      } else {
        console.error("Invalid undo operation");
      }
    },
  },
};

export const useAppStore = defineStore<
  string,
  AppState,
  AppGetters,
  AppActions
>(APP_STORE);

class Utils {
  static async runAsyncOperation(
    state: AppState,
    op: (state: AppState) => Promise<unknown>
  ): Promise<void> {
    state.isLoading = true;
    try {
      await op(state);
    } catch (error) {
      // TODO console.error(error);
    } finally {
      state.isLoading = false;
    }
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
