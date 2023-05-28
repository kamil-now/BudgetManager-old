<template>
  <div class="budget-page">
    <CreateBudget v-if="newUser"/>
  </div>
</template>

<script setup lang="ts">
import CreateBudget from '@/components/CreateBudget.vue';
import axios, { AxiosError, AxiosResponse } from 'axios';
import { onMounted, ref } from 'vue';

const newUser = ref<boolean>(true); // TODO

onMounted(() => {
  axios.get<AxiosResponse<{[currency: string]: number}>>('/api/balance')
    .then(
      res => console.warn(res.data),
      (error: AxiosError<string[]>) => {
        if (error.response?.data.some(x => x.includes('does not exist'))) {
          newUser.value = true;
        } else {
          console.error(error);
        }
      }
    );
});
</script>

<style lang="scss">
.budget-page {
  display: flex;
  gap: 8px;
  flex-direction: column;
  flex-wrap: wrap;
  width: 450px;
  height: auto;
  align-items: center;
  background-color: var(--surface-section);
  border-radius: 1rem;
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
}
</style>
