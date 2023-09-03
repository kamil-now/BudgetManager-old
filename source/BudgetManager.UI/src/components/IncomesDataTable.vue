<template>
  <div class="incomes-table">
    <ConfirmPopup></ConfirmPopup>
    <DynamicTable
      header="Incomes"
      v-model="incomes"
      :allowEdit="true"
      :createNew="createIncomeObject"
      :save="() => createNewIncome(changed)"
      :update="() => updateIncome(changed)"
      :remove="deleteIncome"
      :allowAdd="funds.length > 0 && accounts.length > 0"
      :onReorder="updateUserSettings"
    >
      <template #body="{ item }">
        <div class="incomes-table_body">
          <span class="date">{{ item.date }}</span>
          <div class="incomes-table_body-left">
            <span class="money">{{ DisplayFormat.money(item.value) }}</span>
            <span>{{ getAccountName(item.accountId) }}</span>
          </div>
          <div class="incomes-table_body-right">
            <span class="operation-title">{{ item.title }}</span>
            <span>{{ getFundName(item.fundId) }}</span>
          </div>
        </div>
      </template>
      <template #editor="{ item, index }">
        <div class="incomes-table_editor">
          <IncomeInput 
            :income="item" 
            @changed="onIncomeChanged($event)"
          />
          <Button 
            v-if="item.id && incomes.length > 1"
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
import IncomeInput from '@/components/IncomeInput.vue';
import { DisplayFormat } from '@/helpers/display-format';
import { Income } from '@/models/income';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';
import { useConfirm } from 'primevue/useconfirm';

const confirm = useConfirm();

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
let changed: Income;
function onIncomeChanged(newValue: Income) {
  changed = newValue;
}

function createIncomeObject() {
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
  const income = incomes.value[index];
  confirm.require({
    target: event.target as HTMLElement,
    message: `Remove ${income.title}?`,
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    rejectClass: 'p-button-secondary',
    accept: () => deleteIncome(income)
  });
}


</script>

<style lang="scss">
.incomes-table {
  width: 100%;
  &_body {
    display: flex;
    width: 100%;
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
