<template>
  <div class="expenses-table">
    <ConfirmPopup></ConfirmPopup>
    <DynamicTable
      header="Expenses"
      v-model="expenses"
      :allowEdit="true"
      :createNew="createExpenseObject"
      :saveNew="createNewExpense"
      :update="updateExpense"
      :remove="deleteExpense"
      :onReorder="updateUserSettings"
    >
      <template #body="{ item }">
        <div class="expenses-table_body">
          <span>{{ item.date }}</span>
          <span>{{ item.title }}</span>
          <span>{{ getFundName(item.fundId) }}</span>
          <span>{{ getAccountName(item.accountId) }}</span>
          <span>{{ DisplayFormat.money(item.value) }}</span>
        </div>
      </template>
      <template #editor="{ item, index }">
        <div class="expenses-table_editor">
          <ExpenseInput 
            :expense="item" 
            @changed="onExpenseChanged(item, $event)"
          />
          <Button 
            v-if="item.id && expenses.length > 1"
            icon="pi pi-times" 
            severity="danger" 
            text 
            rounded 
            aria-label="Remove" 
            @click="removeAt($event, index)" 
          />
        </div>
      </template>
    </DynamicTable>
  </div>
</template>
<script setup lang="ts">
import DynamicTable from '@/components/DynamicTable.vue';
import ExpenseInput from '@/components/ExpenseInput.vue';
import { DisplayFormat } from '@/helpers/display-format';
import { Expense } from '@/models/expense';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';
import { useConfirm } from 'primevue/useconfirm';

const confirm = useConfirm();

const store = useAppStore();
const { createNewExpense, updateExpense, deleteExpense, updateUserSettings } = store;

const { expenses, accounts, funds } = storeToRefs(store);
// TODO extend DTO instead
function getAccountName(accountId: string) {
  return accounts.value.find(x => x.id === accountId)?.name;
}
// TODO extend DTO instead
function getFundName(fundId: string) {
  return funds.value.find(x => x.id === fundId)?.name;
}

function onExpenseChanged(expense: Expense, newValue: Expense) {
  expense.accountId = newValue.accountId;
  expense.fundId = newValue.fundId;
  expense.createdDate = newValue.createdDate;
  expense.title = newValue.title;
  expense.value = newValue.value;
  expense.date = newValue.date;
  expense.description = newValue.description;
}

function createExpenseObject() {
  const defaultAccount = store.accounts[0];
  const defaultFund = store.funds[0];
  return  {
    date: new Date(),
    accountId: defaultAccount.id,
    fundId: defaultFund.id,
    value: { 
      currency: defaultAccount.balance.currency
    }
  };
}

function removeAt(event: MouseEvent, index: number) {
  const expense = expenses.value[index];
  confirm.require({
    target: event.target as HTMLElement,
    message: `Remove ${expense.title}?`,
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    rejectClass: 'p-button-secondary',
    accept: () => deleteExpense(expense)
  });
}


</script>

<style lang="scss">
.expenses-table {
  width: 100%;
  &_body {
    display: flex;
    width: 100%;
    justify-content: space-between;
    * {
      display: inline-block;
      text-overflow: ellipsis;
      overflow: hidden;
    }
  }
  &_editor {
    display: flex;
  }
}
</style>
