<template>
  <div class="currencyExchange-input">
    <Calendar v-model="currencyExchangeDate" dateFormat="yy/mm/dd" />
    <InputText
      class="p-inputtext-sm"
      placeholder="CurrencyExchange title"
      v-model="currencyExchangeTitle"
    />
    <Dropdown
      class="p-inputtext-sm"
      v-model="selectedAccount"
      :options="accounts" 
    >
      <template #value="{ value }">
        <span>{{ value?.name }}</span>
      </template>
      <template #option="{ option }">
        <span>{{ option?.name }}</span>
      </template>
    </Dropdown>
    <Dropdown
      class="p-inputtext-sm"
      id="selectedCurrency" 
      v-model="sourceCurrency" 
      :options="currencyCodeList" 
    />
    <InputNumber 
      class="p-inputtext-sm"
      v-model="currencyExchangeValue" 
      mode="currency"
      currencyDisplay="code"
      :highlightOnFocus="true"
      :allowEmpty="false"
      :currency="sourceCurrency" 
      :min="0"
      :maxFractionDigits="2"
      :max="1000000000"
    />
    <Dropdown
      class="p-inputtext-sm"
      id="selectedCurrency" 
      v-model="targetCurrency" 
      :options="currencyCodeList" 
    />
    <InputNumber 
      class="p-inputtext-sm"
      v-model="exchangeRate"
      :allowEmpty="false"
      :highlightOnFocus="true"
      :min="0.00001"
      :maxFractionDigits="5"
      :max="1000000000"
    />
  </div>
</template>
<script setup lang="ts">
import currencies from '@/assets/currencies.json';
import { DateUtils } from '@/helpers/date-utils';
import { Account } from '@/models/account';
import { CurrencyExchange } from '@/models/currency-exchange';
import { useAppStore } from '@/store/store';
import { computed, ref, watch } from 'vue';
const props = defineProps<{ currencyExchange: CurrencyExchange }>();
const emit = defineEmits(['changed']);
const { accounts }  = useAppStore();

const currencyCodeList = Object.keys(currencies);
const selectedAccount = ref<Account | undefined>(
  props.currencyExchange.accountId 
    ? accounts.find(x => x.id === props.currencyExchange.accountId)
    : undefined
);

watch(selectedAccount, async (account) => {
  emit('changed', {
    ...props.currencyExchange, 
    accountId: account?.id,
    accountName: account?.name
  });
});

const currencyExchangeDate = computed({
  get: () => props.currencyExchange.date,
  set: (newValue) => {
    emit('changed', {
      ...props.currencyExchange, 
      date: DateUtils.createDateOnlyString(new Date(newValue))
    });
  }
});
const currencyExchangeTitle = computed({
  get: () => props.currencyExchange.title,
  set: (newValue) => {
    emit('changed', {
      ...props.currencyExchange, 
      title: newValue
    });
  }
});
const currencyExchangeValue = computed({
  get: () => props.currencyExchange.value.amount,
  set: (newValue) => {
    emit('changed', {
      ...props.currencyExchange, 
      value: {
        ...props.currencyExchange.value,
        amount: newValue
      }
    });
  }
});
const sourceCurrency = computed({
  get: () => props.currencyExchange.value.currency,
  set: (newValue) => {
    emit('changed', {
      ...props.currencyExchange, 
      value: {
        ...props.currencyExchange.value,
        currency: newValue
      }
    });
  }
});

const targetCurrency = computed({
  get: () => props.currencyExchange.targetCurrency,
  set: (newValue) => {
    emit('changed', {
      ...props.currencyExchange, 
      targetCurrency: newValue,
    });
  }
});

const exchangeRate = computed({
  get: () => props.currencyExchange.exchangeRate,
  set: (newValue) => {
    emit('changed', {
      ...props.currencyExchange, 
      exchangeRate: newValue,
    });
  }
});
</script>

<style lang="scss">
.currencyExchange-input {
  display: flex;
  max-width: 100%;
  flex-wrap: wrap;
  gap: 1rem;
}
</style>
