<template>
  <div class="income-distribution-form-input">
    <span>Test income</span>
    <MoneyInput :money="testIncome" />
    <IncomeDistributionForm
      :income="testIncome"
      :incomeDistribution="incomeDistribution"
      @changed="onIncomeDistributionChange($event)"
    />
  </div>
</template>
<script setup lang="ts">
import currencies from '@/assets/currencies.json';
import MoneyInput from '@/components/MoneyInput.vue';
import IncomeDistributionForm from '@/components/IncomeDistributionForm.vue';
import { IncomeDistribution } from '@/models/income-distribution';
import { Money } from '@/models/money';
import { ref } from 'vue';

defineProps<{ incomeDistribution: IncomeDistribution }>();
const emit = defineEmits(['changed']);
const testIncome = ref<Money>({
  amount: 0,
  currency: Object.keys(currencies)[0],
});

function onIncomeDistributionChange(changed: IncomeDistribution) {
  emit('changed', changed);
}
</script>
<style lang="scss">
.income-distribution-form-input {
  display: flex;
  flex-direction: column;
  align-items: start;
  gap: 1rem;
}
</style>
