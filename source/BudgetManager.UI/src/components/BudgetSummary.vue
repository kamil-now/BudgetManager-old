<template>
  <div class="budget-summary">
    <FundsTable />
    <AccountsTable/>
  </div>
</template>

<script setup lang="ts">
import AccountsTable from './AccountsTable.vue';
import FundsTable from './FundsTable.vue';
</script>

<style lang="scss">
.budget-summary {
  width: 100%;
  max-height: 100%;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  align-items: center;
}
</style>
