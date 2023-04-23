<template>
  <div class="budget-page">
    <SpendingFundCard />
    <teleport to="#app">
      <button class="log-out-btn" @click="logout()">Log-out</button>
    </teleport>
  </div>
</template>

<script setup lang="ts">
import SpendingFundCard from '@/components/SpendingFundCard.vue';
import axios, { AxiosError, AxiosResponse } from 'axios';
import { inject, onMounted } from 'vue';
import { AUTH, IAuthService } from '../auth';

const auth = inject<IAuthService>(AUTH);

onMounted(() => {
  axios.get<AxiosResponse<{[currency: string]: number}>>('/api/balance')
    .then( 
      res => console.warn(res.data),
      (error: AxiosError) => console.error(error)
    );
    
});
async function logout() {
  await auth?.logout();
}
</script>

<style lang="scss">
.budget-page {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
}
</style>
