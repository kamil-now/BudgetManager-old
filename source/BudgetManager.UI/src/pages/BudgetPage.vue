<template>
  <div class="budget-page">
    <CreateBudget v-if="isNewUser"/>
    <template v-else>
      <TabView :activeIndex="2">
        <TabPanel header="EXC">
        </TabPanel>
        <TabPanel header="INC">
          <IncomesDataTable />
        </TabPanel>
        <TabPanel>
          <template #header>
            <i class="pi pi-chart-bar"></i>
          </template>
          <BudgetSummary />
        </TabPanel>
        <TabPanel header="EXP">
          <ExpensesDataTable />
        </TabPanel><TabPanel header="TRF">
        </TabPanel>
        <!-- <TabPanel>
            <template #header>
              <i class="pi pi-cog"></i>
            </template>
            <SettingsView />
        </TabPanel> -->
      </TabView>
    </template>
  </div>
</template>

<script setup lang="ts">
import SettingsView from '@/components/SettingsView.vue';
import ExpensesDataTable from '@/components/ExpensesDataTable.vue';
import IncomesDataTable from '@/components/IncomesDataTable.vue';
import BudgetSummary from '@/components/BudgetSummary.vue';
import CreateBudget from '@/components/CreateBudget.vue';
import { useAppStore } from '@/store/store';

const { isNewUser } = useAppStore();

</script>

<style lang="scss">
.budget-page {
  max-height: 100%;
  display: flex;
  flex-direction: column;
  flex-wrap: wrap;

  overflow: hidden;
  align-items:center;

  height: auto;
  width: map-get($breakpoints, 'xs');
  margin-top: 3rem;
  @extend .card;
  @include media-breakpoint('sm', 'down') {
    margin: 0;
    border-radius: 0;
    box-shadow: 0;
  }
  @include media-breakpoint('lg') {
    width: map-get($breakpoints, 'md')
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
