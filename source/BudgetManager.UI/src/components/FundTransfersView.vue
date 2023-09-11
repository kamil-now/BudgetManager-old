<template>
  <div class="fundTransfers-view">
    <ListView
      header="Fund Transfers"
      v-model="fundTransfers"
      :createNew="createFundTransferObject"
      :save="createNewFundTransfer"
      :update="updateFundTransfer"
      :remove="deleteFundTransfer"
      :allowAdd="funds.length > 0 && accounts.length > 0"
    >
      <template #content="{ data }">
        <div class="fundTransfers-view_body">
          <span class="date">{{ DisplayFormat.dateOnly(data.date) }}</span>
          <div class="fundTransfers-view_body-left">
            <span class="money">{{ DisplayFormat.money(data.value) }}</span>
            <span>{{ getFundName(data.sourceFundId) }}</span>
          </div>
          <div class="fundTransfers-view_body-right">
            <span class="operation-title">{{ data.title }}</span>
            <span>{{ getFundName(data.targetFundId) }}</span>
          </div>
        </div>
      </template>
      <template #editor="{ data }">
        <FundTransferInput 
          :fundTransfer="data" 
          @changed="onFundTransferChanged(data, $event)"
        />
      </template>
    </ListView>
  </div>
</template>
<script setup lang="ts">
import currencies from '@/assets/currencies.json';
import FundTransferInput from '@/components/FundTransferInput.vue';
import ListView from '@/components/ListView.vue';
import { DisplayFormat } from '@/helpers/display-format';
import { FundTransfer } from '@/models/fund-transfer';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';

const store = useAppStore();
const { createNewFundTransfer, updateFundTransfer, deleteFundTransfer } = store;

const { fundTransfers, accounts, funds } = storeToRefs(store);
// TODO extend DTO instead
function getFundName(fundId: string) {
  return funds.value.find(x => x.id === fundId)?.name;
}

function onFundTransferChanged(fundTransfer: FundTransfer, newValue: FundTransfer) {
  fundTransfer.sourceFundId = newValue.sourceFundId;
  fundTransfer.targetFundId = newValue.targetFundId;
  fundTransfer.createdDate = newValue.createdDate;
  fundTransfer.title = newValue.title;
  fundTransfer.value = newValue.value;
  fundTransfer.date = newValue.date;
  fundTransfer.description = newValue.description;
}

function createFundTransferObject() {
  return  {
    date: new Date(),
    sourceFundId: store.funds[0].id,
    targetFundId: store.funds[1].id,
    value: { 
      currency: Object.keys(store.funds[0].balance)[0] ?? Object.keys(currencies)[0]
    }
  };
}
</script>

<style lang="scss">
.fundTransfers-view {
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
