<template>
  <div class="accountTransfers-view">
    <ListView header="Account Transfers" v-model="accountTransfers" :createNew="createAccountTransferObject"
      :save="createNewAccountTransfer" :update="updateAccountTransfer" :remove="deleteAccountTransfer"
      :allowAdd="accounts.length > 0">
      <template #content="{ data }">
        <div class="accountTransfers-view_body">
          <span class="date">{{ DisplayFormat.dateOnly(data.date) }}</span>
          <div class="accountTransfers-view_body-left">
            <span class="money">{{ DisplayFormat.money(data.value) }}</span>
            <span>{{ data.accountName }}</span>
          </div>
          <div class="accountTransfers-view_body-right">
            <span class="operation-title">{{ data.title }}</span>
            <span>{{ data.targetAccountName }}</span>
          </div>
        </div>
      </template>
      <template #editor="{ data }">
        <AccountTransferInput :accountTransfer="data" @changed="onAccountTransferChanged(data, $event)" />
      </template>
    </ListView>
  </div>
</template>
<script setup lang="ts">
import currencies from '@/assets/currencies.json';
import AccountTransferInput from '@/components/AccountTransferInput.vue';
import ListView from '@/components/ListView.vue';
import { DisplayFormat } from '@/helpers/display-format';
import { AccountTransfer } from '@/models/account-transfer';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';

const store = useAppStore();
const { createNewAccountTransfer, updateAccountTransfer, deleteAccountTransfer } = store;

const { accountTransfers, accounts } = storeToRefs(store);

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
  return {
    date: new Date(),
    sourceAccountId: store.accounts[0].id,
    targetAccountId: store.accounts[1].id,
    value: {
      currency: Object.keys(store.accounts[0].balance)[0] ?? Object.keys(currencies)[0]
    }
  };
}
</script>

<style lang="scss">
.accountTransfers-view {
  width: 100%;
  height: 100%;

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
