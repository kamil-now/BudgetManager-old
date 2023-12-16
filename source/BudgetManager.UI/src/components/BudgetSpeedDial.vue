<template>
  <SpeedDial
    :model="items"
    direction="up"
    :style="{ right: '2rem', bottom: '2rem' }"
    :tooltipOptions="{ position: 'left', event: 'hover' }"
  />
</template>
<script setup lang="ts">
import currencies from '@/assets/currencies.json';
import { MoneyOperationFactory } from '@/helpers/money-operation-factory';
import { StringUtils } from '@/helpers/string-utils';
import { MoneyOperation } from '@/models/money-operation';
import { MoneyOperationType } from '@/models/money-operation-type.enum';
import { useAppStore } from '@/store/store';
import { useDialog } from 'primevue/usedialog';
import { ref } from 'vue';
import InputDialog from './InputDialog.vue';
const dialog = useDialog();
const store = useAppStore();

const items = ref([
  {
    label: 'Add Expense',
    icon: 'pi pi-minus',
    command: () => edit(MoneyOperationFactory.create(store, MoneyOperationType.Expense)),
  },
  {
    label: 'Add Income',
    icon: 'pi pi-plus',
    command: () => edit(MoneyOperationFactory.create(store, MoneyOperationType.Income)),
  },
  {
    label: 'Add Allocation',
    icon: 'pi pi-file-import',
    command: () => edit(MoneyOperationFactory.create(store, MoneyOperationType.Allocation)),
  },
  {
    label: 'Add Currency Exchange',
    icon: 'pi pi-arrow-right-arrow-left',
    command: () => edit(MoneyOperationFactory.create(store, MoneyOperationType.CurrencyExchange)),
  },
  {
    label: 'Add Fund Transfer',
    icon: 'pi pi-forward',
    command: () => edit(MoneyOperationFactory.create(store, MoneyOperationType.FundTransfer)),
  },
  {
    label: 'Add Account Transfer',
    icon: 'pi pi-arrows-h',
    command: () => edit(MoneyOperationFactory.create(store, MoneyOperationType.AccountTransfer)),
  },
  {
    label: 'Add Fund',
    icon: 'pi pi-file',
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
    icon: 'pi pi-dollar',
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

function edit(operation: MoneyOperation) {
  dialog.open(InputDialog, {
    data: {
      operation,
    },
    props: {
      header: `Create new ${StringUtils.camelCaseToWords(
        MoneyOperationType[operation.type]
      )}`,
      modal: true,
      closable: false,
    },
  });
}
</script>

<style lang="scss">
.p-speeddial-action {
  text-decoration: none;
}
</style>
