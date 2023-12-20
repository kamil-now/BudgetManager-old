<template>
  <Toast />
  <div
    v-if="!failed"
    class="budget-page"
  >
    <div class="budget-page_content">
      <template v-if="windowWidth && windowWidth > 800 && windowWidth < 1300">
        <TabView :lazy="true">
          <TabPanel header="Funds">
            <FundsView />
          </TabPanel>
          <TabPanel header="Accounts">
            <AccountsView />
          </TabPanel>
        </TabView>
        <OperationsView></OperationsView>
      </template>
      <TabView
        v-else-if="windowWidth && windowWidth < 800"
        :lazy="true"
      >
        <TabPanel header="Funds">
          <FundsView />
        </TabPanel>
        <TabPanel header="Operations">
          <OperationsView></OperationsView>
        </TabPanel>
        <TabPanel header="Accounts">
          <AccountsView />
        </TabPanel>
      </TabView>
      <template v-else>
        <FundsView />
        <OperationsView></OperationsView>
        <AccountsView />
      </template>
    </div>
  </div>
</template>

<script setup lang="ts">
import AccountsView from '@/components/AccountsView.vue';
import FundsView from '@/components/FundsView.vue';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';
import { onMounted, ref, onDeactivated, nextTick } from 'vue';
import { useToast } from 'primevue/usetoast';
import axios, { AxiosError } from 'axios';
import OperationsView from '@/components/OperationsView.vue';

const store = useAppStore();
const { isNewUser } = storeToRefs(store);
const toast = useToast();
const failed = ref<boolean>(false);
const windowWidth = ref<number>();

onMounted(() => {
  if (isNewUser) {
    store.createBudget();
  }

  nextTick(() => {
    onResize();
    window.addEventListener('resize', onResize);
  });

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

onDeactivated(() => {
  window.removeEventListener('resize', onResize);
});

const onResize = () => (windowWidth.value = window.innerWidth);
</script>

<style lang="scss">
.budget-page {
  height: 100%;
  width: 100%;
  margin: 0.5rem;
  // @extend .card;
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
    overflow: hidden;
    max-width: 100%;

    .operations-view {
      flex: 2;
    }
    .funds-view {
      flex: 1;
    }
    .accounts-view {
      flex: 1;
    }
    .p-tabview {
      flex: 1;
    }
  }


  .p-tabview-nav {
    display: flex;
    justify-content: space-around;

    li {
      flex-grow: 1;

      a {
        width: 100%;
        display: flex;
        justify-content: center;
      }
    }
  }

  .p-tabview {
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
