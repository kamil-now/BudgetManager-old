<template>
  <div class="operations-view">
    <ListView
      header="Operations"
      v-model="operations"
    >
      <template #content="{ data }">
        <div class="operations-view_body">
          <div class="operations-view_body_left">
            <span class="date">{{ data.date }}</span>
            <div
              class="money"
              :class="{
                income: data.type === MoneyOperationType.Income,
                expense: data.type === MoneyOperationType.Expense,
                allocation: data.type === MoneyOperationType.Allocation
              }"
            >
              <i
                v-if="data.type === MoneyOperationType.Expense"
                class="pi pi-minus"
              ></i>
              <i
                v-else-if="data.type === MoneyOperationType.Income"
                class="pi pi-plus"
              ></i>
              <i
                v-else-if="data.type === MoneyOperationType.Allocation"
                class="pi pi-file-import"
              ></i>
              {{ DisplayFormat.money(data.value) }}
            </div>
          </div>
          <div class="operations-view_body_right">
            <span v-if="data.title">{{ data.title }}</span>
            <span v-if="data.fundName">{{ data.fundName }}</span>
            <span v-if="data.accountName">{{ data.accountName }}</span>

            <i
              v-if="data.type === MoneyOperationType.FundTransfer"
              class="pi pi-forward transfer"
            ></i>
            <i
              v-else-if="data.type === MoneyOperationType.AccountTransfer"
              class="pi pi-arrows-h transfer"
            ></i>
            <i
              v-else-if="data.type === MoneyOperationType.CurrencyExchange"
              class="pi pi-arrow-right-arrow-left"
            ></i>
            <span v-if="data.targetFundName">{{ data.targetFundName }}</span>
            <span v-if="data.targetAccountName">
              {{ data.targetAccountName }}
            </span>
          </div>
        </div>
      </template>
    </ListView>
  </div>
</template>
<script setup lang="ts">
import ListView from '@/components/ListView.vue';
import { DisplayFormat } from '@/helpers/display-format';
import { MoneyOperationType } from '@/models/money-operation-type.enum';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';

const store = useAppStore();
const { operations } = storeToRefs(store);
</script>

<style lang="scss">
.operations-view {
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  @include media-breakpoint(lg, down) {
   .date{
    
    font-size: 0.75rem;
   }
  }
  &_body {
    display: flex;
    width: 100%;
    &_left {
      width: 50%;
      gap: 1rem;
      display: flex;
      align-items: center;
      justify-content: end;
      padding-right: 1rem;
      text-overflow: ellipsis;
      overflow: hidden;
    }
    &_right {
      width: 50%;
      gap: 1rem;
      display: flex;
      align-items: center;
      justify-content: start;
      text-overflow: ellipsis;
      overflow: hidden;
      flex-wrap: wrap
    }
  }
}
</style>
