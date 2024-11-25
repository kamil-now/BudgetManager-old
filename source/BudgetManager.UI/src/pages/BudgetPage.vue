<template>
  <AppPage>
    <div class="budget-page">
      <div
        v-if="isBudgetLoaded"
        class="budget-page_content"
      >
        <template v-if="windowWidth && windowWidth > 800 && windowWidth < 1300">
          <TabView :lazy="true">
            <TabPanel header="Funds">
              <FundsView />
            </TabPanel>
            <TabPanel header="Allocation templates">
              <IncomeAllocationTemplatesView />
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
          <TabPanel header="Operations">
            <OperationsView></OperationsView>
          </TabPanel>
          <TabPanel header="Funds">
            <FundsView />
          </TabPanel>
          <TabPanel header="Allocation templates">
            <IncomeAllocationTemplatesView />
          </TabPanel>
          <TabPanel header="Accounts">
            <AccountsView />
          </TabPanel>
        </TabView>
        <template v-else>
          <TabView :lazy="true">
            <TabPanel header="Funds">
              <FundsView />
            </TabPanel>
            <TabPanel header="Allocation templates">
              <IncomeAllocationTemplatesView />
            </TabPanel>
          </TabView>
          <OperationsView></OperationsView>
          <TabView>
            <TabPanel header="Accounts">
              <AccountsView />
            </TabPanel>
          </TabView>
        </template>
      </div>
    </div>
  </AppPage>
</template>

<script setup lang="ts">
import AccountsView from '@/components/account/AccountsView.vue';
import FundsView from '@/components/fund/FundsView.vue';
import OperationsView from '@/components/money-operations/OperationsView.vue';
import IncomeAllocationTemplatesView from '@/components/income-allocation/IncomeAllocationTemplatesView.vue';
import AppPage from '@/pages/AppPage.vue';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';
import { nextTick, onDeactivated, onMounted, ref } from 'vue';

const store = useAppStore();
const { isNewUser, isBudgetLoaded } = storeToRefs(store);
const windowWidth = ref<number>();

onMounted(() => {
  if (isNewUser) {
    store.createBudget();
  }

  nextTick(() => {
    onResize();
    window.addEventListener('resize', onResize);
  });
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
    .income-allocation-templates-view {
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
