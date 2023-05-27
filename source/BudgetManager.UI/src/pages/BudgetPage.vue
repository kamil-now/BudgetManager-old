<template>
  <div class="budget-page">
    <CreateBudget v-if="newUser"/>
    <teleport to="#app">
      <button class="log-out-btn" @click="logout()">Log-out</button>
    </teleport>
  </div>
</template>

<script setup lang="ts">
import axios, { AxiosError, AxiosResponse } from 'axios';
import CreateBudget from '@/components/CreateBudget.vue';
import { inject, onMounted, ref } from 'vue';
import { AUTH, IAuthService } from '../auth';


const auth = inject<IAuthService>(AUTH);
const newUser = ref<boolean>(true); // TODO

onMounted(() => {
  axios.get<AxiosResponse<{[currency: string]: number}>>('/api/balance')
    .then(
      res => console.warn(res.data),
      (error: AxiosError<string[]>) => {
        if (error.response?.data.some(x => x.includes('does not exist'))) {
          newUser.value = true;
        } else {
          console.error(error);
        }
      }
    );
});
async function logout() {
  await auth?.logout();
}
</script>

<style lang="scss">
.budget-page {
  display: flex;
  gap: 8px;
  flex-direction: column;
  flex-wrap: wrap;
  width: 400px;
  height: 100%;
  align-items: center;
}
</style>
