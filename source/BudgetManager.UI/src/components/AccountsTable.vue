<template>
  <div class="accounts">
    <ConfirmPopup></ConfirmPopup>
    <DataTable 
      v-model:editingRows="editingRows"
      :value="accounts" 
      class="p-datatable-sm" 
      editMode="row" 
      dataKey="id"
      tableClass="accounts_table"
      columnResizeMode="expand"
      @row-edit-cancel="onRowEditCancel"
      @row-edit-save="onRowEditSave"
      @rowReorder="onRowReorder" 
    >
      <Column rowReorder headerStyle="width: 2rem" header="Accounts" />
      <Column field="name" class="accounts_name-column">
        <template #editor="{ data }">
          <InputText
            class="p-inputtext-sm"
            :pt="{
                root: { class: 'accounts_name-column_name-editor' }
            }"
            id="accountName" 
            placeholder="Account name"
            v-model="data.name" 
          />
        </template>
      </Column>
      <Column field="balance" class="accounts_balance-column">
        <template #body="slotProps">
          <span>{{ DisplayFormat.money(slotProps.data.balance) }}</span>
        </template>
        <template #editor="{ data, index }">
          <div class="accounts_balance-column_cell">
            <template v-if="data.id">
              <span>{{ DisplayFormat.money(data.balance) }}</span>
            </template>
            <template v-else>
              <Dropdown 
                class="p-inputtext-sm"
                v-model="data.balance.currency" 
                :options="currencyCodeList" 
              />
              <InputNumber 
                class="p-inputtext-sm"
                inputClass="accounts_balance-column_amount-editor"
                v-model="data.balance.amount" 
                :allowEmpty="false"
                :min="0"
                :maxFractionDigits="2"
                :max="1000000000"
              />
            </template>
            <Button 
              v-if="data.id && accounts.length !== 1"
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
        class="accounts_action-column"
      >
        <template #header>
          <Button 
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
import currencies from '@/assets/currencies.json';
import { DisplayFormat } from '@/helpers/display-format';
import { Account } from '@/models/account';
import { useAppStore } from '@/store/store';
import { storeToRefs } from 'pinia';
import { useConfirm } from 'primevue/useconfirm';
import { Ref, ref } from 'vue';
type RowEditEvent =  {
  data: Account,
  newData: Account,
  index: number
};
const currencyCodeList = Object.keys(currencies);

const confirm = useConfirm();
const store = useAppStore();
const { createNewAccount, updateAccount, deleteAccount, updateUserSettings } = store;

const { accounts } = storeToRefs(store);
const editingRows: Ref<Account[]> = ref([]);

function onRowEditSave(event: RowEditEvent) {
  const { newData, index } = event;
  if (newData.id) {
    updateAccount(newData);
  } else {
    createNewAccount(newData);
  }
  accounts.value[index] = newData;
}

function onRowEditCancel(event: RowEditEvent) {
  if (!event.newData.id) {
    accounts.value.splice(event.index, 1);
  }
}

function addNew() {
  const account = {
    name: 'New Account',
    balance: {
      amount: 0,
      currency: getDefaultCurrency()
    }
  };
  accounts.value.push(account);
  editingRows.value = [account];
}

function getDefaultCurrency(): string {
  return accounts.value 
    ? accounts.value[accounts.value.length - 1].balance.currency
    : Object.keys(currencies)[0];
}

function removeAt(event: MouseEvent, index: number) {
  console.warn(event, index);
  const account = accounts.value[index];
  const accept = () => deleteAccount(account);
  if (!account.id && account.balance.amount === 0) {
    accept();
    return;
  }
  confirm.require({
    target: event.target as HTMLElement,
    message: `Remove ${account.name}?`,
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    rejectClass: 'p-button-secondary',
    accept
  });
}

function onRowReorder(event: {dragIndex: number, dropIndex: number}) {
  const { dragIndex, dropIndex } = event;
  const element = accounts.value[dragIndex];
  accounts.value.splice(dragIndex, 1);
  accounts.value.splice(dropIndex, 0, element);
  updateUserSettings();
}


</script>

<style lang="scss">
$padding: 0.5rem;
$table-width: calc($base-width - 2 * $padding);
$action-column-width: 6rem;
$column-width: calc($table-width / 2 - $action-column-width * 2);
.accounts {
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
  }
  &_name-column {
    width: 150px;
    &_name-editor {
      width: 150px;
    }
  }
  &_balance-column {
    width: 260px;
    &_cell {
      display: flex;
      align-items: center;
      justify-content: space-between;
    }
    &_amount-editor {
      width: 100px;
      border-left: 0;
      border-top-left-radius: 0;
      border-bottom-left-radius: 0;
    }
  }
  &_action-column {
    .p-column-header-content {
      justify-content: center;
    }
    width: $action-column-width;
  }
}
</style>
