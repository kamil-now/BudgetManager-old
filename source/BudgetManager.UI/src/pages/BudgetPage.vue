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
import { inject } from 'vue';
import { MSAL, MsalAuthService } from '../plugins/msal-plugin';


const msal = inject<MsalAuthService>(MSAL);
axios.post<void>('/budget');

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
