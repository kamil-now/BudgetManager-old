<template>
  <div style="display: flex">
    <Button
      icon="pi pi-copy"
      text
      rounded
      size="small"
      aria-label="Copy"
      @click="createCopy(operation, isFilteredByTypeOrContent)"
    />
    <Button
      icon="pi pi-pencil"
      text
      rounded
      size="small"
      aria-label="Edit"
      @click="edit(operation, 'Edit')"
    />
    <Button
      icon="pi pi-times"
      severity="danger"
      text
      rounded
      size="small"
      aria-label="Remove"
      @click="remove($event, operation)"
    />
  </div>
</template>
<script setup lang="ts">
import { MoneyOperationUtils } from '@/helpers/money-operation-utils';
import { StringUtils } from '@/helpers/string-utils';
import { MoneyOperation } from '@/models/money-operation';
import { MoneyOperationType } from '@/models/money-operation-type.enum';
import { useAppStore } from '@/store/store';
import { useConfirm } from 'primevue/useconfirm';
import { useDialog } from 'primevue/usedialog';
import InputDialog from '@/components/InputDialog.vue';
import { storeToRefs } from 'pinia';

defineProps<{ operation: MoneyOperation }>();
const dialog = useDialog();
const store = useAppStore();
const confirm = useConfirm();
const { isFilteredByTypeOrContent } = storeToRefs(store);

const {
  deleteIncome,
  deleteAllocation,
  deleteExpense,
  deleteCurrencyExchange,
  deleteAccountTransfer,
  deleteFundTransfer,
} = store;
function createCopy(operation: MoneyOperation, useCurrentDate: boolean) {
  edit(MoneyOperationUtils.copy(operation, useCurrentDate), 'Create');
}
function edit(operation: MoneyOperation, action: 'Edit' | 'Create') {
  dialog.open(InputDialog, {
    data: {
      operation,
    },
    props: {
      header: `${action} ${StringUtils.camelCaseToWords(
        MoneyOperationType[operation.type]
      )}`,
      modal: true,
      closable: false,
    },
  });
}
function remove(event: MouseEvent, operation: MoneyOperation) {
  confirm.require({
    target: event.target as HTMLElement,
    message:
      'Are you sure you want to remove this operation? This action is permanent.',
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    rejectClass: 'p-button-secondary',
    accept: () => removeOperation(operation),
  });
}
function removeOperation(operation: MoneyOperation) {
  if (!operation.id) {
    throw new Error();
  }
  switch (operation.type) {
    case MoneyOperationType.Income:
      deleteIncome(operation.id);
      break;
    case MoneyOperationType.Allocation:
      deleteAllocation(operation.id);
      break;
    case MoneyOperationType.Expense:
      deleteExpense(operation.id);
      break;
    case MoneyOperationType.CurrencyExchange:
      deleteCurrencyExchange(operation.id);
      break;
    case MoneyOperationType.AccountTransfer:
      deleteAccountTransfer(operation.id);
      break;
    case MoneyOperationType.FundTransfer:
      deleteFundTransfer(operation.id);
      break;
    default:
      throw new Error('Unknown operation.');
  }
}
</script>

<style lang="scss"></style>
