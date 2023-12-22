import { BudgetSummary } from '@/models/budget-summary';
import { MoneyOperationType } from '@/models/money-operation-type.enum';

export type AppState = {
  isLoading: boolean;
  isLoggedIn: boolean;
  isNewUser: boolean,
  budget: BudgetSummary,
  operationsFilter: string,
  operationsTypeFilter: MoneyOperationType,
  operationsDateFromFilter: string
  operationsDateToFilter: string
};

export const INITIAL_APP_STATE: AppState = {
  isLoading: true,
  isLoggedIn: false,
  isNewUser: true,
  budget: {
    userSettings: { accountsOrder: [], fundsOrder: [] },
    balance: {},
    unallocated: {},
    funds: [],
    accounts: [],
    operations: []
  },
  operationsFilter: '',
  operationsTypeFilter: MoneyOperationType.Undefined,
  operationsDateFromFilter: '',
  operationsDateToFilter: ''
};