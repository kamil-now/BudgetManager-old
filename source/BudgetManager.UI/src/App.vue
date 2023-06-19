<template>
  <ProgressSpinner v-if="isLoggedIn && isLoading"/>
  <router-view v-else/>
  <SpeedDial 
    v-if="isLoggedIn"
    class="menu"
    :model="items" 
    direction="up" 
    :transitionDelay="80" 
    showIcon="pi pi-bars" 
    hideIcon="pi pi-times" 
  />
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
if (!auth) {
  throw new Error('No provider for IAuthService');
}
const items = ref([
  {
    label: 'Sign-out',
    icon: 'pi pi-sign-out',
    command: () =>  auth.logout().then(() => router.push('/login'))
  },
]);
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
.log-in-btn {
  min-width: 100px;
  background-color: var(--app-accent-color);
  color: var(--app-accent-text-color);
  border-radius: 5px;
  font-weight: 500;
  font-size: 12px;
}
.menu {
  position: absolute;
  bottom: 50px;
  right: 50px;
  @include media-breakpoint(md, down) {
    top: 10px;
    right: 10px;
  }
}
.log-out-btn {
  @extend .log-in-btn;
  position: absolute;
  bottom: 50px;
  right: 50px;
  @include media-breakpoint(md, down) {
  bottom: 10px;
  right: 10px;
  }
}
</style>
