<template>
  <div class="budget-summary">
    <FundsDataTable/>
    <AccountsDataTable/>
  </div>
</template>

<script setup lang="ts">
import AccountsDataTable from './AccountsDataTable.vue';
import FundsDataTable from './FundsDataTable.vue';
</script>

<style lang="scss">
.budget-summary {
  width: 100%;
  max-height: 100%;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  align-items: center;
  @include media-breakpoint('md') {
    justify-content: space-around;
  }
  @include media-breakpoint('lg') {
    flex-direction: row;
    align-items: start;
  }
}
</style>
