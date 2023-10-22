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
            <div
              class="money"
              v-for="(value, currency) in data.balance"
              :key="currency"
            >
              {{
                DisplayFormat.money({
                  amount: value,
                  currency: currency,
                })
              }}
            </div>
          </div>
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
import AccountInput from '@/components/AccountInput.vue';
import ListView from '@/components/ListView.vue';
import { DisplayFormat } from '@/helpers/display-format';
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
        justify-content: end;
      }
    }
  }
}
</style>
