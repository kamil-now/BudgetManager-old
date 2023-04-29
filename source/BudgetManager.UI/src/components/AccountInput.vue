<template>
  <div class="edit-account">
    <InputField
      class="name-input"
      type="text"
      v-model="accountName"
      label="Account name"
      :autofocus="autofocus"
    />
    <InputField
      :readonly="!isNew"
      class="balance-input"
      type="number"
      min="0"
      v-model="accountBalance"
      label="Balance"
    />
    <InputField
      class="currency-input"
      type="text"
      v-model="accountCurrency"
      label="Currency"
    />
  </div>
</template>
<script setup lang="ts">
import InputField from '@/components/InputField.vue';
import { Account } from '@/models/account';
import { computed } from 'vue';
const props = defineProps<{
  account: Account,
  autofocus?: boolean,
  isNew?: boolean
}>();
const emit = defineEmits(['changed']);
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
.edit-account {
  display: flex;
  flex-direction: row;
  gap: 16px;
}
.name-input {
  width: 150px;
}
.balance-input {
  width: 100px;
}
.currency-input {
  width: 50px;
}
</style>
