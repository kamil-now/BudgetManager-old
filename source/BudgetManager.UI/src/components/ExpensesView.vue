<template>
  <div class="expenses-view">
    <ListView
      header="Expenses"
      v-model="expenses"
      :copy="copyExpense"
      :createNew="createExpenseObject"
      :save="createNewExpense"
      :update="updateExpense"
      :remove="deleteExpense"
      :onReorder="updateUserSettings"
      :allowAdd="funds.length > 0 && accounts.length > 0"
    >
      <template #content="{ data }">
        <div class="expenses-view_body">
          <span class="date">{{ DisplayFormat.dateOnly(data.date) }}</span>
          <div class="expenses-view_body-left">
            <span class="money">{{ DisplayFormat.money(data.value) }}</span>
            <span>{{ getAccountName(data.accountId) }}</span>
          </div>
          <div class="expenses-view_body-right">
            <span class="operation-title">{{ data.title }}</span>
            <span>{{ getFundName(data.fundId) }}</span>
          </div>
        </div>
      </template>
      <template #editor="{ data }">
        <ExpenseInput 
          :expense="data" 
          @changed="onExpenseChanged(data, $event)"
        />
      </template>
    </ListView>
  </div>
</template>
<script setup lang="ts">
import ListView from '@/components/ListView.vue';
import ExpenseInput from '@/components/ExpenseInput.vue';
import { DisplayFormat } from '@/helpers/display-format';
import { Expense } from '@/models/expense';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';

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
  const defaultAccount = store.accounts.filter(x => !!x.id)[0];
  const defaultFund = store.funds.filter(x => !!x.id)[0];
  return  {
    date: new Date(),
    accountId: defaultAccount.id,
    fundId: defaultFund.id,
    value: { 
      currency: Object.keys(defaultAccount.balance)[0],
      amount: 0
    }
  };
}
function copyExpense(expense: Expense) {
  const copy =  { 
    ...expense,
    id: undefined
  };
  return copy;
}
</script>

<style lang="scss">
.expenses-view {
  width: 100%;
  height: 100%;
  &_body {
    display: flex;
    width: 100%;
    align-items: center;
    span {
      display: inline-block;
      text-overflow: ellipsis;
      overflow: hidden;
    }
    &-left {
      width: calc(50% - #{$date-width});
      display: flex;
      flex-direction: column;
      align-items: end;
      span {
        text-align: right;
      }
    }
    &-right {
      width: calc(50% - #{$date-width});
      display: flex;
      flex-direction: column;
      align-items: start;
      span {
        text-align: left;
        padding-left: 1rem;
      }
    }
  }
  &_editor {
    display: flex;
  }
}
</style>
