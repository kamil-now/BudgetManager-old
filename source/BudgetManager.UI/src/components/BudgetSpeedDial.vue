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
import { MoneyOperationUtils } from '@/helpers/money-operation-utils';
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
    command: () => {
      const defaultAccount = store.accounts.filter((x) => !!x.id)[0];
      const defaultFund = store.funds.filter((x) => !!x.id)[0];
      edit({
        ...MoneyOperationUtils.createNew(MoneyOperationType.Expense),
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
    label: 'Add Income',
    icon: 'pi pi-plus',
    command: () => {
      const defaultAccount = store.accounts.filter((x) => !!x.id)[0];
      edit(
        {
          ...MoneyOperationUtils.createNew(MoneyOperationType.Income),
          accountId: defaultAccount.id,
          accountName: defaultAccount.name,
          value: {
            currency: Object.keys(defaultAccount.balance)[0],
            amount: 0,
          },
        }
      );
    },
  },
  {
    label: 'Add Allocation',
    icon: 'pi pi-file-import',
    command: () => {
      const defaultFund = store.funds.filter((x) => !!x.id)[0];
      edit({
        ...MoneyOperationUtils.createNew(MoneyOperationType.Allocation),
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
        ...MoneyOperationUtils.createNew(MoneyOperationType.CurrencyExchange),
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
    icon: 'pi pi-forward',
    command: () => {
      const defaultSourceFund = store.funds.filter((x) => !!x.id)[0];
      const defaultTargetFund = store.funds.filter((x) => !!x.id)[0];
      edit({
        ...MoneyOperationUtils.createNew(MoneyOperationType.FundTransfer),
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
    icon: 'pi pi-arrows-h',
    command: () => {
      const defaultSourceAccount = store.accounts.filter((x) => !!x.id)[0];
      const defaultTargetAccount = store.accounts.filter((x) => !!x.id)[0];
      edit({
        ...MoneyOperationUtils.createNew(MoneyOperationType.AccountTransfer),
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
