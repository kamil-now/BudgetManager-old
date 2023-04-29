<template>
  <div class="budget-page">
    <AccountInput :autofocus="true" class="account-input" v-model="account"/>
    <AccountInput class="account-input" v-model="account"/>
    <teleport to="#app">
      <button class="log-out-btn" @click="logout()">Log-out</button>
    </teleport>
  </div>
</template>

<script setup lang="ts">
import AccountInput from '@/components/AccountInput.vue';
import { Account } from '@/models/account';
// import axios, { AxiosError, AxiosResponse } from 'axios';
import { inject, onMounted, ref } from 'vue';
import { AUTH, IAuthService } from '../auth';


const auth = inject<IAuthService>(AUTH);

const account = ref<Account>({
  name: 'Your account name',
  balance: {
    amount: 0,
    currency: 'USD'
  }
});


onMounted(() => {
  // axios.get<AxiosResponse<{[currency: string]: number}>>('/api/balance')
  //   .then(
  //     res => console.warn(res.data),
  //     (error: AxiosError<string[]>) => {
  //       if (error.response?.data.some(x => x.includes('does not exist'))) {
  //         // TODO
  //       } else {
  //         console.error(error);
  //       }
  //     }
  //   );
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
  width: 100%;
  height: 100%;
  align-items: center;
}
.account-input {
  margin: 8px;
}
</style>
