<template>
  <div class="login-page">
    <button @click="login()" class="log-in-btn">Log-in</button>
  </div>
</template>

<script setup lang="ts">
import { MSAL, MsalAuthService } from '@/auth';
import { inject } from 'vue';
const msal = inject<MsalAuthService>(MSAL);

function login() {
  msal?.login();
}
</script>

<style lang="scss">
.login-page {
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>
