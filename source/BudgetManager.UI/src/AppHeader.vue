<template>
  <div
    class="app-header"
    ref="header"
  >
    <ProgressSpinner
      strokeWidth="8"
      animationDuration=".5s"
      aria-label="Loading indicator"
      class="loading-indicator"
      :class="{ show: isLoading }"
    />
    <div class="app-header-content">
      <BalanceLabel
        :balance="balance"
        label="Balance"
      />
      <BalanceLabel
        :balance="unallocated"
        label="Unallocated"
      />
    </div>
    <Button
      class="app-header-menu-toggle"
      @click="toggle()"
    >
      <i class="pi pi-bars"></i>
    </Button>

    <div
      class="app-header-menu-content"
      :class="{ show: isOpen }"
      @click="toggle()"
    >
      <Button @click="createNewAccount()">
        <AccountIcon />
        add account
      </Button>
      <Button @click="createNewFund()">
        <FundIcon />
        add fund
      </Button>
      <Button @click="createNew(MoneyOperationType.CurrencyExchange)">
        <CurrencyExchangeIcon />
        add currency exchange
      </Button>
      <Button @click="createNew(MoneyOperationType.AccountTransfer)">
        <AccountTransferIcon />
        add account transfer
      </Button>
      <Button @click="createNew(MoneyOperationType.FundTransfer)">
        <FundTransferIcon />
        add fund transfer
      </Button>
      <Button @click="createNew(MoneyOperationType.Allocation)">
        <AllocationIcon />
        add allocation
      </Button>
      <Button @click="createNew(MoneyOperationType.Income)">
        <IncomeIcon />
        add income
      </Button>
      <Button @click="createNew(MoneyOperationType.Expense)">
        <ExpenseIcon />
        add expense
      </Button>
      <ThemeToggle />
    </div>
  </div>
</template>

<script setup lang="ts">
import BalanceLabel from '@/components/BalanceLabel.vue';
import currencies from '@/assets/currencies.json';
import { useAppStore } from './store/store';
import ThemeToggle from './components/ThemeToggle.vue';
import { ref } from 'vue';
import { useDialog } from 'primevue/usedialog';
import InputDialog from './components/InputDialog.vue';
import AccountTransferIcon from './components/icons/AccountTransferIcon.vue';
import FundTransferIcon from './components/icons/FundTransferIcon.vue';
import IncomeIcon from './components/icons/IncomeIcon.vue';
import ExpenseIcon from './components/icons/ExpenseIcon.vue';
import AllocationIcon from './components/icons/AllocationIcon.vue';
import CurrencyExchangeIcon from './components/icons/CurrencyExchangeIcon.vue';
import FundIcon from './components/icons/FundIcon.vue';
import AccountIcon from './components/icons/AccountIcon.vue';
import { MoneyOperationType } from './models/money-operation-type.enum';
import { MoneyOperationFactory } from './helpers/money-operation-factory';
import { StringUtils } from './helpers/string-utils';
import { storeToRefs } from 'pinia';

const dialog = useDialog();

const store = useAppStore();
const { isLoading, balance, unallocated } = storeToRefs(store);

const isOpen = ref(false);
const header = ref<HTMLDivElement>();

function toggle() {
  isOpen.value = !isOpen.value;
}
function createNew(type: MoneyOperationType) {
  const operation = MoneyOperationFactory.create(store, type);

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

function createNewFund() {
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
}

function createNewAccount() {
  let defaultCurrency = Object.keys(currencies)[0];
  const acc = store.accounts[store.accounts.length - 1];
  if (acc && acc.initialBalance && Object.keys(acc.initialBalance).length > 0) {
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
}
</script>
<style lang="scss">
.app-header {
  width: 100%;
  max-height: 88px;
  overflow: visible;
  z-index: 10;

  position: -webkit-sticky;
  position: sticky;
  background-color: var(--surface-0);

  top: 0;
  padding: 0;

  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;

  &-content {
    // background-color: var(--surface-card);
    display: flex;
    flex-grow: 1;
    align-items: center;
    justify-content: center;
    padding: 0.5rem;
    gap: 1rem;
    overflow: hidden;
  }

  &-menu {
    &-toggle {
      display: flex;
      align-items: center;
      justify-content: center;
      cursor: pointer;
      color: var(--text-color);

      $size: 1.5rem !important;
      min-width: $size;
      min-height: $size;
      width: $size;
      height: $size;
      font-size: $size;
      line-height: $size;
      padding: 0.5rem;
      border-radius: 0.25rem;
      background-color: transparent;
      border: 1px solid transparent;

      &-icon {
        width: $size;
        height: $size;
        display: inline-block;
        border-radius: 0.25rem;
      }
    }
    &-content {
      display: flex;
      justify-content: flex-end;
      align-items: end;
      flex-basis: 100%;
      flex-grow: 1;
      max-height: 0;
      width: 100%;
      overflow: hidden;
      flex-direction: column;
      transition: all 0.5s ease-in-out;
      z-index: 2;
      &.show {
        max-height: 500px;
      }
      button {
        font-size: 0.5rem;
        text-transform: uppercase;
        font-weight: bold;
        letter-spacing: 0.03rem;

        display: flex;
        align-items: center;
        justify-content: center;
        gap: 0.5rem;
        margin: 0.25rem;

        color: #ffffffde;
        background-color: #036f6f;

        border-color: var(--text-color-secondary);
        box-shadow: none;
        &:focus {
          box-shadow: none;
          border-color: var(--text-color-secondary);
        }
      }
    }
  }

  .loading-indicator {
    visibility: hidden;
    margin: 0.5rem;

    width: 1rem;
    height: 1rem;

    &.show {
      visibility: visible;
    }
  }
}
</style>
