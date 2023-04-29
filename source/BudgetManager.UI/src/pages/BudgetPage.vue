<template>
  <div class="budget-page">
    <InputField 
      #input
      v-model="value" 
      ref="input" 
      @hook:mounted="() => input?.focus()"/>
    <InputField v-model="value" ref="input2"/>
    <teleport to="#app">
      <button class="log-out-btn" @click="logout()">Log-out</button>
    </teleport>
  </div>
</template>

<script setup lang="ts">
import InputField from '@/components/InputField.vue';
// import axios, { AxiosError, AxiosResponse } from 'axios';
import { ComponentPublicInstance, inject, onMounted, ref } from 'vue';
import { AUTH, IAuthService } from '../auth';

type InputFieldComponent = ComponentPublicInstance<typeof InputField>;

const auth = inject<IAuthService>(AUTH);

const value = ref<number>(42);
const input = ref<InputFieldComponent | null>(null);

onMounted(() => {
  // axios.get<AxiosResponse<{[currency: string]: number}>>('/api/balance')
  //   .then( 
  //     res => console.warn(res.data),
  //     (error: AxiosError) => console.error(error)
  //   );
    
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
