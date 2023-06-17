<template>
  <div class="account-input">
    <span class="p-float-label">
      <InputText
        class="p-inputtext-sm"
        id="accountName" 
        placeholder="Account name"
        v-model="accountName" 
      />
      <label v-if="displayLabel" for="accountName">Account name</label>
    </span>
    <span class="p-float-label">
      <InputNumber 
        class="p-inputtext-sm"
        :readonly="!isNew"
        id="accountBalance"
        v-model="accountBalance" 
        mode="currency"
        currencyDisplay="code"
        :allowEmpty="false"
        :currency="accountCurrency" 
        :min="0"
        :maxFractionDigits="2"
        :max="1000000000"
      />
      <label v-if="displayLabel" for="accountBalance">{{ isNew ? 'Initial Balance' : 'Balance' }}</label>
    </span>
    <span class="p-float-label">
      <Dropdown 
        class="p-inputtext-sm"
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
}>();
const emit = defineEmits(['changed']);

const currencyCodeList = Object.keys(currencies);
const isNew = computed(() => !props.account.id);
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
