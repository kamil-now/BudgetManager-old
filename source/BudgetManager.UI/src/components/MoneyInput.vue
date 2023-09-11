<template>
  <div class="money-input">
    <InputNumber
      class="p-inputtext-sm"
      v-model="amount" 
      mode="currency"
      currencyDisplay="code"
      :allowEmpty="false"
      :currency="currency" 
      :min="0"
      :maxFractionDigits="2"
      :max="1000000000"
    />
  </div>
</template>
<script setup lang="ts">
import { Money } from '@/models/money';
import { computed } from 'vue';

const props = defineProps<{ money: Money }>();
const emit = defineEmits(['changed']);

const amount = computed({
  get: () => props.money.amount,
  set: (newValue) => {
    emit('changed', {
      ...props.money,
      amount: newValue
    });
  }
});
const currency = computed({
  get: () => props.money.currency,
  set: (newValue) => {
    emit('changed', {
      ...props.money,
      currency: newValue
    });
  }
});
</script>

<style lang="scss">
.money-input {
  display: flex;
  max-width: 100%;
  width: 100%;
  flex-wrap: wrap;
  gap: 1rem;
}
</style>
