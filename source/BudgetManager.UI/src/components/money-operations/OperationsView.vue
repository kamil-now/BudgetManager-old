<template>
  <div class="operations-view">
    <ConfirmPopup />
    <DynamicDialog />
    <div class="operations-view_filters">
      <Calendar
        style="width: 10rem"
        v-model="dateRangeFilter"
        selectionMode="range"
        size="small"
        placeholder="yy/mm/dd - yy/mm/dd"
        mask="9999/99/99"
      />
      <span class="p-input-icon-left">
        <i class="pi pi-search" />
        <InputText
          style="width: 8rem"
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
          <span :class="{ placeholder: value === 0 }">
            {{ value === 0 ? "type" : MoneyOperationType[value] }}
          </span>
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
    <div class="operations-view_balance">
      <BalanceLabel
        v-if="
          !!operationsContentFilter ||
          dateRangeFilter.length > 0 ||
          (!!typeFilter &&
            [
              MoneyOperationType.Income,
              MoneyOperationType.Expense,
              MoneyOperationType.CurrencyExchange,
            ].includes(typeFilter))
        "
        :balance="filteredOperationsBalance"
        :useColors="true"
      />
    </div>
    <ListView
      header="Operations"
      v-model="filteredOperations"
      :virtualScrollerOptions="{ itemSize: 40, lazy: true, step: 30 }"
    >
      <template #actions="{ data }">
        <MoneyOperationActions :operation="data" />
      </template>
      <template #content="{ data }">
        <div class="operations-view_body">
          <div class="operations-view_body_left">
            <span class="date">{{ data.date }}</span>
            <component :is="getIcon(data.type)">
              <MoneySpan :money="data.value" />
            </component>
          </div>
          <div class="operations-view_body_right">
            <span v-if="data.title">{{ data.title }}</span>
            <span v-if="data.fundName">
              <FundIcon>
                <span class="fund-name">{{ data.fundName }}</span>
              </FundIcon>
            </span>
            <span v-if="data.accountName">
              <AccountIcon>
                <span class="account-name">{{ data.accountName }}</span>
              </AccountIcon>
            </span>
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
            <span v-if="data.targetFundName">
              <FundIcon>
                <span class="fund-name">{{ data.targetFundName }}</span>
              </FundIcon>
            </span>
            <span v-if="data.targetAccountName">
              <AccountIcon>
                <span class="account-name">{{ data.targetAccountName }}</span>
              </AccountIcon>
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
import BalanceLabel from '@/components/BalanceLabel.vue';
import MoneySpan from '@/components/MoneySpan.vue';
import { DateUtils } from '@/helpers/date-utils';
import { MoneyOperationType } from '@/models/money-operation-type.enum';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';
import { computed, ref } from 'vue';
import FundIcon from '@/components/icons/FundIcon.vue';
import AccountIcon from '@/components/icons/AccountIcon.vue';
import AccountTransferIcon from '@/components/icons/AccountTransferIcon.vue';
import FundTransferIcon from '@/components/icons/FundTransferIcon.vue';
import IncomeIcon from '@/components/icons/IncomeIcon.vue';
import ExpenseIcon from '@/components/icons/ExpenseIcon.vue';
import AllocationIcon from '@/components/icons/AllocationIcon.vue';
import CurrencyExchangeIcon from '@/components/icons/CurrencyExchangeIcon.vue';
import MoneyOperationActions from '@/components/money-operations/MoneyOperationActions.vue';
import { MoneyOperation } from '@/models/money-operation';
import { Balance } from '@/models/balance';
import { EnumUtils } from '@/helpers/enum-utils';

const store = useAppStore();

const {
  filteredOperations,
  operationsContentFilter,
  operationsTypeFilter,
  operationsDateFromFilter,
  operationsDateToFilter,
} = storeToRefs(store);
const moneyOperationTypes = EnumUtils.getStringValues(MoneyOperationType);

const filteredOperationsBalance = computed({
  get: () => getOperationsBalance(filteredOperations.value),
  set: () => {},
});

const filter = computed({
  get: () => operationsContentFilter.value,
  set: (newValue) => {
    operationsContentFilter.value = newValue;
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

const selectedDateRange = ref<Date[]>([]);
const dateRangeFilter = computed({
  get: () => selectedDateRange.value,
  set: (newValue) => {
    selectedDateRange.value = newValue;
    operationsDateFromFilter.value = DateUtils.createDateOnlyString(
      new Date(newValue[0])
    );
    operationsDateToFilter.value = DateUtils.createDateOnlyString(
      new Date(newValue[1])
    );
  },
});

function clearFilters() {
  typeFilter.value = MoneyOperationType.Undefined;
  filter.value = '';
  operationsDateFromFilter.value = '';
  operationsDateToFilter.value = '';
  selectedDateRange.value = [];
}

function getOperationsBalance(operations: MoneyOperation[]) {
  const balance: Balance = {};
  for (const operation of operations) {
    if (!balance[operation.value.currency]) {
      balance[operation.value.currency] = 0;
    }
    if (operation.targetCurrency && !balance[operation.targetCurrency]) {
      balance[operation.targetCurrency] = 0;
    }
    switch (operation.type) {
      case MoneyOperationType.Income:
        balance[operation.value.currency] += operation.value.amount;
        break;
      case MoneyOperationType.Expense:
        balance[operation.value.currency] -= operation.value.amount;
        break;
      case MoneyOperationType.CurrencyExchange:
        balance[operation.value.currency] -= operation.value.amount;
        balance[operation.targetCurrency!] +=
          operation.value.amount / operation.exchangeRate!;
        break;
      default:
        break;
    }
  }
  return balance;
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
    padding-bottom: 0.25rem;

    @media (max-width: 800px) {
      padding-top: 0.5rem;
    }

    display: flex;
    width: 100%;
    flex-wrap: wrap;
    gap: 1rem;
    align-items: start;
    justify-content: start;
  }

  &_balance {
    display: flex;
    align-items: center;
    padding: 0.25rem 0;
    width: 100%;

    .balance-label {
      height: 2.25rem;
    }
  }

  .list-view {
    height: calc(100% - 5rem);
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
