<template>
  <div class="login-page">
    <Button 
      @click="login()" 
      class="log-in-btn" 
      icon="pi pi-sign-in"
      label="log-in"
    />
  </div>
</template>

<script setup lang="ts">
import { AUTH, IAuthService } from '@/auth';
import { inject } from 'vue';
import { useRouter } from 'vue-router';
const auth = inject<IAuthService>(AUTH);
if (!auth) {
  throw new Error('No provider for IAuthService');
}
const router = useRouter();
const login = () => auth.login().then(() => router.push('/home'));

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
