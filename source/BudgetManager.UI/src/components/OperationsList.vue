<template>
  <BudgetSpeedDial />
  <ConfirmPopup />
  <DynamicDialog />
  <DataTable
    v-model:filters="filters"
    filterDisplay="row"
    class="p-datatable-sm"
    :value="operations"
    scrollable
    tableStyle="width: 100%"
    paginator
    :rowsPerPageOptions="[10, 20, 50]"
    :rows="20"
    scrollHeight="calc(100vh - 1rem)"
  >
    <Column
      sortable
      field="date"
      header="Date"
    >
      <template #body="{ data }">
        <span class="date">{{ data.date }}</span>
      </template>
      <template #filter="{ filterModel, filterCallback }">
        <Calendar
          v-model="filterModel.value"
          dateFormat="yy/mm/dd"
          size="small"
          placeholder="yy/mm/dd"
          mask="9999/99/99"
          @date-select="filterModel.value = DateUtils.createDateOnlyString($event); filterCallback()"
        />
      </template>
    </Column>
    <Column
      sortable
      field="type"
      header="Type"
    >
      <template #body="{ data }">
        <span>{{ MoneyOperationType[data.type] }}</span>
      </template>

      <template #filter="{ filterModel, filterCallback }">
        <Dropdown
          class="p-inputtext-sm"
          v-model="filterModel.value"
          :options="moneyOperationTypes"
          @change="filterCallback()"
        >
          <template #value="{ value }">
            <span>{{ MoneyOperationType[value] }}</span>
          </template>
          <template #option="{ option }">
            <span>{{ MoneyOperationType[option] }}</span>
          </template>
        </Dropdown>
      </template>
    </Column>
    <Column
      sortable
      field="value"
      header="Value"
    >
      <template #body="{ data }">
        <span class="money">{{ DisplayFormat.money(data.value) }}</span>
      </template>
    </Column>
    <Column
      sortable
      field="title"
      header="Title"
    >
      <template #body="{ data }">
        <span>{{ data.title }}</span>
      </template>

      <template #filter="{ filterModel, filterCallback }">
        <InputText
          v-model="filterModel.value"
          type="text"
          size="small"
          @input="filterCallback()"
          class="p-column-filter"
          placeholder="title"
        />
      </template>
    </Column>
    <Column
      field="fundName"
      header="Fund"
    >
      <template #filter="{ filterModel, filterCallback }">
        <Dropdown
          class="p-inputtext-sm"
          v-model="filterModel.value"
          :options="store.fundsNames"
          placeholder="fund"
          @change="filterCallback()"
        ></Dropdown>
      </template>
    </Column>
    <Column
      field="accountName"
      header="Account"
    >
      <template #filter="{ filterModel, filterCallback }">
        <Dropdown
          class="p-inputtext-sm"
          v-model="filterModel.value"
          :options="store.accountsNames"
          placeholder="account"
          @change="filterCallback()"
        ></Dropdown>
      </template>
    </Column>
    <Column
      field="targetFundName"
      header="Target Fund"
    >
      <template #filter="{ filterModel, filterCallback }">
        <Dropdown
          class="p-inputtext-sm"
          v-model="filterModel.value"
          :options="store.fundsNames"
          placeholder="fund"
          @change="filterCallback()"
        ></Dropdown>
      </template>
    </Column>
    <Column
      field="targetAccountName"
      header="Target Account"
    >
      <template #filter="{ filterModel, filterCallback }">
        <Dropdown
          class="p-inputtext-sm"
          v-model="filterModel.value"
          :options="store.accountsNames"
          placeholder="account"
          @change="filterCallback()"
        ></Dropdown>
      </template>
    </Column>
    <Column>
      <template #body="{ data }">
        <MoneyOperationActions :operation="data"/>
      </template>
    </Column>
  </DataTable>
</template>
<script setup lang="ts">
import { DateUtils } from '@/helpers/date-utils';
import { DisplayFormat } from '@/helpers/display-format';
import { MoneyOperationType } from '@/models/money-operation-type.enum';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';
import { FilterMatchMode } from 'primevue/api';
import { ref } from 'vue';
import MoneyOperationActions from './MoneyOperationActions.vue';
import BudgetSpeedDial from './BudgetSpeedDial.vue';

const store = useAppStore();
const { operations } = storeToRefs(store);

const moneyOperationTypes = Object.keys(MoneyOperationType).filter(
  (item) => !isNaN(Number(item))
);
const filters = ref({
  date: { value: null, matchMode: FilterMatchMode.EQUALS },
  type: { value: null, matchMode: FilterMatchMode.EQUALS },
  title: { value: null, matchMode: FilterMatchMode.CONTAINS },
  fundName: { value: null, matchMode: FilterMatchMode.EQUALS },
  accountName: { value: null, matchMode: FilterMatchMode.EQUALS },
  targetFundName: { value: null, matchMode: FilterMatchMode.EQUALS },
  targetAccountName: { value: null, matchMode: FilterMatchMode.EQUALS },
});
</script>

<style lang="scss">
.operations-list {
  width: 100%;
  height: 100%;
  .hover-container {
    height: 100px !important;
    width: 100px !important;
    min-height: 100px;
    min-width: 100px;
    margin: 0;
    padding: 0;
  }
}
</style>
