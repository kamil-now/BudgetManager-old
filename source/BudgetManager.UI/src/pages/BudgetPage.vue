<template>
  <Toast />
  <div v-if="!failed" class="budget-page">
    <div class="budget-page_content">
      <div class="budget-page_content-items">
        <BalanceView />
        <TabView class="budget-page_content-items_tab-view">
          <TabPanel header="Funds">
            <FundsView />
          </TabPanel>
          <TabPanel header="Accounts">
            <AccountsView />
          </TabPanel>
        </TabView>
      </div>

      <div class="budget-page_content-operations">
        <OperationsList></OperationsList>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import BalanceView from '@/components/BalanceView.vue';
import AccountsView from '@/components/AccountsView.vue';
import FundsView from '@/components/FundsView.vue';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';
import { onMounted, ref } from 'vue';
import { useToast } from 'primevue/usetoast';
import axios, { AxiosError } from 'axios';
import OperationsList from '@/components/OperationsList.vue';

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
      const axiosErrorMessage = (error as AxiosError).response?.data as string[];
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
  margin: 1rem;
  padding: 0.5rem;
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

    &-items {
      height: calc(100% - 4rem);
      min-width: 300px;
      width: calc(25% - 0.5rem);
      >* {
        overflow: auto;
        max-height: 33%;
      }

      &_tab-view {
        max-height: 100%;
      }
    
    }
    &-operations {
      height: calc(100% - 4rem);
      width: calc(75% - 0.5rem);
    }
  }

  overflow: hidden;

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
}</style>
