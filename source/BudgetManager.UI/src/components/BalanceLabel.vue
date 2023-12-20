<template>
  <div class="balance-label">
    <label>{{ label }}</label>
    <div class="money" v-if="isEmpty">0</div>
    <div
      v-else
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
import { computed } from 'vue';

const props = defineProps<{ balance: Balance; label: string }>();
const isEmpty = computed(() => Object.keys(props.balance).length === 0);
</script>

<style lang="scss">
.balance-label {
  height: 80px;
  padding: 0.5rem;
  display: flex;
  flex-direction: column;
  align-items: end;
  gap: 0.25rem;
  flex-wrap: wrap;
  label {
    align-self: start;
    color: var(--text-color-secondary);
    text-transform: uppercase;
    font-size: 0.5rem;
    font-weight: bold;
  }
}
</style>
