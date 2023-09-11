<template>
  <Toast />
  <div v-if="!failed" class="budget-page">
    <div class="budget-page_header">
      <BalanceView />
    </div>

    <div class="budget-page_content">
      <div class="budget-page_content-panel">
        <TabView class="budget-page_content-panel_tab-view">
          <TabPanel header="Funds">
            <FundsView/>
          </TabPanel>
          <TabPanel header="Accounts">
            <AccountsView/>
          </TabPanel>
        </TabView>
      </div>

      <div class="budget-page_content-panel">
        <TabView class="budget-page_content-panel_tab-view">
          <TabPanel header="Expenses">
            <ExpensesView />
          </TabPanel>
          <TabPanel header="Incomes">
            <IncomesView />
          </TabPanel>
          <TabPanel header="Allocations">
            <AllocationsView />
          </TabPanel>
        </TabView>
      </div>
      
      <div class="budget-page_content-panel">
        <TabView class="budget-page_content-panel_tab-view">
          <TabPanel header="Fund Transfers">
            <FundTransfersView />
          </TabPanel>
          <TabPanel header="Account Transfers">
            <AccountTransfersView />
          </TabPanel>
          <TabPanel header="Currency Exchanges">
            <CurrencyExchangesView/>
          </TabPanel>
        </TabView>
      </div>

    </div>
  </div>
</template>

<script setup lang="ts">
import BalanceView from '@/components/BalanceView.vue';
import AllocationsView from '@/components/AllocationsView.vue';
import FundTransfersView from '@/components/FundTransfersView.vue';
import AccountTransfersView from '@/components/AccountTransfersView.vue';
import AccountsView from '@/components/AccountsView.vue';
import FundsView from '@/components/FundsView.vue';
import ExpensesView from '@/components/ExpensesView.vue';
import IncomesView from '@/components/IncomesView.vue';
import CurrencyExchangesView from '@/components/CurrencyExchangesView.vue';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';
import { onMounted, ref } from 'vue';
import { useToast } from 'primevue/usetoast';
import axios, { AxiosError } from 'axios';

const store = useAppStore();
const { isNewUser } = storeToRefs(store);
const toast = useToast();
const failed = ref<boolean>(false);
onMounted(() => {
  if (isNewUser) {
    store.createBudget();
  }
  
  axios.interceptors.response.use(
    response => response,
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
      const axiosErrorMessage = (error as AxiosError).response?.data  as string[];
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
        summary: (error as AxiosError).request ? (error as AxiosError).request.statusText : error.name, 
        detail: errorMessage,
      });
    }
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
    display: flex;
    gap: 1rem;
    height: 100%;
    flex-wrap: wrap;
    overflow: auto;
    &-panel {
      height: 100%;
      width: 32%;
      min-width: 300px;
      > * {
        overflow: auto;
        max-height: 33%;
      }
      &_tab-view {
        max-height: 100%;
      }
    }
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
    height: 100%;
    overflow: hidden;
    .p-tabview-panels {
      height: 90%;
      padding: 0;
      .p-tabview-panel {
        height: 100%;
      }
    }
  }
}
</style>
