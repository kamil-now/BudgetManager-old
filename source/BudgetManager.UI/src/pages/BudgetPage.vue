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
import axios from 'axios';
import { inject, onMounted } from 'vue';
import { MSAL, MsalAuthService } from '../auth';

const msal = inject<MsalAuthService>(MSAL);

onMounted(() => {
  axios.post<void>('/api/budget')
    .then( 
      () => console.warn('OK'),
      error => console.error(error)
    );
    
});
async function logout() {
  await msal?.logout();
}
</script>

<style lang="scss">
.budget-page {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
}
</style>
