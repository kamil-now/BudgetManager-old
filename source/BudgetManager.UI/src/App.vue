<template>
  <AppHeader v-if="isLoggedIn" />
  <router-view />
</template>

<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { useAppStore } from './store/store';
import { AUTH, IAuthService } from '@/auth';
import { inject, onBeforeMount } from 'vue';
import AppHeader from './AppHeader.vue';

const auth = inject<IAuthService>(AUTH);
if (!auth) {
  throw new Error('No provider for IAuthService');
}
const store = useAppStore();

const { isLoggedIn } = storeToRefs(store);
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
  height: calc(100vh);
  width: calc(100vw - 1rem);
  > * {
    max-width: 1400px;
  }
  margin: 0;
  display: flex;
  align-items: center;
  flex-direction: column;
  justify-content: center;
  animation: fadein 1s;
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
