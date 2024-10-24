<template>
  <div class="income-allocation-form-input">
    <div class="income-allocation-form-input_income">
      <span>Test income</span>
      <MoneyInput :money="testIncome" @changed="onIncomeChange($event)" />
    </div>
    <IncomeAllocationForm
      :income="testIncome"
      :incomeAllocation="incomeAllocation"
      @changed="onIncomeAllocationChange($event)"
    />
  </div>
</template>
<script setup lang="ts">
import currencies from '@/assets/currencies.json';
import MoneyInput from '@/components/money-operations/MoneyInput.vue';
import IncomeAllocationForm from '@/components/money-operations/IncomeAllocationForm.vue';
import { IncomeAllocation } from '@/models/income-allocation';
import { Money } from '@/models/money';
import { ref } from 'vue';

defineProps<{ incomeAllocation: IncomeAllocation }>();
const emit = defineEmits(['changed']);
const testIncome = ref<Money>({
  amount: 0,
  currency: Object.keys(currencies)[0],
});

function onIncomeAllocationChange(changed: IncomeAllocation) {
  emit('changed', changed);
}

function onIncomeChange(money: Money) {
  testIncome.value = money;
}

</script>
<style lang="scss">
.income-allocation-form-input {
  display: flex;
  flex-direction: column;
  align-items: start;
  gap: 1rem;

  &_income {
    display: flex;
    align-items: center;
    gap: 1rem;
  }
}
</style>
