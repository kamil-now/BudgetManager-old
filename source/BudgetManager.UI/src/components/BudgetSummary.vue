<template>
  <div class="budget-summary">
    <DataTable 
      :value="funds" 
      class="p-datatable-sm" 
      tableClass="budget-summary_table"
      columnResizeMode="expand"
    >
      <Column field="name" header="Funds" class="budget-summary_column">
      </Column>
      <Column field="balance" class="budget-summary_column">
        <template #body="slotProps">
          <div>
            <span v-for="(value, currency) in slotProps.data.balance" :key="currency">
              {{ currency + ' ' + value }}
            </span>
          </div>
        </template>
      </Column>
    </DataTable>
    <AccountsTable/>
  </div>
</template>

<script setup lang="ts">
import { useAppStore } from '@/store/store';
import AccountsTable from './AccountsTable.vue';
const { funds } = useAppStore();
</script>

<style lang="scss">
$padding: 0.5rem;
$table-width: calc(map-get($breakpoints, xs) - 2*$padding);
$column-width: calc($table-width / 2);
.budget-summary {
  padding: $padding;
  width: 100%;
  max-height: 100%;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  &_table {
    width: $table-width;
  }
  &_column {
    width: $column-width;
  }
}

</style>
