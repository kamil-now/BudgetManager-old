<template>
  <div class="budget-page">
    <CreateBudget v-if="isNewUser"/>
    <BudgetSummary v-else />
  </div>
</template>

<script setup lang="ts">
import BudgetSummary from '@/components/BudgetSummary.vue';
import CreateBudget from '@/components/CreateBudget.vue';
import { useAppStore } from '@/store/store';

const { isNewUser } = useAppStore();

</script>

<style lang="scss">
.budget-page {
  display: flex;
  gap: 8px;
  flex-direction: column;
  flex-wrap: wrap;
  width: map-get($breakpoints, xs);
  height: auto;
  max-height: 100%;
  overflow: hidden;
  align-items: center;
  background-color: var(--surface-section);
  border-radius: 1rem;
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
}
</style>
