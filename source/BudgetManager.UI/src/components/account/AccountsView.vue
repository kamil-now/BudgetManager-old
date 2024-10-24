<template>
  <div class="accounts-view">
    <ListView
      header="Accounts"
      v-model="accounts"
      :update="updateAccount"
      :remove="deleteAccount"
      :onReorder="reorderAccounts"
    >
      <template #content="{ data }">
        <div class="accounts-view_body">
          <div class="accounts-view_body_balance">
            <MoneySpan
              v-for="(value, currency) in data.balance"
              :key="currency"
              :amount="value"
              :currency="currency.toString()"
            />
          </div>
          <div class="accounts-view_body_name account-name">
            {{ data.name }}
          </div>
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
import AccountInput from '@/components/account/AccountInput.vue';
import ListView from '@/components/ListView.vue';
import MoneySpan from '@/components/MoneySpan.vue';
import { Account } from '@/models/account';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';

const store = useAppStore();
const { updateAccount, deleteAccount, reorderAccounts } = store;

const { accounts } = storeToRefs(store);

function onAccountChanged(account: Account, newValue: Account) {
  account.name = newValue.name;
  account.balance = { ...newValue.balance };
  account.initialBalance = { ...newValue.initialBalance };
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
      display: inline-block;
      text-overflow: ellipsis;
      overflow: hidden;

      > div {
        display: flex;
        justify-content: flex-end;
      }
    }
  }
}
</style>
