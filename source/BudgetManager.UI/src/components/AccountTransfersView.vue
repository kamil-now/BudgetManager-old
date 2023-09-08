<template>
  <div class="accountTransfers-view">
    <ListView
      header="Account Transfers"
      v-model="accountTransfers"
      :createNew="createAccountTransferObject"
      :save="createNewAccountTransfer"
      :update="updateAccountTransfer"
      :remove="deleteAccountTransfer"
      :onReorder="updateUserSettings"
      :allowAdd="accounts.length > 0 && accounts.length > 0"
    >
      <template #content="{ data }">
        <div class="accountTransfers-view_body">
          <span class="date">{{ data.date }}</span>
          <div class="accountTransfers-view_body-left">
            <span class="money">{{ DisplayFormat.money(data.value) }}</span>
            <span>{{ getAccountName(data.sourceAccountId) }}</span>
          </div>
          <div class="accountTransfers-view_body-right">
            <span class="operation-title">{{ data.title }}</span>
            <span>{{ getAccountName(data.targetAccountId) }}</span>
          </div>
        </div>
      </template>
      <template #editor="{ data }">
        <AccountTransferInput 
          :accountTransfer="data" 
          @changed="onAccountTransferChanged(data, $event)"
        />
      </template>
    </ListView>
  </div>
</template>
<script setup lang="ts">
import ListView from '@/components/ListView.vue';
import AccountTransferInput from '@/components/AccountTransferInput.vue';
import { DisplayFormat } from '@/helpers/display-format';
import { AccountTransfer } from '@/models/account-transfer';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';

const store = useAppStore();
const { createNewAccountTransfer, updateAccountTransfer, deleteAccountTransfer, updateUserSettings } = store;

const { accountTransfers, accounts } = storeToRefs(store);
// TODO extend DTO instead
function getAccountName(accountId: string) {
  return accounts.value.find(x => x.id === accountId)?.name;
}

function onAccountTransferChanged(accountTransfer: AccountTransfer, newValue: AccountTransfer) {
  accountTransfer.sourceAccountId = newValue.sourceAccountId;
  accountTransfer.targetAccountId = newValue.targetAccountId;
  accountTransfer.createdDate = newValue.createdDate;
  accountTransfer.title = newValue.title;
  accountTransfer.value = newValue.value;
  accountTransfer.date = newValue.date;
  accountTransfer.description = newValue.description;
}

function createAccountTransferObject() {
  const defaultSourceAccount = store.accounts[0];
  const defaultTargetAccount = store.accounts.filter(x => x.balance.currency === defaultSourceAccount.balance.currency)[0];
  return  {
    date: new Date().toDateString(),
    sourceAccountId: defaultSourceAccount.id,
    targetAccountId: defaultTargetAccount.id,
    value: { 
      currency: defaultSourceAccount.balance.currency
    }
  };
}
</script>

<style lang="scss">
.accountTransfers-view {
  width: 100%;
  &_body {
    display: flex;
    width: 100%;
    align-items: center;
    span {
      display: inline-block;
      text-overflow: ellipsis;
      overflow: hidden;
    }
    &-left {
      width: calc(50% - #{$date-width});
      display: flex;
      flex-direction: column;
      align-items: end;
      span {
        text-align: right;
      }
    }
    &-right {
      width: calc(50% - #{$date-width});
      display: flex;
      flex-direction: column;
      align-items: start;
      span {
        text-align: left;
        padding-left: 1rem;
      }
    }
  }
  &_editor {
    display: flex;
  }
}
</style>
