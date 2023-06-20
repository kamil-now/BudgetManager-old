<template>
  <div class="accounts-table">
    <ConfirmPopup></ConfirmPopup>
    <DynamicTable
      header="Accounts"
      v-model="accounts"
      :allowEdit="true"
      :allowReorder="true"
      :createNew="createAccountObject"
      :saveNew="createNewAccount"
      :update="updateAccount"
      :remove="deleteAccount"
      :onReorder="updateUserSettings"
    >
      <template #body="{ item }">
        <div class="accounts-table_body">
          <div class="accounts-table_body_name">{{ item.name }}</div>
          <div class="accounts-table_body_balance">{{ DisplayFormat.money(item.balance) }}</div>
        </div>
      </template>
      <template #editor="{ item, index }">
        <div class="accounts-table_editor">
          <AccountInput 
            :account="item" 
            @changed="onAccountChanged(item, $event)"
          />
          <Button 
            v-if="item.id && accounts.length > 1"
            icon="pi pi-times" 
            severity="danger" 
            text 
            rounded 
            aria-label="Remove" 
            @click="removeAt($event, index)" 
          />
        </div>
      </template>
    </DynamicTable>
  </div>
</template>
<script setup lang="ts">
import AccountInput from '@/components/AccountInput.vue';
import DynamicTable from '@/components/DynamicTable.vue';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';
import currencies from '@/assets/currencies.json';
import { DisplayFormat } from '@/helpers/display-format';
import { useConfirm } from 'primevue/useconfirm';
import { Account } from '@/models/account';

const confirm = useConfirm();

const store = useAppStore();
const { createNewAccount, updateAccount, deleteAccount, updateUserSettings } = store;

const { accounts } = storeToRefs(store);

function onAccountChanged(account: Account, newValue: Account) {
  account.name = newValue.name;
  account.balance = newValue.balance;
}

function createAccountObject() {
  return  {
    balance: {
      amount: 0,
      currency: getDefaultCurrency()
    }
  };
}

function removeAt(event: MouseEvent, index: number) {
  const account = accounts.value[index];
  confirm.require({
    target: event.target as HTMLElement,
    message: `Remove ${account.name}?`,
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    rejectClass: 'p-button-secondary',
    accept: () => deleteAccount(account)
  });
}

function getDefaultCurrency(): string {
  return accounts.value 
    ? accounts.value[accounts.value.length - 1].balance.currency
    : Object.keys(currencies)[0];
}

</script>

<style lang="scss">
.accounts-table {
  width: 100%;
  &_body {
    display: flex;
    width: 100%;
    &_name {
      width: 50%;
      text-align: right;
      padding-right: 1rem;
      display: inline-block;
      text-overflow: ellipsis;
      overflow: hidden;
    }
    &_balance {
      width: 50%;
      display: inline-block;
      text-overflow: ellipsis;
      overflow: hidden;
    }
  }
}
</style>
