import { createAllocationRequest } from '@/api/allocation-requests';
import { createIncomeRequest } from '@/api/income-requests';
import { IncomeAllocationUtils } from '@/helpers/income-allocation-utils';
import { MoneyOperationFactory } from '@/helpers/money-operation-factory';
import { Account } from '@/models/account';
import { AccountTransfer } from '@/models/account-transfer';
import { Allocation } from '@/models/allocation';
import { CurrencyExchange } from '@/models/currency-exchange';
import { Expense } from '@/models/expense';
import { Fund } from '@/models/fund';
import { FundTransfer } from '@/models/fund-transfer';
import { Income } from '@/models/income';
import { IncomeAllocation } from '@/models/income-allocation';
import { IncomeAllocationRuleType } from '@/models/income-allocation-rule-type.enum';
import { MoneyOperationType } from '@/models/money-operation-type.enum';
import { DefineStoreOptions, Store, defineStore } from 'pinia';
import { AccountActions, IAccountActions } from './actions/account-actions';
import { AccountTransferActions, IAccountTransferActions } from './actions/account-transfer-actions';
import { AllocationActions, IAllocationActions } from './actions/allocation-actions';
import { BudgetActions, IBudgetActions } from './actions/budget-actions';
import { CurrencyExchangeActions, ICurrencyExchangeActions } from './actions/currency-exchange-actions';
import { ExpenseActions, IExpenseActions } from './actions/expense-actions';
import { FundActions, IFundActions } from './actions/fund-actions';
import { FundTransferActions, IFundTransferActions } from './actions/fund-transfer-actions';
import { IIncomeActions, IncomeActions } from './actions/income-actions';
import { IIncomeAllocationTemplateActions, IncomeAllocationTemplateActions } from './actions/income-allocation-template-actions';
import { IUserSettingsActions, UserSettingsActions } from './actions/user-settings-actions';
import { APP_GETTERS, AppGetters } from './getters';
import { AppState, INITIAL_APP_STATE } from './state';
import { StoreUtils } from './store-utils';

export type AppActions = 
  IBudgetActions 
  & IUserSettingsActions 
  & IAccountActions
  & IFundActions
  & IExpenseActions
  & IIncomeActions
  & IAllocationActions
  & IFundTransferActions
  & IAccountTransferActions
  & ICurrencyExchangeActions
  & IIncomeAllocationTemplateActions
  & {
  setLoggedIn(value: boolean): void;
};
export type AppStore = Store<string, AppState, AppGetters, AppActions>;

export const APP_STORE: DefineStoreOptions<
  string,
  AppState,
  AppGetters,
  AppActions
> = {
  id: 'app',
  state: () => INITIAL_APP_STATE,
  getters: APP_GETTERS,
  actions: {
    setLoggedIn(value: boolean) {
      this.isLoggedIn = value;
      this.fetchBudget();
    },

    createBudget() {
      BudgetActions.createBudget(this);
    },
    fetchBudget() {
      BudgetActions.fetchBudget(this);
    },

    reorderAccounts(oldIndex: number, newIndex: number) {
      UserSettingsActions.reorderAccounts(this, oldIndex, newIndex);
    },
    reorderFunds(oldIndex: number, newIndex: number) {
      UserSettingsActions.reorderFunds(this, oldIndex, newIndex);
    },
    updateUserSettings() {
      UserSettingsActions.updateUserSettings(this);
    },
    
    createNewAccount(account: Account) {
      AccountActions.createNewAccount(this, account);
    },
    updateAccount(account: Account) {
      AccountActions.updateAccount(this, account);
    },
    deleteAccount(accountId: string) {
      AccountActions.deleteAccount(this, accountId);
    },

    createNewFund(fund: Fund) {
      FundActions.createNewFund(this, fund);
    },
    updateFund(fund: Fund) {
      FundActions.updateFund(this, fund);
    },
    deleteFund(fundId: string) {
      FundActions.deleteFund(this, fundId);
    },

    createNewExpense(expense: Expense) {
      ExpenseActions.createNewExpense(this, expense);
    },
    updateExpense(expense: Expense) {
      ExpenseActions.updateExpense(this, expense);
    },
    deleteExpense(expenseId: string) {
      ExpenseActions.deleteExpense(this, expenseId);
    },

    createNewIncome(income: Income, incomeAllocation?: IncomeAllocation) {
      if (incomeAllocation && income.value) {
        // TODO validate income value is sufficient to accomodate all allocations
        const allocations: Allocation[] = [];
        const incomeValue = income.value.amount;
        const currency = income.value.currency;
        // TODO move allocation logic to one place
        let leftoverAfterLastFixedAllocation = incomeValue;
        let leftover = incomeValue;
        incomeAllocation.rules.forEach(rule => {
          if (leftover < 0) {
            leftoverAfterLastFixedAllocation = leftover = 0;
            return;
          }
          if (!rule.fundId) {
            throw new Error('Invalid income allocation rule.');
          }
          const { leftoverAmount } = IncomeAllocationUtils.calculate(
            rule.type === IncomeAllocationRuleType.Percent ? leftoverAfterLastFixedAllocation : leftover,
            rule
          );
          let ruleValue = 0;
          if (rule.type === IncomeAllocationRuleType.Fixed) {
            ruleValue = leftover - leftoverAmount;
            leftover = leftoverAmount;
            leftoverAfterLastFixedAllocation = leftoverAmount;
          } else {
            ruleValue = leftoverAfterLastFixedAllocation - leftoverAmount;
            leftover = leftover - ruleValue;
          }
          allocations.push({
            ...MoneyOperationFactory.create(this, MoneyOperationType.Allocation),
            title: '',
            value: {
              amount: ruleValue,
              currency
            },
            targetFundId: rule.fundId,
            targetFundName: rule.fundName,
            date: income.date
          });
        });
        if (leftover > 0) {
          if (!incomeAllocation.defaultFundId) {
            throw new Error('Income allocation default fund id must be defined.');
          }
          allocations.push({
            ...MoneyOperationFactory.create(this, MoneyOperationType.Allocation),
            title: '',
            value: {
              amount: leftover,
              currency
            },
            targetFundId: incomeAllocation.defaultFundId,
            targetFundName: incomeAllocation.defaultFundName,
            date: income.date
          });
        }
        StoreUtils.runAsyncOperation(this, async () => {
          await createIncomeRequest(income);
          await Promise.all(allocations.map(async allocation => createAllocationRequest(allocation)));
        }).then(() => this.fetchBudget());
      } else {
        IncomeActions.createNewIncome(this, income);
      }
    },
    updateIncome(income: Income) {
      IncomeActions.updateIncome(this, income);
    },
    deleteIncome(incomeId: string) {
      IncomeActions.deleteIncome(this, incomeId);
    },

    createNewAllocation(allocation: Allocation) {
      AllocationActions.createNewAllocation(this, allocation);
    },
    updateAllocation(allocation: Allocation) {
      AllocationActions.updateAllocation(this, allocation);
    },
    deleteAllocation(allocationId: string) {
      AllocationActions.deleteAllocation(this, allocationId);
    },
  
    createNewFundTransfer(fundTransfer: FundTransfer) {
      FundTransferActions.createNewFundTransfer(this, fundTransfer);
    },
    updateFundTransfer(fundTransfer: FundTransfer) {
      FundTransferActions.updateFundTransfer(this, fundTransfer);
    },
    deleteFundTransfer(fundTransferId: string) {
      FundTransferActions.deleteFundTransfer(this, fundTransferId);
    },

    createNewAccountTransfer(accountTransfer: AccountTransfer) {
      AccountTransferActions.createNewAccountTransfer(this, accountTransfer);
    },
    updateAccountTransfer(accountTransfer: AccountTransfer) {
      AccountTransferActions.updateAccountTransfer(this, accountTransfer);
    },
    deleteAccountTransfer(accountTransferId: string) {
      AccountTransferActions.deleteAccountTransfer(this, accountTransferId);
    },

    createNewCurrencyExchange(currencyExchange: CurrencyExchange) {
      CurrencyExchangeActions.createNewCurrencyExchange(this, currencyExchange);
    },
    updateCurrencyExchange(currencyExchange: CurrencyExchange) {
      CurrencyExchangeActions.updateCurrencyExchange(this, currencyExchange);
    },
    deleteCurrencyExchange(currencyExchangeId: string) {
      CurrencyExchangeActions.deleteCurrencyExchange(this, currencyExchangeId);
    },

    createNewIncomeAllocationTemplate(incomeAllocation: IncomeAllocation) {
      IncomeAllocationTemplateActions.createNewIncomeAllocationTemplate(this, incomeAllocation);
    },
    updateIncomeAllocationTemplate(incomeAllocation: IncomeAllocation) {
      IncomeAllocationTemplateActions.updateIncomeAllocationTemplate(this, incomeAllocation);
    },
    deleteIncomeAllocationTemplate(incomeAllocationTemplateId: string) {
      IncomeAllocationTemplateActions.deleteIncomeAllocationTemplate(this, incomeAllocationTemplateId);
    },
  }
};

export const useAppStore = defineStore<
  string,
  AppState,
  AppGetters,
  AppActions
>(APP_STORE);
