<template>
  <div class="incomes-view">
    <ListView
      header="Incomes"
      v-model="incomes"
      :createNew="createIncomeObject"
      :save="createNewIncome"
      :update="updateIncome"
      :remove="deleteIncome"
      :allowAdd="funds.length > 0 && accounts.length > 0"
      :onReorder="updateUserSettings"
    >
      <template #content="{ data }">
        <div class="incomes-view_body">
          <span class="date">{{ data.date }}</span>
          <div class="incomes-view_body-left">
            <span class="money">{{ DisplayFormat.money(data.value) }}</span>
            <span>{{ getAccountName(data.accountId) }}</span>
          </div>
          <div class="incomes-view_body-right">
            <span class="operation-title">{{ data.title }}</span>
            <span>{{ getFundName(data.fundId) }}</span>
          </div>
        </div>
      </template>
      <template #editor="{ data }">
        <IncomeInput 
          :income="data" 
          @changed="onIncomeChanged(data, $event)"
        />
      </template>
    </ListView>
  </div>
</template>
<script setup lang="ts">
import ListView from '@/components/ListView.vue';
import IncomeInput from '@/components/IncomeInput.vue';
import { DisplayFormat } from '@/helpers/display-format';
import { Income } from '@/models/income';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';

const store = useAppStore();
const { createNewIncome, updateIncome, deleteIncome, updateUserSettings } = store;

const { incomes, accounts, funds } = storeToRefs(store);
// TODO extend DTO instead
function getAccountName(accountId: string) {
  return accounts.value.find(x => x.id === accountId)?.name;
}
// TODO extend DTO instead
function getFundName(fundId: string) {
  return funds.value.find(x => x.id === fundId)?.name;
}

function onIncomeChanged(income: Income, newValue: Income) {
  income.accountId = newValue.accountId;
  income.fundId = newValue.fundId;
  income.createdDate = newValue.createdDate;
  income.title = newValue.title;
  income.value = newValue.value;
  income.date = newValue.date;
  income.description = newValue.description;
}

function createIncomeObject() {
  const defaultAccount = store.accounts[0];
  const defaultFund = store.funds[0];
  return  {
    date: new Date().toDateString(),
    accountId: defaultAccount.id,
    fundId: defaultFund.id,
    value: { 
      currency: defaultAccount.balance.currency,
      amount: 0
    }
  };
}

</script>

<style lang="scss">
.incomes-view {
  width: 100%;
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
