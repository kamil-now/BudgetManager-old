<template>
  <ProgressSpinner v-if="isLoggedIn && isLoading"/>
  <router-view v-else/>
</template>

<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { inject, ref } from 'vue';
import { useRouter } from 'vue-router';
import { AUTH, IAuthService } from './auth';
import { useAppStore } from './store/store';

const auth = inject<IAuthService>(AUTH);
const store = useAppStore();
const { isLoggedIn, isLoading } = storeToRefs(store);
const router = useRouter();

function signOut() {
  if (!auth) {
    throw new Error('No provider for IAuthService');
  }
  auth.logout().then(() => router.push('/login'));
}
</script>

<style lang="scss">
#app {
  @media (max-width: map-get($breakpoints, xs)), (max-height: 500px) {
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
      
    @media (max-width: map-get($breakpoints, xs)), (max-height: 500px) {
      display: flex;
    }
  }
  height: calc(100vh - 4rem);
  margin: 0;
  display: flex;
  align-items: flex-start;
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
