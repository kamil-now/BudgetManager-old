<template>
  <div class="currency-exchanges-view">
    <ListView
      header="Currency Exchanges"
      v-model="currencyExchanges"
      :copy="copyCurrencyExchange"
      :createNew="createCurrencyExchangeObject"
      :save="createNewCurrencyExchange"
      :update="updateCurrencyExchange"
      :remove="deleteCurrencyExchange"
      :allowAdd="accounts.length > 0"
    >
      <template #content="{ data }">
        <div class="currency-exchanges-view_body">
          <span class="date">{{ DisplayFormat.dateOnly(data.date) }}</span>
          <div class="currency-exchanges-view_body-left">
            <span class="money">{{ DisplayFormat.money(data.value) }}</span>
            <span>{{ getAccountName(data.accountId) }}</span>
          </div>
          <div class="currency-exchanges-view_body-right">
            <span class="operation-title">{{ data.title }}</span>
            <span class="money">{{ DisplayFormat.money(getExchangeValue(data)) }}</span>
          </div>
        </div>
      </template>
      <template #editor="{ data }">
        <CurrencyExchangeInput 
          :currencyExchange="data" 
          @changed="onCurrencyExchangeChanged(data, $event)"
        />
      </template>
    </ListView>
  </div>
</template>
<script setup lang="ts">
import currencies from '@/assets/currencies.json';
import ListView from '@/components/ListView.vue';
import CurrencyExchangeInput from '@/components/CurrencyExchangeInput.vue';
import { DisplayFormat } from '@/helpers/display-format';
import { CurrencyExchange } from '@/models/currency-exchange';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';
import { Money } from '@/models/money';

const store = useAppStore();
const { createNewCurrencyExchange, updateCurrencyExchange, deleteCurrencyExchange } = store;

const { currencyExchanges, accounts } = storeToRefs(store);
function getExchangeValue(currencyExchange: CurrencyExchange): Money {
  return { amount: currencyExchange.value.amount / currencyExchange.exchangeRate, currency: currencyExchange.targetCurrency };
}
// TODO extend DTO instead
function getAccountName(accountId: string) {
  return accounts.value.find(x => x.id === accountId)?.name;
}

function onCurrencyExchangeChanged(currencyExchange: CurrencyExchange, newValue: CurrencyExchange) {
  currencyExchange.accountId = newValue.accountId;
  currencyExchange.targetCurrency = newValue.targetCurrency;
  currencyExchange.exchangeRate = newValue.exchangeRate;
  currencyExchange.createdDate = newValue.createdDate;
  currencyExchange.title = newValue.title;
  currencyExchange.value = newValue.value;
  currencyExchange.date = newValue.date;
  currencyExchange.description = newValue.description;
}

function createCurrencyExchangeObject() {
  const defaultAccount = store.accounts.filter(x => !!x.id)[0];
  const accountCurrencies = Object.keys(defaultAccount.initialBalance);
  const targetCurrency =  Object.keys(currencies).filter(x => !accountCurrencies.includes(x));
  return  {
    date: new Date(),
    accountId: defaultAccount.id,
    targetCurrency,
    exchangeRate: 1,
    value: { 
      currency: Object.keys(defaultAccount.balance)[0],
      amount: 0
    }
  };
}
function copyCurrencyExchange(currencyExchange: CurrencyExchange) {
  const copy =  { 
    ...currencyExchange,
    id: undefined
  };
  return copy;
}
</script>

<style lang="scss">
.currency-exchanges-view {
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
