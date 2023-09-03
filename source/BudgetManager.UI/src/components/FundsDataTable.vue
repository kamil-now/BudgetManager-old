<template>
  <div class="funds-table">
    <ConfirmPopup></ConfirmPopup>
    <DynamicTable
      header="Funds"
      v-model="funds"
      :allowEdit="true"
      :allowReorder="true"
      :createNew="createFundObject"
      :saveNew="createNewFund"
      :update="updateFund"
      :remove="deleteFund"
      :onReorder="updateUserSettings"
    >
      <template #body="{ item }">
        <div class="funds-table_body">
            <i 
              v-if="item.isDefault" 
              class="pi pi-star" 
              style="position: absolute; color: var(--primary-color); right: 0;"
            >
            </i>
          <div class="funds-table_body_balance">
            <div class="money" v-for="(value, currency) in item.balance" :key="currency">
              {{ DisplayFormat.money({ amount: value, currency: currency.toString() }) }}
            </div>
          </div>
          <div class="funds-table_body_name">{{ item.name }}</div>
        </div>
      </template>
      <template #editor="{ item, index }">
        <div class="funds-table_editor">
          <FundInput 
            :fund="item" 
            @changed="onFundChanged(item, $event)"
          />
          <Button 
            v-if="item.id && funds.length > 1"
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
import FundInput from '@/components/FundInput.vue';
import { DisplayFormat } from '@/helpers/display-format';
import { Fund } from '@/models/fund';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';
import { useConfirm } from 'primevue/useconfirm';

const confirm = useConfirm();

const store = useAppStore();
const { createNewFund, updateFund, deleteFund, updateUserSettings } = store;

const { funds } = storeToRefs(store);

function onFundChanged(fund: Fund, newValue: Fund) {
  fund.name = newValue.name;
}

function createFundObject() {
  return  { };
}

function removeAt(event: MouseEvent, index: number) {
  const fund = funds.value[index];
  confirm.require({
    target: event.target as HTMLElement,
    message: `Remove ${fund.name}?`,
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    rejectClass: 'p-button-secondary',
    accept: () => deleteFund(fund)
  });
}


</script>

<style lang="scss">
.funds-table {
  width: 100%;
  max-height: 100%;
  &_body {
    display: flex;
    width: 100%;
    &_name {
      width: 50%;
      text-align: left;
      padding-left: 1rem;
      display: inline-block;
      text-overflow: ellipsis;
      overflow: hidden;
    }
    &_balance {
      width: 50%;
      display: inline-block;
      text-overflow: ellipsis;
      overflow: hidden;
      > div {
        display: flex;
        justify-content: end;
      }
    }
  }
  &_editor {
    display: flex;
  }
}
</style>
