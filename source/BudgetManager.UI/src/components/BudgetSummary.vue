<template>
  <div class="budget-summary">
    <div 
      v-for="(account, index) in budget?.accounts" :key="account.id" 
    >
    {{index}}
    </div>
  </div>
</template>

<script setup lang="ts">
import { useAppStore } from '@/store/store';

const { budget } = useAppStore();

</script>

<style lang="scss">
.budget-summary {
  padding: 0.5rem;
  width: 100%;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
}

</style>
