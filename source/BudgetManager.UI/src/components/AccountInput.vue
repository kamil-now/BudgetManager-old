<template>
  <div class="account-input">
    <span class="p-float-label">
      <InputText
        id="accountName" 
        v-model="accountName" 
      />
      <label v-if="displayLabel" for="accountName">Account name</label>
    </span>
    <span class="p-float-label">
      <InputNumber 
        :readonly="!isNew"
        id="accountBalance"
        v-model="accountBalance" 
        :min="0"
        :maxFractionDigits="2"
        :max="1000000000"
      />
      <label v-if="displayLabel" for="accountBalance">{{ isNew ? 'Initial Balance' : 'Balance' }}</label>
    </span>
    <span class="p-float-label">
      <Dropdown 
        id="accountCurrency" 
        v-model="accountCurrency" 
        :options="currencyCodeList" 
      />
      <label v-if="displayLabel" for="accountCurrency">Currency</label>
    </span>
  </div>
</template>
<script setup lang="ts">
import { Account } from '@/models/account';
import { computed } from 'vue';
import currencies from '@/assets/currencies.json';

const props = defineProps<{
  account: Account,
  displayLabel?: boolean,
  isNew?: boolean
}>();
const emit = defineEmits(['changed']);

const currencyCodeList = Object.keys(currencies);
const accountName = computed({
  get: () => props.account.name,
  set: (newValue) => {
    emit('changed', {
      ...props.account, 
      name: newValue
    });
  }
});
const accountBalance = computed({
  get: () => props.account.balance.amount,
  set: (newValue) => {
    emit('changed', {
      ...props.account,
      balance: {
        ...props.account.balance,
        amount: newValue
      }
    });
  }
});
const accountCurrency = computed({
  get: () => props.account.balance.currency,
  set: (newValue) => {
    emit('changed', {
      ...props.account,
      balance: {
        ...props.account.balance,
        currency: newValue 
      }
    });
  }
});
</script>

<style lang="scss">
.account-input {
  display: flex;
  max-width: 100%;
  flex-wrap: wrap;
  gap: 1rem;
}
</style>
