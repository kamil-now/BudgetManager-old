<template>
  <div class="operations-view">
    <BudgetSpeedDial />
    <ConfirmPopup />
    <DynamicDialog />
    <div class="operations-view_filters">
      <Calendar
          v-model="dateFilter"
          dateFormat="yy/mm/dd"
          size="small"
          placeholder="yy/mm/dd"
          mask="9999/99/99"
          @date-select="dateFilter = DateUtils.createDateOnlyString($event);"
        />
      <span class="p-input-icon-left">
        <i class="pi pi-search" />
        <InputText
          ref="input"
          v-model="filter"
          placeholder="Search"
        />
      </span>
      <Dropdown
        v-model="typeFilter"
        :options="moneyOperationTypes"
      >
        <template #value="{ value }">
          <span>{{ MoneyOperationType[value] }}</span>
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
      :virtualScrollerOptions="{ itemSize: 40 }"
    >
      <template #actions="{ data }">
        <MoneyOperationActions :operation="data" />
      </template>
      <template #content="{ data }">
        <div class="operations-view_body">
          <div class="operations-view_body_left">
            <span class="date">{{ data.date }}</span>
            <div
              class="money"
              :class="{
                income: data.type === MoneyOperationType.Income,
                expense: data.type === MoneyOperationType.Expense,
                allocation: data.type === MoneyOperationType.Allocation,
                exchange: data.type === MoneyOperationType.CurrencyExchange,
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
              <i
                v-else-if="data.type === MoneyOperationType.CurrencyExchange"
                class="pi pi-arrow-right-arrow-left exchange"
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
              class="pi pi-arrow-right-arrow-left exchange"
            ></i>
            <span v-if="data.targetFundName">{{ data.targetFundName }}</span>
            <span v-if="data.targetAccountName">
              {{ data.targetAccountName }}
            </span>
            <span v-if="data.targetCurrency">
              {{
                Math.round(data.value.amount / data.exchangeRate, 2) +
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
import BudgetSpeedDial from '@/components/BudgetSpeedDial.vue';
import { DisplayFormat } from '@/helpers/display-format';
import { MoneyOperationType } from '@/models/money-operation-type.enum';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';
import { computed,  nextTick, onMounted, ref } from 'vue';
import MoneyOperationActions from './MoneyOperationActions.vue';

const store = useAppStore();
const input = ref();

const { filteredOperations, operationsFilter, operationsTypeFilter, operationsDateFilter } =
  storeToRefs(store);
const moneyOperationTypes = Object.keys(MoneyOperationType).filter(
  (item) => !isNaN(Number(item))
);
const filter = computed({
  get: () => operationsFilter.value,
  set: (newValue) => {
    operationsFilter.value = newValue;
  },
});
const typeFilter = computed({
  get: () => operationsTypeFilter.value,
  set: (newValue) => {
    operationsTypeFilter.value =
      MoneyOperationType[MoneyOperationType[newValue]];
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
</script>

<style lang="scss">
.operations-view {
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  flex-direction: column;
  @include media-breakpoint(lg, down) {
    .date {
      font-size: 0.75rem;
    }
  }
  &_filters {
    padding: 0.5rem;
    display: flex;
    width: 100%;
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
      flex-wrap: wrap;
    }
  }
}
</style>
