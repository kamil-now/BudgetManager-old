<template>
  <div class="accounts-view">
    <ListView 
      header="Accounts"
      v-model="accounts"
      :allowReorder="true"
      :createNew="createAccountObject"
      :save="createNewAccount"
      :update="updateAccount"
      :remove="deleteAccount"
      :onReorder="updateUserSettings"
      :allowAdd="true"
    >
      <template #content="{ data }">
        <div class="accounts-view_body">
          <div class="accounts-view_body_balance money">{{ DisplayFormat.money(data.balance) }}</div>
          <div class="accounts-view_body_name">{{ data.name }}</div>
        </div>
      </template>
      <template #editor="{ data }">
        <AccountInput 
          :account="data" 
          @changed="onAccountChanged(data, $event)"
        />
      </template>
    </ListView>
  </div>
</template>
<script setup lang="ts">
import currencies from '@/assets/currencies.json';
import AccountInput from '@/components/AccountInput.vue';
import { DisplayFormat } from '@/helpers/display-format';
import ListView from '@/components/ListView.vue';
import { Account } from '@/models/account';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';

const store = useAppStore();
const { createNewAccount, updateAccount, deleteAccount, updateUserSettings } = store;

const { accounts } = storeToRefs(store);

function onAccountChanged(account: Account, newValue: Account) {
  account.name = newValue.name;
  account.balance = { ...newValue.balance };
}

function createAccountObject() {
  return  {
    balance: {
      amount: 0,
      currency: getDefaultCurrency() 
    }
  };
}


function getDefaultCurrency(): string {
  return accounts.value.length > 0
    ? accounts.value[accounts.value.length - 1].balance.currency
    : Object.keys(currencies)[0];
}

</script>

<style lang="scss">
.accounts-view {
  width: 100%;
  max-height: 100%;
  &_body {
    display: flex;
    width: 100%;
    &_name {
      width: 50%;
      text-align: left;
      padding-left: 1rem;
      display: inline-block;
      text-overflow: ellipsis;
      overflow: hidden;
    }
    &_balance {
      width: 50%;
      display: flex;
      justify-content: end;
      text-overflow: ellipsis;
      overflow: hidden;
    }
  }
}
</style>
