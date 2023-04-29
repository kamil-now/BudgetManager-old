<template>
  <router-view />
</template>

<script setup lang="ts"></script>

<style lang="scss">
#app {
  @media (max-width: map-get($breakpoints, xs)), (max-height: 500px) {
    * {
      display: none;
    }
  }

  &::before {
    @extend .label-text;
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
  padding-top: 1rem;
  margin: 0;
  display: flex;
  align-items: flex-start;
  justify-content: center;
  animation: fadein 1s;
  
  @include media-breakpoint(sm) {
    padding: 3rem 0 1rem 0;
    margin: 0 1rem;
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
.log-in-btn {
  min-width: 100px;
  background-color: var(--app-accent-color);
  color: var(--app-accent-text-color);
  border-radius: 5px;
  font-weight: 500;
  font-size: 12px;
}
.log-out-btn {
  @extend .log-in-btn;
  position: absolute;
  bottom: 50px;
  right: 50px;
}
</style>
