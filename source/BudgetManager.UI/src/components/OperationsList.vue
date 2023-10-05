<template>
  <SpeedDial
    :model="items"
    direction="up"
    buttonClass="p-button-outlined"
    :style="{ right: '2rem', bottom: '2rem' }"
  />
  <DynamicDialog />
  <ConfirmPopup />
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
    scrollHeight="80vh"
  >
    <Column
      sortable
      field="date"
      header="Date"
    >
      <template #body="{ data }">
        <span class="date">{{ DisplayFormat.dateOnly(data.date) }}</span>
      </template>
      <template #filter="{ filterModel, filterCallback }">
        <Calendar
          v-model="filterModel.value"
          dateFormat="dd/mm/yy"
          size="small"
          placeholder="dd/mm/yyyy"
          mask="99/99/9999"
          @date-select="filterCallback()"
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
      <template #body="{ data, index }">
        <div style="display: flex">
          <Button
            icon="pi pi-copy"
            text
            rounded
            size="small"
            aria-label="Copy"
            @click="createCopy(data)"
          />
          <Button
            icon="pi pi-pencil"
            text
            rounded
            size="small"
            aria-label="Edit"
            @click="edit(data)"
          />
          <Button
            icon="pi pi-times"
            severity="danger"
            text
            rounded
            size="small"
            aria-label="Remove"
            @click="removeAt($event, index)"
          />
        </div>
      </template>
    </Column>
  </DataTable>
</template>
<script setup lang="ts">
import currencies from '@/assets/currencies.json';
import { DisplayFormat } from '@/helpers/display-format';
import { StringUtils } from '@/helpers/string-utils';
import { MoneyOperation } from '@/models/money-operation';
import { MoneyOperationType } from '@/models/money-operation-type.enum';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';
import { FilterMatchMode } from 'primevue/api';
import { useConfirm } from 'primevue/useconfirm';
import { useDialog } from 'primevue/usedialog';
import { ref } from 'vue';
import InputDialog from './InputDialog.vue';
const dialog = useDialog();
const store = useAppStore();
const confirm = useConfirm();
const { operations } = storeToRefs(store);

const {
  deleteIncome,
  deleteAllocation,
  deleteExpense,
  deleteCurrencyExchange,
  deleteAccountTransfer,
  deleteFundTransfer,
} = store;

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
const items = ref([
  {
    label: 'Add Expense',
    icon: 'pi pi-minus',
    command: () => {
      const defaultAccount = store.accounts.filter((x) => !!x.id)[0];
      const defaultFund = store.funds.filter((x) => !!x.id)[0];
      edit({
        ...createNewMoneyOperation(),
        type: MoneyOperationType.Expense,
        accountId: defaultAccount.id,
        accountName: defaultAccount.name,
        fundId: defaultFund.id,
        fundName: defaultFund.name,
        value: {
          currency: Object.keys(defaultAccount.balance)[0],
          amount: 0,
        },
      });
    },
  },
  {
    label: 'Add Allocation',
    icon: 'pi pi-circle',
    command: () => {
      const defaultFund = store.funds.filter((x) => !!x.id)[0];
      edit({
        ...createNewMoneyOperation(),
        type: MoneyOperationType.Allocation,
        fundId: defaultFund.id,
        fundName: defaultFund.name,
        value: {
          currency: Object.keys(defaultFund.balance)[0],
          amount: 0,
        },
      });
    },
  },
  {
    label: 'Add Currency Exchange',
    icon: 'pi pi-arrow-right-arrow-left',
    command: () => {
      const defaultAccount = store.accounts.filter((x) => !!x.id)[0];
      const accountCurrencies = Object.keys(defaultAccount.initialBalance);
      const targetCurrency = Object.keys(currencies).filter(
        (x) => !accountCurrencies.includes(x)
      )[0];
      edit({
        ...createNewMoneyOperation(),
        type: MoneyOperationType.CurrencyExchange,
        accountId: defaultAccount.id,
        accountName: defaultAccount.name,
        targetCurrency,
        value: {
          currency: Object.keys(defaultAccount.balance)[0],
          amount: 0,
        },
      });
    },
  },
  {
    label: 'Add Fund Transfer',
    icon: 'pi pi-arrows-h',
    command: () => {
      const defaultSourceFund = store.funds.filter((x) => !!x.id)[0];
      const defaultTargetFund = store.funds.filter((x) => !!x.id)[0];
      edit({
        ...createNewMoneyOperation(),
        type: MoneyOperationType.FundTransfer,
        fundId: defaultSourceFund.id,
        fundName: defaultSourceFund.name,
        targetFundId: defaultTargetFund.id,
        targetFundName: defaultTargetFund.name,
        value: {
          currency:
            Object.keys(defaultSourceFund.balance)[0] ??
            Object.keys(currencies)[0],
          amount: 0,
        },
      });
    },
  },
  {
    label: 'Add Account Transfer',
    icon: 'pi pi-arrows-v',
    command: () => {
      const defaultSourceAccount = store.accounts.filter((x) => !!x.id)[0];
      const defaultTargetAccount = store.accounts.filter((x) => !!x.id)[0];
      edit({
        ...createNewMoneyOperation(),
        type: MoneyOperationType.FundTransfer,
        accountId: defaultSourceAccount.id,
        accountName: defaultSourceAccount.name,
        targetAccountId: defaultTargetAccount.id,
        targetAccountName: defaultTargetAccount.name,
        value: {
          currency:
            Object.keys(defaultSourceAccount.balance)[0] ??
            Object.keys(currencies)[0],
          amount: 0,
        },
      });
    },
  },
  {
    label: 'Add Income',
    icon: 'pi pi-plus',
    command: () => {
      const defaultAccount = store.accounts.filter((x) => !!x.id)[0];
      edit(
        {
          ...createNewMoneyOperation(),
          type: MoneyOperationType.Income,
          accountId: defaultAccount.id,
          accountName: defaultAccount.name,
          value: {
            currency: Object.keys(defaultAccount.balance)[0],
            amount: 0,
          },
        },
        'Create'
      );
    },
  },
  {
    label: 'Add Fund',
    icon: 'pi pi-dollar',
    command: () => {
      dialog.open(InputDialog, {
        data: {
          fund: {},
        },
        props: {
          header: 'Create New Fund',
          modal: true,
          closable: false,
        },
      });
    },
  },
  {
    label: 'Add Account',
    icon: 'pi pi-wallet',
    command: () => {
      let defaultCurrency = Object.keys(currencies)[0];
      const acc = store.accounts[store.accounts.length - 1];
      if (
        acc &&
        acc.initialBalance &&
        Object.keys(acc.initialBalance).length > 0
      ) {
        defaultCurrency = Object.keys(acc.initialBalance)[0];
      }
      dialog.open(InputDialog, {
        data: {
          account: {
            balance: {
              [defaultCurrency]: 0,
            },
            initialBalance: {
              [defaultCurrency]: 0,
            },
          },
        },
        props: {
          header: 'Create New Account',
          modal: true,
          closable: false,
        },
      });
    },
  },
]);

function createNewMoneyOperation(): MoneyOperation {
  return {
    id: undefined,
    title: '',
    type: MoneyOperationType.Undefined,
    date: DisplayFormat.dateOnly(new Date()),
    value: {
      currency: Object.keys(currencies)[0],
      amount: 0,
    },
    createdDate: new Date().toISOString(),
  };
}

function createCopy(operation: MoneyOperation) {
  const copy = {
    ...operation,
    date: DisplayFormat.dateOnly(new Date()),
    id: undefined,
  };
  edit(copy, 'Create');
}
function edit(operation: MoneyOperation, action: 'Edit' | 'Create' = 'Edit') {
  dialog.open(InputDialog, {
    data: {
      operation,
    },
    props: {
      header: `${action} ${StringUtils.camelCaseToWords(
        MoneyOperationType[operation.type]
      )}`,
      modal: true,
      closable: false,
    },
  });
}
function removeAt(event: MouseEvent, index: number) {
  confirm.require({
    target: event.target as HTMLElement,
    message:
      'Are you sure you want to remove this operation? This action is permanent.',
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    rejectClass: 'p-button-secondary',
    accept: () => removeOperation(operations.value[index])
  });
}
function removeOperation(operation: MoneyOperation) {
  if (!operation.id) {
    throw new Error();
  }
  switch (operation.type) {
  case MoneyOperationType.Income:
    deleteIncome(operation.id);
    break;
  case MoneyOperationType.Allocation:
    deleteAllocation(operation.id);
    break;
  case MoneyOperationType.Expense:
    deleteExpense(operation.id);
    break;
  case MoneyOperationType.CurrencyExchange:
    deleteCurrencyExchange(operation.id);
    break;
  case MoneyOperationType.AccountTransfer:
    deleteAccountTransfer(operation.id);
    break;
  case MoneyOperationType.FundTransfer:
    deleteFundTransfer(operation.id);
    break;
  default:
    throw new Error('Unknown operation.');
  }
 
}
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
