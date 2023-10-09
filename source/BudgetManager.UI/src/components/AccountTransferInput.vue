<template>
  <div class="accountTransfer-input">
    <Calendar v-model="accountTransferDate" dateFormat="yy/mm/dd" />
    <InputText
      class="p-inputtext-sm"
      placeholder="AccountTransfer title"
      v-model="accountTransferTitle"
    />
    <Dropdown
      class="p-inputtext-sm"
      v-model="selectedSourceAccount"
      :options="accounts" 
    >
      <template #value="{ value }">
        <span>{{ value?.name }}</span>
      </template>
      <template #option="{ option }">
        <span>{{ option?.name }}</span>
      </template>
    </Dropdown>
    <Dropdown
      class="p-inputtext-sm"
      v-model="selectedTargetAccount"
      :options="accounts" 
    >
      <template #value="{ value }">
        <span>{{ value?.name }}</span>
      </template>
      <template #option="{ option }">
        <span>{{ option?.name }}</span>
      </template>
    </Dropdown>
    <Dropdown
      class="p-inputtext-sm"
      id="accountTransferCurrency" 
      v-model="accountTransferCurrency" 
      :options="currencyCodeList" 
    />
    <InputNumber 
      class="p-inputtext-sm"
      id="accountTransferValue"
      v-model="accountTransferValue" 
      mode="currency"
      currencyDisplay="code"
      :allowEmpty="false"
      :currency="accountTransferCurrency" 
      :min="0"
      :maxFractionDigits="2"
      :max="1000000000"
    />
  </div>
</template>
<script setup lang="ts">
import currencies from '@/assets/currencies.json';
import { DateUtils } from '@/helpers/date-utils';
import { Account } from '@/models/account';
import { AccountTransfer } from '@/models/account-transfer';
import { useAppStore } from '@/store/store';
import { computed, ref, watch } from 'vue';
const props = defineProps<{ accountTransfer: AccountTransfer }>();
const emit = defineEmits(['changed']);
const { accounts }  = useAppStore();
const currencyCodeList = Object.keys(currencies);

const selectedSourceAccount = ref<Account | undefined>(
  props.accountTransfer.accountId 
    ? accounts.find(x => x.id === props.accountTransfer.accountId)
    : undefined
);
watch(selectedSourceAccount, async (account) => {
  emit('changed', {
    ...props.accountTransfer, 
    accountId: account?.id,
    accountName: account?.name
  });
});

const selectedTargetAccount = ref<Account | undefined>(
  props.accountTransfer.targetAccountId 
    ? accounts.find(x => x.id === props.accountTransfer.targetAccountId)
    : undefined
);
watch(selectedTargetAccount, async (account) => {
  emit('changed', {
    ...props.accountTransfer, 
    targetAccountId: account?.id,
    targetAccountName: account?.name
  });
});

const accountTransferDate = computed({
  get: () => props.accountTransfer.date,
  set: (newValue) => {
    emit('changed', {
      ...props.accountTransfer, 
      date: DateUtils.createDateOnlyString(new Date(newValue))
    });
  }
});
const accountTransferTitle = computed({
  get: () => props.accountTransfer.title,
  set: (newValue) => {
    emit('changed', {
      ...props.accountTransfer, 
      title: newValue
    });
  }
});
const accountTransferValue = computed({
  get: () => props.accountTransfer.value.amount,
  set: (newValue) => {
    emit('changed', {
      ...props.accountTransfer, 
      value: {
        ...props.accountTransfer.value,
        amount: newValue
      }
    });
  }
});
const accountTransferCurrency = computed({
  get: () => props.accountTransfer.value.currency,
  set: (newValue) => {
    emit('changed', {
      ...props.accountTransfer, 
      value: {
        ...props.accountTransfer.value,
        currency: newValue
      }
    });
  }
});

</script>

<style lang="scss">
.accountTransfer-input {
  display: flex;
  max-width: 100%;
  flex-wrap: wrap;
  gap: 1rem;
}
</style>
