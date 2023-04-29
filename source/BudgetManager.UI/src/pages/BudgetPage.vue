<template>
  <div class="budget-page">
    <AccountsList 
      :autofocus="true" 
      :accounts="accounts"
      :account-factory="() => createAccount()"
    />
    <teleport to="#app">
      <button class="log-out-btn" @click="logout()">Log-out</button>
    </teleport>
  </div>
</template>

<script setup lang="ts">
import AccountsList from '@/components/AccountsList.vue';
import { Account } from '@/models/account';
// import axios, { AxiosError, AxiosResponse } from 'axios';
import { inject, onMounted, ref } from 'vue';
import { AUTH, IAuthService } from '../auth';


const auth = inject<IAuthService>(AUTH);

const accounts = ref<Account[]>([createAccount()]);

function createAccount(): Account {
  return {
    name: 'Your account name',
    balance: {
      amount: 0,
      currency: 'USD'
    }
  };
}

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
</style>
