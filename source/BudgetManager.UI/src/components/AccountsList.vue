<template>
  <div class="accounts-list">
    <div 
      class="accounts-list_item"
      v-for="(account, index) in accounts" :key="account.id" 
    >
      <AccountInput 
        :account="account" 
        :isNew="!account.id"
        :display-label="index === 0"
        :autofocus="autofocus && index === 0"
        @changed="onAccountChanged($event, index)"
      />
      <div class="accounts-list_item-actions">
        <ConfirmPopup></ConfirmPopup>
        <Button 
          v-if="accounts.length !== 1"
          icon="pi pi-times" 
          severity="danger" 
          text 
          rounded 
          aria-label="Remove" 
          @click="removeAt($event, index)" 
        />
        <Button 
          v-if="index === accounts.length - 1"
          icon="pi pi-plus" 
          text 
          rounded 
          aria-label="Add" 
          @click="addNew()" 
        />
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import AccountInput from '@/components/AccountInput.vue';
import { Account } from '@/models/account';
import { computed } from 'vue';
import { useConfirm } from 'primevue/useconfirm';

const props = defineProps<{
  accounts: Account[],
  autofocus?: boolean,
  accountFactory:() => Account
}>();
const emit = defineEmits(['update']);

const confirm = useConfirm();

const accounts = computed({
  get: () => props.accounts,
  set: (newValue) => emit('update', newValue)
});

function onAccountChanged(account: Account, index: number) {
  accounts.value[index] = account;
}

function removeAt(event: MouseEvent, index: number) {
  const accept = () => accounts.value.splice(index, 1);
  const account = accounts.value[index];
  if (!!account.id || !!account.name || account.balance.amount === 0) {
    accept();
    return;
  }
  confirm.require({
    target: event.target as HTMLElement,
    message: 'Delete this account?',
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    rejectClass: 'p-button-secondary',
    accept
  });
}


function addNew() {
  if (accounts.value.length >= 100) {
    alert('You cannot create more than 100 accounts');
    return;
  }
  accounts.value.push(props.accountFactory());
}

</script>

<style lang="scss">
.accounts-list {
  display: flex;
  flex-direction: column;
  align-items: flex-end;

  &_item {
    width: 100%;
    display: flex;
    flex-direction: row;
    align-items: center;
    gap: 0.25rem;

    &:not(:first-of-type) {
      margin-top: 0.5rem;
    }

    &-actions {
      display: flex;
      flex-direction: column;
      .p-button {
        padding: 0 !important;
        height: 1.5rem !important;
        width: 1.5rem !important;
      }
    }
  }
}
</style>
