<template>
  <div class="incomes-view">
    <ListView header="Incomes" v-model="incomes" :createNew="createIncomeObject" :save="createNewIncome"
      :update="updateIncome" :remove="deleteIncome" :allowAdd="accounts.length > 0">
      <template #content="{ data }">
        <div class="incomes-view_body">
          <span class="date">{{ DisplayFormat.dateOnly(data.date) }}</span>
          <div class="incomes-view_body-left">
            <span class="money">{{ DisplayFormat.money(data.value) }}</span>
            <span>{{ data.accountName }}</span>
          </div>
          <div class="incomes-view_body-right">
            <span class="operation-title">{{ data.title }}</span>
          </div>
        </div>
      </template>
      <template #editor="{ data }">
        <IncomeInput :income="data" @changed="onIncomeChanged(data, $event)" />
      </template>
    </ListView>
  </div>
</template>
<script setup lang="ts">
import IncomeInput from '@/components/IncomeInput.vue';
import ListView from '@/components/ListView.vue';
import { DisplayFormat } from '@/helpers/display-format';
import { Income } from '@/models/income';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';

const store = useAppStore();
const { createNewIncome, updateIncome, deleteIncome } = store;

const { incomes, accounts } = storeToRefs(store);

function onIncomeChanged(income: Income, newValue: Income) {
  income.accountId = newValue.accountId;
  income.createdDate = newValue.createdDate;
  income.title = newValue.title;
  income.value = newValue.value;
  income.date = newValue.date;
  income.description = newValue.description;
}

function createIncomeObject() {
  const defaultAccount = store.accounts.filter(x => !!x.id)[0];
  return {
    date: new Date(),
    accountId: defaultAccount.id,
    value: {
      currency: Object.keys(defaultAccount.balance)[0],
      amount: 0
    }
  };
}

</script>

<style lang="scss">
.incomes-view {
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
