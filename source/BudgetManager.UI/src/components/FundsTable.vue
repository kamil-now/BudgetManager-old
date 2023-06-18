<template>
  <div class="funds">
    <ConfirmPopup></ConfirmPopup>
    <DataTable 
      v-model:editingRows="editingRows"
      :value="funds" 
      class="p-datatable-sm" 
      editMode="row" 
      dataKey="id"
      tableClass="funds_table"
      columnResizeMode="expand"
      @row-edit-init="onRowEditInit"
      @row-edit-cancel="onRowEditCancel"
      @row-edit-save="onRowEditSave"
      @rowReorder="onRowReorder" 
    >
      <Column rowReorder header-class="funds_table_header-column" header="Funds"/>
      <Column field="name" class="funds_name-column">
        <template #editor="{ data }">
          <InputText
            :pt="{
                root: { class: 'funds_name-column_name-editor' }
            }"
            placeholder="Fund name"
            v-model="data.name" 
          />
        </template>
      </Column>
      <Column field="balance" class="funds_balance-column">
        <template #body="slotProps">
          <div class="funds_balance-column_cell">
            <div class="funds_balance-column_cell_values">
              <span v-for="(value, currency) in slotProps.data.balance" :key="currency">
                {{ DisplayFormat.money({ amount: value, currency: currency.toString() }) }}
              </span>
            </div>
            <label v-if="slotProps.data.isDefault" class="funds_balance-column_cell_label">default</label>
          </div>
        </template>
        <template #editor="{ data, index }">
          <div class="funds_balance-column_cell">
            <div class="funds_balance-column_cell_values">
              <span v-for="(value, currency) in data.balance" :key="currency">
                {{ DisplayFormat.money({ amount: value, currency: currency.toString() }) }}
              </span>
            </div>
            <label v-if="data.isDefault" class="funds_balance-column_cell_label">default</label>
            <Button 
              v-if="data.id && funds.length !== 1 && !data.isDefault"
              icon="pi pi-times" 
              severity="danger" 
              text 
              rounded 
              aria-label="Remove" 
              @click="removeAt($event, index)" 
            />
          </div>
        </template>
      </Column>
      <Column 
        :rowEditor="true"
        bodyStyle="text-align:center"
        class="funds_action-column"
      >
        <template #header>
          <Button 
            v-if="editingRows.length === 0"
            icon="pi pi-plus" 
            text 
            rounded 
            aria-label="Add" 
            @click="addNew()" 
          />
        </template>
      </Column>
    </DataTable>
  </div>
</template>
<script setup lang="ts">
import { DisplayFormat } from '@/helpers/display-format';
import { Fund } from '@/models/fund';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';
import { useConfirm } from 'primevue/useconfirm';
import { Ref, ref } from 'vue';
type RowEditEvent =  {
  data: Fund,
  newData: Fund,
  index: number
};

const confirm = useConfirm();
const store = useAppStore();
const { createNewFund, updateFund, deleteFund, updateUserSettings } = store;

const { funds } = storeToRefs(store);
const editingRows: Ref<Fund[]> = ref([]);

function onRowEditInit(event: RowEditEvent) {
  editingRows.value = [event.data];
}

function onRowEditSave(event: RowEditEvent) {
  const { newData, index } = event;
  if (newData.id) {
    updateFund(newData);
  } else {
    funds.value.splice(index, 1); // will be re-added after it's created
    createNewFund(newData);
  }
}

function onRowEditCancel(event: RowEditEvent) {
  if (!event.newData.id) {
    funds.value = funds.value.filter(x => x.id);
    editingRows.value = [];
  }
}

function addNew() {
  const fund = {
    name: 'New Fund',
  };
  funds.value.unshift(fund);
  editingRows.value = [fund];
}

function removeAt(event: MouseEvent, index: number) {
  const fund = funds.value[index];
  confirm.require({
    target: event.target as HTMLElement,
    message: `Remove ${fund.name}?`,
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    rejectClass: 'p-button-secondary',
    accept: () => deleteFund(fund)
  });
}

function onRowReorder(event: {dragIndex: number, dropIndex: number}) {
  const { dragIndex, dropIndex } = event;
  const element = funds.value[dragIndex];
  funds.value.splice(dragIndex, 1);
  funds.value.splice(dropIndex, 0, element);
  updateUserSettings();
}
</script>

<style lang="scss">
$padding: 0.25rem;
$table-width: calc($base-width - 2 * $padding);
$action-column-width: 6rem;
$header-column-width: 1rem;
$cell-padding: 0.5rem;
$content-width: calc($table-width - $action-column-width - $header-column-width);
$balance-column-width: 94.47px;
$name-column-width: calc($content-width - $balance-column-width - 4 * $cell-padding);
.funds {
  .p-inputtext {
    padding: 0.5rem;
  }
  .p-datatable.p-datatable-sm .p-datatable-tbody > tr > td {
    padding: $cell-padding;
  }
  th {
    .p-button.p-button-icon-only {
      height: 1rem;
      width: 1rem;
    }
  }
  tr {
    .p-button.p-button-icon-only {
      height: 2rem;
      width: 2rem;
    }
  }
  &_table {
    width: $table-width;
    &_header-column {
      width: $header-column-width;
      max-width: $header-column-width;
    }
  }
  &_name-column {
    width: $name-column-width;
    padding: 0 !important;
    &_name-editor {
      width: $name-column-width;
      max-width: $name-column-width;
    }
  }
  &_balance-column {
    width: $balance-column-width;
    &_cell {
      display: flex;
      align-items: center;
      justify-content: space-between;
      &_label {
        margin: 0.25rem;
        font-size: 0.75rem;
      }
      &_values {
        display: flex;
        flex-direction: column;
        gap: 1rem;
      }
    }
  }
  &_action-column {
    padding: 0 !important;
    .p-column-header-content {
      justify-content: center;
    }
    width: $action-column-width;
    min-width: $action-column-width;
  }
}
</style>
