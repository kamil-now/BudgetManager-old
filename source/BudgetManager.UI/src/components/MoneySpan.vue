<template>
  <span class="money">
    {{ moneyDisplayValue }}
  </span>
</template>
<script setup lang="ts">
import { Money } from '@/models/money';
import { computed } from 'vue';
import { DisplayFormat } from '@/helpers/display-format';

const props = defineProps<{
  money?: Money;
  amount?: number;
  currency?: string;
  addSign?: boolean;
}>();
const moneyDisplayValue = computed(() =>
  DisplayFormat.money(
    props.money ?? { amount: props.amount!, currency: props.currency! },
    props.addSign
  )
);
</script>
