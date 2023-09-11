<template>
  <div class="settings-view">
    <Button 
      icon="pi pi-sign-out" 
      class="log-out-btn"
      text 
      rounded 
      aria-label="SignOut" 
      @click="signOut()" 
    />
  </div>
</template>

<script setup lang="ts">
import { inject } from 'vue';
import { useRouter } from 'vue-router';
import { AUTH, IAuthService } from '../auth';

const auth = inject<IAuthService>(AUTH);
const router = useRouter();

function signOut() {
  if (!auth) {
    throw new Error('No provider for IAuthService');
  }
  auth.logout().then(() => router.push('/login'));
}
</script>

<style lang="scss">
.settings-view {
  width: 100%;
  max-height: 100%;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  align-items: center;
  
  .log-out-btn {
    position: absolute;
    bottom: 50px;
    left: 50px;
    @include media-breakpoint(md, down) {
      bottom: 10px;
      left: 10px;
    }
  }
}
</style>
