<template>
  <div class="balance-label">
    <label>{{ label }}</label>
    <div class="balance-label-content">
      <div class="money" v-if="isEmpty">0</div>
        <span
          v-else
          class="money"
          v-for="(value, currency, index) in balance"
          :key="currency"
          :class="{ alternate: index % 2 !== 0 }"
          >
          {{ DisplayFormat.money({ amount: value, currency: currency.toString() }) }}
        </span>
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
      font-size: 1.25rem;
      &.alternate {
        color: var(--text-color-secondary);
      }
    }
  }
}
</style>
