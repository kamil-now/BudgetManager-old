import currencies from '@/assets/currencies.json';
import { MoneyOperation } from '@/models/money-operation';
import { MoneyOperationType } from '@/models/money-operation-type.enum';
import { DateUtils } from './date-utils';
import { AppStore } from '@/store/store';
import { Income } from '@/models/income';
import { MoneyOperationUtils } from './money-operation-utils';
import { Allocation } from '@/models/allocation';
import { AccountTransfer } from '@/models/account-transfer';
import { CurrencyExchange } from '@/models/currency-exchange';
import { Expense } from '@/models/expense';
import { FundTransfer } from '@/models/fund-transfer';

export class MoneyOperationFactory {

  public static create(store: AppStore, type: MoneyOperationType): MoneyOperation {
    return {
      ...this.createNewOperationBase(type),
      ...this.createNewOperationDetails(store, type)
    };
  }

  private static createNewOperationBase(type: MoneyOperationType): MoneyOperation {
    return {
      id: undefined,
      title: '',
      type,
      date: DateUtils.createDateOnlyString(new Date()),
      value: {
        currency: Object.keys(currencies)[0],
        amount: 0,
      },
      createdDate: new Date().toString(),
      targetCurrency: Object.keys(currencies)[1],
    };
  }

  private static createNewOperationDetails(store: AppStore, type: MoneyOperationType): Partial<MoneyOperation> {
    const lastOperationOfType = store.lastOperation(type);
    if (lastOperationOfType) {
      return {
        ...MoneyOperationUtils.copy(lastOperationOfType, store.isFilteredByTypeOrContent),
        value: {
          amount: 0,
          currency: lastOperationOfType.value.currency
        }
      };
    }

    switch (type) {
      case MoneyOperationType.Income:
        return this.createNewIncome(store);
      case MoneyOperationType.Allocation:
        return this.createNewAllocation(store);
      case MoneyOperationType.Expense:
        return this.createNewExpense(store);
      case MoneyOperationType.CurrencyExchange:
        return this.createNewCurrencyExchange(store);
      case MoneyOperationType.AccountTransfer:
        return this.createNewAccountTransfer(store);
      case MoneyOperationType.FundTransfer:
        return this.createNewFundTransfer(store);
      default:
        throw new Error('Unknown operation.');
    }
  }

  private static createNewIncome(store: AppStore): Partial<Income> {
    return this.tryCreateFromLastUsedAccount(store);
  }

  private static createNewAllocation(store: AppStore): Partial<Allocation> {
    const unallocatedCurrencies = Object.keys(store.unallocated).filter(x => store.unallocated[x] !== 0);
    if (unallocatedCurrencies) {
      if (store.lastUsedFund) {
        for (const lastUsedFundCurrency of Object.keys(store.lastUsedFund.balance)) {
          if (unallocatedCurrencies.includes(lastUsedFundCurrency)) {
            return {
              ...this.tryCreateFromLastUsedFund(store),
              value: {
                amount: store.unallocated[lastUsedFundCurrency],
                currency: lastUsedFundCurrency
              }
            };
          }
        }
      }
      return {
        value: {
          amount: store.unallocated[unallocatedCurrencies[0]],
          currency: unallocatedCurrencies[0]
        }
      };
    }
    return {
      ...this.tryCreateFromLastUsedFund(store)
    };
  }

  private static createNewExpense(store: AppStore): Partial<Expense> {
    return {
      ...this.tryCreateFromLastUsedAccount(store),
      ...this.tryCreateFromLastUsedFund(store)
    };
  }

  private static createNewCurrencyExchange(store: AppStore): Partial<CurrencyExchange> {
    return this.tryCreateFromLastUsedAccount(store);
  }

  private static createNewAccountTransfer(store: AppStore): Partial<AccountTransfer> {
    return this.tryCreateFromLastUsedAccount(store);
  }

  private static createNewFundTransfer(store: AppStore): Partial<FundTransfer> {
    return this.tryCreateFromLastUsedFund(store);
  }

  private static tryCreateFromLastUsedAccount(store: AppStore): Partial<MoneyOperation> {
    if (store.lastUsedAccount) {
      const lastUsedAccountCurrencies = Object.keys(store.lastUsedAccount?.balance);
      return {
        accountId: store.lastUsedAccount.id,
        accountName: store.lastUsedAccount.name,
        targetAccountId: store.lastUsedAccount.id,
        targetAccountName: store.lastUsedAccount.name,
        value: {
          amount: 0,
          currency: Object.keys(store.lastUsedAccount.balance)[0],
        },
        targetCurrency: Object.keys(currencies).find((x) => !lastUsedAccountCurrencies.includes(x)),
      };
    }
    return {};
  }

  private static tryCreateFromLastUsedFund(store: AppStore): Partial<MoneyOperation> {
    if (store.lastUsedFund) {
      return {
        fundId: store.lastUsedFund.id,
        fundName: store.lastUsedFund.name,
        targetFundId: store.lastUsedFund.id,
        targetFundName: store.lastUsedFund.name,
        value: {
          amount: 0,
          currency: Object.keys(store.lastUsedFund.balance)[0],
        }
      };
    }
    return {};
  }
}