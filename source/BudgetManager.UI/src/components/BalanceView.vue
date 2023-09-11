<template>
  <div class="balance-view">
    <div class="balance-view_left">
      <span>Balance</span>
      <div class="money" v-for="(value, currency) in budgetBalance.balance" :key="currency">
        {{ DisplayFormat.money({ amount: value, currency: currency.toString() }) }}
      </div>
    </div>
    
    <div class="balance-view_right">
      <span>Unallocated</span>
      <div class="money" v-for="(value, currency) in budgetBalance.unallocated" :key="currency">
        {{ DisplayFormat.money({ amount: value, currency: currency.toString() }) }}
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { DisplayFormat } from '@/helpers/display-format';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';

const store = useAppStore();

const { budgetBalance } = storeToRefs(store);

</script>

<style lang="scss">
.balance-view {
  width: 100%;
  max-height: 100%;
  display: flex;
  &_left {
    width: 50%;
    display: flex;
    flex-direction: column;
    align-items: start;
  }
  &_right {
    width: 50%;
    display: flex;
    flex-direction: column;
    align-items: start;
  }
}
</style>
