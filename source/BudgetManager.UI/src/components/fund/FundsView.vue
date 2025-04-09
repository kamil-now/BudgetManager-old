<template>
  <div class="funds-view">
    <ListView
      header="Funds"
      v-model="funds"
      :update="updateFund"
      :remove="deleteFund"
      :onReorder="reorderFunds"
    >
      <template #content="{ data }">
        <div class="funds-view_body">
          <div class="funds-view_body_balance">
            <template v-for="(value, currency) in data.balance">
              <MoneySpan
                v-if="value > 0.01 || value < -0.01"
                :key="currency"
                :amount="value"
                :currency="currency.toString()"
              />
            </template>
          </div>
          <div class="funds-view_body_name fund-name">{{ data.name }}</div>
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
import FundInput from '@/components/fund/FundInput.vue';
import MoneySpan from '@/components/MoneySpan.vue';
import { Fund } from '@/models/fund';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';

const store = useAppStore();
const { updateFund, deleteFund, reorderFunds } = store;

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
        justify-content: flex-end;
      }
    }
  }
  &_editor {
    display: flex;
  }
}
</style>
