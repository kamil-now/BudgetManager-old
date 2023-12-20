<template>
  <div class="balance-label">
    <label>{{ label }}</label>
    <div
      class="money"
      v-for="(value, currency) in balance"
      :key="currency"
    >
      {{ DisplayFormat.money({ amount: value, currency: currency.toString() }) }}
    </div>
  </div>
</template>
<script setup lang="ts">
import { DisplayFormat } from '@/helpers/display-format';
import { Balance } from '@/models/balance';

defineProps<{ balance: Balance; label: string }>();
</script>

<style lang="scss">
.balance-label {
  padding: 0.5rem;
  display: flex;
  flex-direction: column;
  align-items: end;
  gap: 0.25rem;
  label {
    align-self: start;
    color: var(--text-color-secondary);
    text-transform: uppercase;
    font-size: 0.5rem;
    font-weight: bold;
  }
}
</style>
