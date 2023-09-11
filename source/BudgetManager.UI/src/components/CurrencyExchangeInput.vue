<template>
  <div class="currencyExchange-input">
    <Calendar v-model="currencyExchangeDate" dateFormat="dd/mm/yy" />
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
    <InputNumber 
      class="p-inputtext-sm"
      v-model="currencyExchangeValue" 
      mode="currency"
      currencyDisplay="code"
      :allowEmpty="false"
      :currency="sourceCurrency" 
      :min="0"
      :maxFractionDigits="2"
      :max="1000000000"
    />
    <Dropdown
      class="p-inputtext-sm"
      id="selectedCurrency" 
      v-model="sourceCurrency" 
      :options="currencyCodeList" 
    />
    <InputNumber 
      class="p-inputtext-sm"
      v-model="exchangeRate"
      :allowEmpty="false"
      :min="0.00001"
      :maxFractionDigits="5"
      :max="1000000000"
    />
    <Dropdown
      class="p-inputtext-sm"
      id="selectedCurrency" 
      v-model="targetCurrency" 
      :options="currencyCodeList" 
    />
  </div>
</template>
<script setup lang="ts">
import currencies from '@/assets/currencies.json';
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
  });
});

const currencyExchangeDate = computed({
  get: () => props.currencyExchange.date,
  set: (newValue) => {
    emit('changed', {
      ...props.currencyExchange, 
      date: new Date(newValue)
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
