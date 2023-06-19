<template>
  <div class="budget-summary">
    <FundsDataTable/>
    <AccountsDataTable/>
    <IncomesDataTable/>
  </div>
</template>

<script setup lang="ts">
import AccountsDataTable from './AccountsDataTable.vue';
import FundsDataTable from './FundsDataTable.vue';
import IncomesDataTable from './IncomesDataTable.vue';
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
}
</style>
