<template>
  <div class="money-input">
    <Dropdown
      class="p-inputtext-sm"
      id="selectedCurrency"
      v-model="currency"
      :options="currencyCodeList"
    />
    <InputNumber
      class="p-inputtext-sm"
      v-model="amount" 
      @input="onAmountInput($event)"
      mode="currency"
      currencyDisplay="code"
      :allowEmpty="false"
      :highlightOnFocus="true"
      :currency="currency" 
      :min="0"
      :maxFractionDigits="2"
      :max="1000000000"
    />
  </div>
</template>
<script setup lang="ts">
import currencies from '@/assets/currencies.json';
import { Money } from '@/models/money';
import { InputNumberInputEvent } from 'primevue/inputnumber';
import { computed } from 'vue';

const currencyCodeList = Object.keys(currencies);
const props = defineProps<{ money: Money }>();
const emit = defineEmits(['changed']);

const amount = computed({
  get: () => props.money.amount,
  set: () => {} // handled on @input instead
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
function onAmountInput(event: InputNumberInputEvent) {
  amount.value = Number(event.value);
  emit('changed', {
    ...props.money,
    amount: Number(event.value)
  });
}
</script>

<style lang="scss">
.money-input {
  display: flex;
  max-width: 100%;
  flex-wrap: wrap;
  gap: 1rem;
}
</style>
