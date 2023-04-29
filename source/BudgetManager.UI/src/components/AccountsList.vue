<template>
  <div class="accounts-list">
    <div 
      class="accounts-list_item" 
      v-for="(account, index) in accounts" :key="account.id"
    >
      <AccountInput 
        :account="account" 
        :isNew="!account.id"
        :autofocus="autofocus && index === 0"
        @changed="onAccountChanged($event, index)"
      />
      <button @click="removeAt(index)" class="remove-btn">-</button>
    </div>
    
    <button @click="addNew()" class="add-btn">+</button>
  </div>
</template>
<script setup lang="ts">
import AccountInput from '@/components/AccountInput.vue';
import { Account } from '@/models/account';
import { computed } from 'vue';
const props = defineProps<{
  accounts: Account[],
  autofocus?: boolean,
  accountFactory:() => Account
}>();

const emit = defineEmits(['update']);
const accounts = computed({
  get: () => props.accounts,
  set: (newValue) => emit('update', newValue)
});

function onAccountChanged(account: Account, index: number) {
  accounts.value[index] = account;
}

function removeAt(index: number) {
  accounts.value.splice(index, 1);
}

function addNew() {
  accounts.value.push(props.accountFactory());
}

</script>

<style lang="scss">
.accounts-list {
  display: flex;
  flex-direction: column;
  align-items: end;
  gap: 16px;
  &_item {
    gap: 8px;
    display: flex;
    flex-direction: row;
    align-items: center
  }
}
</style>
