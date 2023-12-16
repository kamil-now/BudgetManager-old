<template>
  <div class="input-dialog">
    <div class="input-dialog_content">
      <FundInput
        v-if="fund"
        :fund="fund"
        @changed="onFundChanged($event)"
      />
      <AccountInput
        v-if="account"
        :account="account"
        @changed="onAccountChanged($event)"
      />
      <IncomeInput
        v-if="operation?.type === MoneyOperationType.Income"
        :income="(operation as Income)"
        @changed="onOperationChanged($event)"
      />
      <AllocationInput
        v-if="operation?.type === MoneyOperationType.Allocation"
        :allocation="(operation as Allocation)"
        @changed="onOperationChanged($event)"
      />
      <ExpenseInput
        v-if="operation?.type === MoneyOperationType.Expense"
        :expense="(operation as Expense)"
        @changed="onOperationChanged($event)"
      />
      <CurrencyExchangeInput
        v-if="operation?.type === MoneyOperationType.CurrencyExchange"
        :currencyExchange="(operation as CurrencyExchange)"
        @changed="onOperationChanged($event)"
      />
      <AccountTransferInput
        v-if="operation?.type === MoneyOperationType.AccountTransfer"
        :accountTransfer="(operation as AccountTransfer)"
        @changed="onOperationChanged($event)"
      />
      <FundTransferInput
        v-if="operation?.type === MoneyOperationType.FundTransfer"
        :fundTransfer="(operation as FundTransfer)"
        @changed="onOperationChanged($event)"
      />
    </div>
    <div class="input-dialog_footer">
      <Button
        label="Discard"
        icon="pi pi-times"
        @click="discard()"
        text
      />
      <Button
        label="Save"
        icon="pi pi-check"
        @click="save()"
        autofocus
      />
    </div>
  </div>
</template>
<script setup lang="ts">
import { Expense } from '@/models/expense';
import { MoneyOperation } from '@/models/money-operation';
import { MoneyOperationType } from '@/models/money-operation-type.enum';
import { DynamicDialogInstance } from 'primevue/dynamicdialogoptions';
import { ComputedRef, inject, onMounted, ref } from 'vue';
import { Income } from '@/models/income';
import { Allocation } from '@/models/allocation';
import { CurrencyExchange } from '@/models/currency-exchange';
import { AccountTransfer } from '@/models/account-transfer';
import { FundTransfer } from '@/models/fund-transfer';
import FundInput from '@/components/FundInput.vue';
import AccountInput from '@/components/AccountInput.vue';
import IncomeInput from '@/components/IncomeInput.vue';
import AllocationInput from '@/components/AllocationInput.vue';
import ExpenseInput from '@/components/ExpenseInput.vue';
import CurrencyExchangeInput from '@/components/CurrencyExchangeInput.vue';
import AccountTransferInput from '@/components/AccountTransferInput.vue';
import FundTransferInput from '@/components/FundTransferInput.vue';
import { useAppStore } from '@/store/store';
import { Account } from '@/models/account';
import { Fund } from '@/models/fund';

const store = useAppStore();
const {
  createNewIncome,
  updateIncome,
  createNewAllocation,
  updateAllocation,
  createNewExpense,
  updateExpense,
  createNewCurrencyExchange,
  updateCurrencyExchange,
  createNewAccountTransfer,
  updateAccountTransfer,
  createNewFundTransfer,
  updateFundTransfer,
  createNewFund,
  updateFund,
  createNewAccount,
  updateAccount,
} = store;

const dialogRef = inject<ComputedRef<DynamicDialogInstance>>('dialogRef');
const operation = ref<MoneyOperation>();
const initialOperationValue = ref<MoneyOperation>();
const fund = ref<Fund>();
const initialFundValue = ref<Fund>();
const account = ref<Account>();
const initialAccountValue = ref<Account>();
onMounted(() => {
  operation.value = dialogRef?.value.data?.operation;
  if (operation.value) {
    initialOperationValue.value = JSON.parse(
      JSON.stringify(dialogRef?.value.data?.operation)
    );
  }

  fund.value = dialogRef?.value.data?.fund;
  if (fund.value) {
    initialFundValue.value = JSON.parse(
      JSON.stringify(dialogRef?.value.data?.fund)
    );
  }

  account.value = dialogRef?.value.data?.account;
  if (account.value) {
    initialAccountValue.value = JSON.parse(
      JSON.stringify(dialogRef?.value.data?.account)
    );
  }

  if (!operation.value && !fund.value && !account.value) {
    throw new Error();
  }
});

function onFundChanged(changed: Fund) {
  if (!fund.value || !changed) {
    throw new Error();
  }
  fund.value.name = changed.name;
}

function onAccountChanged(changed: Account) {
  if (!account.value || !changed) {
    throw new Error();
  }
  account.value.name = changed.name;
  account.value.balance = { ...changed.balance };
  account.value.initialBalance = { ...changed.initialBalance };
}

function onOperationChanged(changed: MoneyOperation) {
  if (!operation.value || !changed) {
    throw new Error();
  }
  operation.value.type = changed.type;
  operation.value.accountId = changed.accountId;
  operation.value.accountName = changed.accountName;
  operation.value.fundId = changed.fundId;
  operation.value.fundName = changed.fundName;
  operation.value.targetFundId = changed.targetFundId;
  operation.value.targetFundName = changed.targetFundName;
  operation.value.targetAccountId = changed.targetAccountId;
  operation.value.targetAccountName = changed.targetAccountName;
  operation.value.createdDate = changed.createdDate;
  operation.value.title = changed.title;
  operation.value.value = changed.value;
  operation.value.date = changed.date;
  operation.value.description = changed.description;
  operation.value.targetCurrency = changed.targetCurrency;
  operation.value.exchangeRate = changed.exchangeRate;
}
function save() {
  if (operation.value) {
    saveOperation();
  }
  else if (fund.value) {
    if (fund.value.id) {
      updateFund(fund.value);
    } else {
      createNewFund(fund.value);
    }
  }
  else if (account.value) {
    if (account.value.id) {
      updateAccount(account.value);
    } else {
      createNewAccount(account.value);
    }
  }
  dialogRef?.value.close();
}
function saveOperation() {
  if (!operation.value) {
    throw new Error();
  }
  if (operation.value.id) {
    // TODO move switch to store - updateOperation action
    switch (operation.value.type) {
    case MoneyOperationType.Income:
      updateIncome(operation.value as Income);
      break;
    case MoneyOperationType.Allocation:
      updateAllocation(operation.value as Allocation);
      break;
    case MoneyOperationType.Expense:
      updateExpense(operation.value as Expense);
      break;
    case MoneyOperationType.CurrencyExchange:
      updateCurrencyExchange(operation.value as CurrencyExchange);
      break;
    case MoneyOperationType.AccountTransfer:
      updateAccountTransfer(operation.value as AccountTransfer);
      break;
    case MoneyOperationType.FundTransfer:
      updateFundTransfer(operation.value as FundTransfer);
      break;
    default:
      throw new Error('Unknown operation.');
    }
  } else {
    // TODO move switch to store - createOperation action
    switch (operation.value.type) {
    case MoneyOperationType.Income:
      createNewIncome(operation.value as Income);
      break;
    case MoneyOperationType.Allocation:
      createNewAllocation(operation.value as Allocation);
      break;
    case MoneyOperationType.Expense:
      createNewExpense(operation.value as Expense);
      break;
    case MoneyOperationType.CurrencyExchange:
      createNewCurrencyExchange(operation.value as CurrencyExchange);
      break;
    case MoneyOperationType.AccountTransfer:
      createNewAccountTransfer(operation.value as AccountTransfer);
      break;
    case MoneyOperationType.FundTransfer:
      createNewFundTransfer(operation.value as FundTransfer);
      break;
    default:
      throw new Error('Unknown operation.');
    }
  }
}
function discard() {
  if (initialOperationValue.value) {
    onOperationChanged(initialOperationValue.value);
  }
  if (initialAccountValue.value) {
    onAccountChanged(initialAccountValue.value);
  }
  if (initialFundValue.value) {
    onFundChanged(initialFundValue.value);
  }
  dialogRef?.value.close();
}
</script>

<style lang="scss">
.input-dialog {
  display: flex;
  flex-direction: column;
  padding-top: 1rem;
  gap: 1rem;
  max-width: 100%;
  &_content {
    display: flex;
    flex-wrap: wrap;
    gap: 1rem;
  }
  &_footer {
    width: 100%;
    display: flex;
    gap: 1rem;
    justify-content: flex-end;
  }
}
</style>
