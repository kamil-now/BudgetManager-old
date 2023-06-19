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
  max-height: 100%;
  display: flex;
  gap: 8px;
  flex-direction: column;
  flex-wrap: wrap;
  
  overflow: hidden;
  align-items: center;

  height: auto;
  width: map-get($breakpoints, 'xs');
  margin-top: 3rem;
  @extend .card;
  @include media-breakpoint('sm', 'down') {
    margin: 0;
    border-radius: 0;
    box-shadow: 0;
  }
  @include media-breakpoint('lg') {
    width: map-get($breakpoints, 'md')
  }
}
</style>
