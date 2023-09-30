<template>
  <div class="allocations-view">
    <ListView header="Allocations" v-model="allocations" :copy="copyAllocation" :createNew="createAllocationObject"
      :save="createNewAllocation" :update="updateAllocation" :remove="deleteAllocation" :allowAdd="funds.length > 0">
      <template #content="{ data }">
        <div class="allocations-view_body">
          <span class="date">{{ DisplayFormat.dateOnly(data.date) }}</span>
          <div class="allocations-view_body-left">
            <span class="money">{{ DisplayFormat.money(data.value) }}</span>
          </div>
          <div class="allocations-view_body-right">
            <span class="operation-title">{{ data.title }}</span>
            <span>{{ data.targetFundName }}</span>
          </div>
        </div>
      </template>
      <template #editor="{ data }">
        <AllocationInput :allocation="data" @changed="onAllocationChanged(data, $event)" />
      </template>
    </ListView>
  </div>
</template>
<script setup lang="ts">
import ListView from '@/components/ListView.vue';
import AllocationInput from '@/components/AllocationInput.vue';
import { DisplayFormat } from '@/helpers/display-format';
import { Allocation } from '@/models/allocation';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';

const store = useAppStore();
const { createNewAllocation, updateAllocation, deleteAllocation } = store;

const { allocations, funds, unallocated } = storeToRefs(store);

function onAllocationChanged(allocation: Allocation, newValue: Allocation) {
  allocation.targetFundId = newValue.targetFundId;
  allocation.createdDate = newValue.createdDate;
  allocation.title = newValue.title;
  allocation.value = newValue.value;
  allocation.date = newValue.date;
  allocation.description = newValue.description;
}

function createAllocationObject() {
  const defaultFund = store.funds[0];
  const defaultCurrency = Object.keys(unallocated.value)[0] ?? Object.keys(defaultFund.balance)[0];
  return {
    date: new Date(),
    targetFundId: defaultFund.id,
    value: {
      currency: defaultCurrency
    }
  };
}
function copyAllocation(allocation: Allocation) {
  const copy = {
    ...allocation,
    id: undefined
  };
  return copy;
}
</script>

<style lang="scss">
.allocations-view {
  width: 100%;
  height: 100%;

  &_body {
    display: flex;
    width: 100%;
    align-items: center;

    span {
      display: inline-block;
      text-overflow: ellipsis;
      overflow: hidden;
    }

    &-left {
      width: calc(50% - #{$date-width});
      display: flex;
      flex-direction: column;
      align-items: end;

      span {
        text-align: right;
      }
    }

    &-right {
      width: calc(50% - #{$date-width});
      display: flex;
      flex-direction: column;
      align-items: start;

      span {
        text-align: left;
        padding-left: 1rem;
      }
    }
  }

  &_editor {
    display: flex;
  }
}
</style>
