<template>
  <div class="operations-view">
    <ConfirmPopup />
    <DynamicDialog />
    <div class="operations-view_filters">
      <Calendar
        style="width: 8rem"
        v-model="dateFilter"
        dateFormat="yy/mm/dd"
        size="small"
        placeholder="yy/mm/dd"
        mask="9999/99/99"
        @date-select="dateFilter = DateUtils.createDateOnlyString($event)"
      />
      <span class="p-input-icon-left">
        <i class="pi pi-search" />
        <InputText
          style="width: 8rem"
          ref="input"
          v-model="filter"
          placeholder="search"
        />
      </span>
      <Dropdown
        v-model="typeFilter"
        :options="moneyOperationTypes"
        style="width: 8rem"
      >
        <template #value="{ value }">
          <span>{{ value == 0 ? "type" : MoneyOperationType[value] }}</span>
        </template>
        <template #option="{ option }">
          <span>{{ MoneyOperationType[option] }}</span>
        </template>
      </Dropdown>

      <Button
        icon="pi pi-times"
        text
        rounded
        aria-label="Clear"
        @click="clearFilters()"
      />
    </div>
    <ListView
      header="Operations"
      v-model="filteredOperations"
      :virtualScrollerOptions="{ itemSize: 40, lazy: true, step: 20 }"
    >
      <template #actions="{ data }">
        <MoneyOperationActions :operation="data" />
      </template>
      <template #content="{ data }">
        <div class="operations-view_body">
          <div class="operations-view_body_left">
            <span class="date">{{ data.date }}</span>
            <component :is="getIcon(data.type)">
              <span class="money">{{ DisplayFormat.money(data.value) }}</span> 
            </component>
          </div>
          <div class="operations-view_body_right">
            <span v-if="data.title">{{ data.title }}</span>
            <span v-if="data.fundName">{{ data.fundName }}</span>
            <span v-if="data.accountName">{{ data.accountName }}</span>
            <i
              v-if="
                [
                  MoneyOperationType.FundTransfer,
                  MoneyOperationType.AccountTransfer,
                  MoneyOperationType.CurrencyExchange,
                ].includes(data.type)
              "
              class="pi pi-arrow-right transfer-icon"
            ></i>
            <span v-if="data.targetFundName">{{ data.targetFundName }}</span>
            <span v-if="data.targetAccountName">
              {{ data.targetAccountName }}
            </span>
            <span v-if="data.targetCurrency">
              {{
                (data.value.amount / data.exchangeRate).toFixed(2) +
                " " +
                data.targetCurrency
              }}
            </span>
          </div>
        </div>
      </template>
    </ListView>
  </div>
</template>
<script setup lang="ts">
import ListView from '@/components/ListView.vue';
import { DateUtils } from '@/helpers/date-utils';
import { DisplayFormat } from '@/helpers/display-format';
import { MoneyOperationType } from '@/models/money-operation-type.enum';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';
import { computed, nextTick, onMounted, ref } from 'vue';
import AccountTransferIcon from './icons/AccountTransferIcon.vue';
import FundTransferIcon from './icons/FundTransferIcon.vue';
import IncomeIcon from './icons/IncomeIcon.vue';
import ExpenseIcon from './icons/ExpenseIcon.vue';
import AllocationIcon from './icons/AllocationIcon.vue';
import CurrencyExchangeIcon from './icons/CurrencyExchangeIcon.vue';
import MoneyOperationActions from './MoneyOperationActions.vue';

const store = useAppStore();
const input = ref();
const {
  filteredOperations,
  operationsFilter,
  operationsTypeFilter,
  operationsDateFilter,
} = storeToRefs(store);
const moneyOperationTypes = Object.keys(MoneyOperationType).filter(
  (item) => !isNaN(Number(item)) && item !== '0'
);

const filter = computed({
  get: () => operationsFilter.value,
  set: (newValue) => {
    operationsFilter.value = newValue;
  },
});
const typeFilter = computed({
  get: () => operationsTypeFilter.value,
  set: (newValue: number) => {
    operationsTypeFilter.value =
      MoneyOperationType[
        MoneyOperationType[newValue] as keyof typeof MoneyOperationType
      ];
  },
});
const dateFilter = computed({
  get: () => operationsDateFilter.value,
  set: (newValue) => {
    operationsDateFilter.value = newValue;
  },
});

onMounted(() => focusInput());

function focusInput() {
  nextTick(() => {
    input.value?.$el.focus();
  });
}

function clearFilters() {
  typeFilter.value = MoneyOperationType.Undefined;
  filter.value = '';
  dateFilter.value = '';
  focusInput();
}

function getIcon(type: MoneyOperationType) {
  switch (type) {
  case MoneyOperationType.Income:
    return IncomeIcon;
  case MoneyOperationType.Expense:
    return ExpenseIcon;
  case MoneyOperationType.AccountTransfer:
    return AccountTransferIcon;
  case MoneyOperationType.FundTransfer:
    return FundTransferIcon;
  case MoneyOperationType.Allocation:
    return AllocationIcon;
  case MoneyOperationType.CurrencyExchange:
    return CurrencyExchangeIcon;
  }
}
</script>

<style lang="scss">
.operations-view {
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  flex-direction: column;
  &_filters {
    padding: 0.5rem;
    display: flex;
    width: 100%;
    flex-wrap: wrap;
    gap: 1rem;
    display: flex;
    align-items: center;
    justify-content: start;
  }
  &_body {
    display: flex;
    width: 100%;
    &_left {
      width: 50%;
      gap: 1rem;
      display: flex;
      align-items: center;
      justify-content: space-between;
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
      flex-wrap: wrap;
    }
  }
}
</style>
