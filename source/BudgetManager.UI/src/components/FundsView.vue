<template>
  <div class="funds-view">
    <ListView
      header="Funds"
      v-model="funds"
      :save="createNewFund"
      :update="updateFund"
      :remove="deleteFund"
      :onReorder="reorderFunds"
      :allowAdd="true"
    >
      <template #content="{ data }">
        <div class="funds-view_body">
          <div class="funds-view_body_balance">
            <div class="money" v-for="(value, currency) in data.balance" :key="currency">
              {{ DisplayFormat.money({ amount: value, currency: currency.toString() }) }}
            </div>
          </div>
          <div class="funds-view_body_name">{{ data.name }}</div>
        </div>
      </template>
      <template #editor="{ data }">
        <FundInput 
          :fund="data" 
          @changed="onFundChanged(data, $event)"
        />
      </template>
    </ListView>
  </div>
</template>
<script setup lang="ts">
import ListView from '@/components/ListView.vue';
import FundInput from '@/components/FundInput.vue';
import { DisplayFormat } from '@/helpers/display-format';
import { Fund } from '@/models/fund';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';

const store = useAppStore();
const { createNewFund, updateFund, deleteFund, reorderFunds } = store;

const { funds } = storeToRefs(store);

function onFundChanged(fund: Fund, newValue: Fund) {
  fund.name = newValue.name;
}
</script>

<style lang="scss">
.funds-view {
  width: 100%;
  height: 100%;
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
