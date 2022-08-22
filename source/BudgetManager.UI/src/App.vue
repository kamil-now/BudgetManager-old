<template>
  <router-view />
</template>

<script setup lang="ts"></script>

<style lang="scss">
@media only screen and (max-width: 600px) {
  #app {
    padding-top: 1rem !important;
    margin: 0 !important;
  }
}
#app {
  height: calc(100vh - 4rem);
  padding: 3rem 0 1rem 0;
  margin: 0 1rem;
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
  &:hover {
    cursor: pointer;
  }
}
.log-out-btn {
  @extend .log-in-btn;
  position: absolute;
  bottom: 50px;
  right: 50px;
}
</style>
