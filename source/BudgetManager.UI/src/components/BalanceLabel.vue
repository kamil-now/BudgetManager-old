<template>
  <div class="balance-label">
    <label v-if="label">{{ label }}</label>
    <div class="balance-label-content">
      <div
        class="money"
        v-if="isEmpty"
      >
        0
      </div>

      <MoneySpan
        v-else
        v-for="(value, currency, index) in balance"
        :key="currency"
        :class="{
          'large-font': largeFont,
          alternate: !useColors && index % 2 !== 0,
          positive: useColors && value > 0,
          negative: useColors && value < 0,
        }"
        :amount="value"
        :currency="currency.toString()"
        :addSign="useColors"
      />
    </div>
  </div>
</template>
<script setup lang="ts">
import MoneySpan from '@/components/MoneySpan.vue';
import { Balance } from '@/models/balance';
import { computed } from 'vue';

const props = defineProps<{
  balance: Balance;
  label?: string;
  largeFont?: boolean;
  useColors?: boolean;
}>();
const isEmpty = computed(() => Object.keys(props.balance).length === 0);
</script>

<style lang="scss">
.balance-label {
  height: calc($appHeaderHeight - 0.5rem);
  padding: 0.25rem;
  display: flex;
  flex-direction: column;
  align-items: start;
  overflow: hidden;

  label {
    @extend .label;
  }
  &-content {
    display: flex;
    flex-direction: column;
    align-items: end;
    flex-wrap: wrap;
    max-height: 100%;
    .money {
      padding: 0.25rem;
      &.alternate {
        color: var(--text-color-secondary);
      }
      &.positive {
        color: limegreen;
      }
      &.negative {
        color: crimson;
      }
      &.large-font {
        font-size: 1.25rem;
      }
    }
  }
}
</style>
