<template>
  <div class="budget-page">
    <CreateBudget v-if="isNewUser"/>
    <template v-else>
      <div class="budget-page_panel">
        <FundsDataTable/>
      </div>
      <div class="budget-page_panel">
        <IncomesDataTable />
        <ExpensesDataTable />
      </div>
      <div class="budget-page_panel">
        <AccountsDataTable/>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import SettingsView from '@/components/SettingsView.vue';
import AccountsDataTable from '@/components/AccountsDataTable.vue';
import FundsDataTable from '@/components/FundsDataTable.vue';
import ExpensesDataTable from '@/components/ExpensesDataTable.vue';
import IncomesDataTable from '@/components/IncomesDataTable.vue';
import BudgetSummary from '@/components/BudgetSummary.vue';
import CreateBudget from '@/components/CreateBudget.vue';
import { useAppStore } from '@/store/store';
import { ref } from 'vue';
import { storeToRefs } from 'pinia';

const { isNewUser } = storeToRefs(useAppStore());
const activeIndex = ref(2);

</script>

<style lang="scss">
.budget-page {
  max-height: 100%;
  display: flex;
  flex-direction: row;
  flex-wrap: nowrap;

  overflow: hidden;
  justify-content: space-around;

  height: 100%;
  width: 100%;
  margin: 3rem;
  @extend .card;
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
