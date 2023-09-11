<template>
  <ProgressBar 
    v-if="isLoggedIn && isLoading"
    class="loading-indicator"
    mode="indeterminate"
    >
  </ProgressBar>
  <router-view/>
</template>

<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { useAppStore } from './store/store';

const store = useAppStore();
const { isLoggedIn, isLoading } = storeToRefs(store);

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

  .loading-indicator {
    height: 0.25rem;
    width: 100vw;
    position: absolute;
    top: 0;
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
