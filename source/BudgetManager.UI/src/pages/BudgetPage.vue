<template>
  <Toast />
  <div class="budget-page">
    <div class="budget-page_header">
      <BalanceView />
      <Divider />
    </div>
    <div class="budget-page_content">
      <FundsView/>
      <AccountsView/>
      <AllocationsView />
      <IncomesView />
      <ExpensesView />
    </div>
  </div>
</template>

<script setup lang="ts">
import BalanceView from '@/components/BalanceView.vue';
import AllocationsView from '@/components/AllocationsView.vue';
import AccountsView from '@/components/AccountsView.vue';
import FundsView from '@/components/FundsView.vue';
import ExpensesView from '@/components/ExpensesView.vue';
import IncomesView from '@/components/IncomesView.vue';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';
import { onMounted } from 'vue';
import { useToast } from 'primevue/usetoast';
import axios, { AxiosError } from 'axios';

const store = useAppStore();
const { isNewUser } = storeToRefs(store);
const toast = useToast();
onMounted(() => {
  if (isNewUser) {
    store.createBudget();
  }
  
  axios.interceptors.response.use(
    response => response,
    (error: AxiosError) => toast.add({ 
      severity: 'error',
      summary: error.request.statusText, 
      detail: (error.response?.data as string[]).join('\n'),
      life: 3000 
    })
  );
});

</script>

<style lang="scss">
.budget-page {  
  height: 100%;
  width: 100%;
  margin: 3rem;
  padding: 1rem;
  @extend .card;
  max-height: 100%;
  display: flex;
  flex-direction: column;
  flex-wrap: nowrap;
  &_header {
    display: flex;
    flex-direction: column;
    width: 100%;
    padding-left: 1rem;
  }
  &_content {
    display: grid;
    grid-template-columns: 1fr 1fr 1fr;
    gap: 50px;
  }

  overflow: hidden;


  // width: map-get($breakpoints, 'xs');
  // @include media-breakpoint('sm', 'down') {
  //   margin: 0;
  //   border-radius: 0;
  //   box-shadow: 0;
  //   width: 100%;
  // }
  // @include media-breakpoint('lg') {
  //   width: map-get($breakpoints, 'md')
  // }

  &_panel {
    height: 100%;
    width: 100%;
    display: flex;
    flex-direction: column;
    justify-content: start;
    padding: 1rem;
  }

  .p-tabview-nav {
    display: flex;
    justify-content: space-around;
    li {
      flex-grow: 1;
      a {
        width: 100%;
        display: flex;
        justify-content: center
      }
    }
  }

  .p-tabview {
    width: 100%;
  }
}
</style>
