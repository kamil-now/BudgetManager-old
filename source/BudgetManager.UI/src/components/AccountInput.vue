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
  modelValue: Account,
  autofocus?: boolean
}>();
const emit = defineEmits(['update:modelValue']);
const accountName = computed({
  get: () => props.modelValue.name,
  set: (newValue) => {
    emit('update:modelValue', {
      ...props.modelValue, 
      name: newValue
    });
  }
});
const accountBalance = computed({
  get: () => props.modelValue.balance.amount,
  set: (newValue) => {
    emit('update:modelValue', {
      ...props.modelValue,
      balance: {
        ...props.modelValue.balance,
        amount: newValue
      }
    });
  }
});
const accountCurrency = computed({
  get: () => props.modelValue.balance.currency,
  set: (newValue) => {
    emit('update:modelValue', {
      ...props.modelValue,
      balance: {
        ...props.modelValue.balance,
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
