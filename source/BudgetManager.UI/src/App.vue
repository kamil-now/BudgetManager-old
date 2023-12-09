<template>
  <ProgressSpinner
    v-if="isLoggedIn && isLoading"
    strokeWidth="8"
    animationDuration=".5s"
    aria-label="Loading indicator"
    class="loading-indicator"
  />
  <router-view />
</template>

<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { useAppStore } from './store/store';
import { AUTH, IAuthService } from '@/auth';
import { inject, onBeforeMount } from 'vue';

const auth = inject<IAuthService>(AUTH);
if (!auth) {
  throw new Error('No provider for IAuthService');
}
const store = useAppStore();

const { isLoggedIn, isLoading } = storeToRefs(store);
onBeforeMount(() => {
  if (!isLoggedIn.value) {
    auth.login();
  }
});
</script>

<style lang="scss">
#app {
  @media (max-width: map-get($breakpoints, xs)), (max-height: 300px) {
    * {
      display: none;
    }
  }

  &::before {
    display: none;
    content: "Your screen size is not supported";
    align-items: center;
    justify-content: center;
    height: 50%;

    @media (max-width: map-get($breakpoints, xs)), (max-height: 300px) {
      display: flex;
    }
  }
  height: calc(100vh - 1rem);
  margin: 0;
  display: flex;
  align-items: flex-start;
  justify-content: center;
  animation: fadein 1s;

  .loading-indicator {
    position: absolute;
    
    width: 2rem;
    height: 2rem;
    
    top: 0.25rem;
    right: 0.5rem;
  }
}

@keyframes fadein {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
</style>
