<template>
  <Toast />
  <router-view v-if="!failed" />
</template>

<script setup lang="ts">
import { useAppStore } from './store/store';
import { AUTH, IAuthService } from '@/auth';
import { inject, onBeforeMount, ref } from 'vue';
import { useToast } from 'primevue/usetoast';
import axios, { AxiosError } from 'axios';
import { useRouter } from 'vue-router';

const auth = inject<IAuthService>(AUTH);
if (!auth) {
  throw new Error('No provider for IAuthService');
}
const store = useAppStore();
const toast = useToast();
const router = useRouter();
const failed = ref<boolean>(false);

onBeforeMount(async () => {
  if (!store.isLoggedIn) {
    await auth.login();
  }
  axios.interceptors.response.use(
    (response) => response,
    (error: AxiosError | Error) => {
      console.error(error);
      let errorMessage: string;

      if (typeof (error as AxiosError).response === 'string') {
        toast.add({
          severity: 'error',
          summary: 'Unexpected error.',
          detail: 'Please reload the page.',
        });
        failed.value = true;
        return;
      }
      if ((error as AxiosError).response?.status === 401) {
        router.push('/login');
        return;
      }
      const axiosErrorMessage = (error as AxiosError).response
        ?.data as string[];
      if (Array.isArray(axiosErrorMessage)) {
        errorMessage = axiosErrorMessage.join('\n');
      } else {
        errorMessage = (error as Error).message;
      }
      if (errorMessage.includes('Budget already exists.')) {
        return;
      }
      toast.add({
        severity: 'error',
        summary: (error as AxiosError).request
          ? (error as AxiosError).request.statusText
          : error.name,
        detail: errorMessage,
      });
    }
  );
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
  height: 100vh;
  width: 100vw;
  > * {
    max-width: 1400px;
  }
  margin: 0;
  display: flex;
  align-items: center;
  flex-direction: column;
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
