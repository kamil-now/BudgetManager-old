<template>
  <div class="account-input">
    <span class="p-float-label input-text">
      <InputText
        id="accountName" 
        v-model="accountName" 
      />
      <label v-if="displayLabel" for="accountName">Account name</label>
    </span>
    <span class="p-float-label dropdown-currency-code">
      <Dropdown 
        id="accountCurrency" 
        v-model="accountCurrency" 
        :options="currencyCodeList" 
      />
      <label v-if="displayLabel" for="accountCurrency">Currency</label>
    </span>
    <span class="p-float-label input-number">
      <InputNumber 
        :readonly="!isNew"
        id="accountBalance"
        v-model="accountBalance" 
        :min="0"
        :maxFractionDigits="2"
        :max="1000000000"
      />
      <label v-if="displayLabel" for="accountBalance">Balance</label>
    </span>
  </div>
</template>
<script setup lang="ts">
import { Account } from '@/models/account';
import { computed } from 'vue';
import currencies from '@/assets/currencies.json';

const props = withDefaults(defineProps<{
  account: Account,
  displayLabel?: boolean,
  autofocus?: boolean,
  isNew?: boolean
}>(),
{
  displayLabel: true
});
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
}
</style>
