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
import { useAppStore } from '@/store/store';
import { inject, onBeforeMount } from 'vue';
import { useRouter } from 'vue-router';
const auth = inject<IAuthService>(AUTH);
if (!auth) {
  throw new Error('No provider for IAuthService');
}
const router = useRouter();
const store = useAppStore();
const login = () => auth.login().then(() => router.push('/home'));

onBeforeMount(() => {
  if (store.isLoggedIn) {
    router.push('/home');
  }
});

</script>

<style lang="scss">
.login-page {
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;

  .log-in-btn {
    min-width: 100px;
    border-radius: 5px;
    font-weight: 500;
    font-size: 12px;
  }
}
</style>
