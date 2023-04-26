<template>
  <div class="budget-page">
    <InputField v-model="value" ref="input"/>
    <teleport to="#app">
      <button class="log-out-btn" @click="logout()">Log-out</button>
    </teleport>
  </div>
</template>

<script setup lang="ts">
import InputField from '@/components/InputField.vue';
import axios, { AxiosError, AxiosResponse } from 'axios';
import { Ref, ref, inject, onMounted } from 'vue';
import { AUTH, IAuthService } from '../auth';

const auth = inject<IAuthService>(AUTH);

const value = ref(42);
const input: Ref<any> = ref(null);


onMounted(() => {
  console.warn(input.value);
  input.value?.focus();
  axios.get<AxiosResponse<{[currency: string]: number}>>('/api/balance')
    .then( 
      res => console.warn(res.data),
      (error: AxiosError) => console.error(error)
    );
    
});
async function logout() {
  await auth?.logout();
}
</script>

<style lang="scss">
.budget-page {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
}
</style>
