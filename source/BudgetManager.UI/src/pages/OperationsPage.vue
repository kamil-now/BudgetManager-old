<template>
  <div class="operations-page">
    <OperationsList></OperationsList>
  </div>
</template>

<script setup lang="ts">
import OperationsList from '@/components/OperationsList.vue';
</script>

<style lang="scss">
.operations-page {
  height: 100%;
  width: 100%;
  margin: 1rem;
  padding: 0.5rem;
  @extend .card;
  max-height: 100%;
  display: flex;
  flex-direction: column;
  flex-wrap: nowrap;

  overflow: hidden;
}</style>
