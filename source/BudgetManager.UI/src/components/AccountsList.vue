<template>
  <div class="accounts-list">
    <DynamicList 
      v-slot="slotProps"
      :items="accounts"
      :createNew="accountFactory"
      :max="100"
      :canBeRemovedWithoutConfirmation="account => account.balance.amount > 0"
    >
      <AccountInput 
        :account="slotProps.item" 
        @changed="onAccountChanged($event, slotProps.index)"
      />
    </DynamicList>
  </div>
</template>
<script setup lang="ts">
import AccountInput from '@/components/AccountInput.vue';
import DynamicList from '@/components/DynamicList.vue';
import { Account } from '@/models/account';
import { computed } from 'vue';

const props = defineProps<{
  accounts: Account[],
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
</script>

<style lang="scss">
</style>
