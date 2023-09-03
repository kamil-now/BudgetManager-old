<template>
  <div class="income-input">
    <Calendar v-model="incomeDate" dateFormat="dd/mm/yy" />
    <InputText
      class="p-inputtext-sm"
      placeholder="Income title"
      v-model="incomeTitle"
    />
    <Dropdown
      class="p-inputtext-sm"
      v-model="selectedFund"
      :options="funds" 
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
      v-model="selectedAccount"
      :options="accounts" 
    >
      <template #value="{ value }">
        <span>{{ value?.name }}</span>
      </template>
      <template #option="{ option }">
        <span>{{ option?.name }}</span>
      </template>
    </Dropdown>
    <InputNumber 
      v-if="selectedAccount"
      class="p-inputtext-sm"
      id="accountBalance"
      v-model="incomeValue" 
      mode="currency"
      currencyDisplay="code"
      :allowEmpty="false"
      :currency="selectedAccount?.balance.currency" 
      :min="0"
      :maxFractionDigits="2"
      :max="1000000000"
    />
  </div>
</template>
<script setup lang="ts">
import { Account } from '@/models/account';
import { Fund } from '@/models/fund';
import { Income } from '@/models/income';
import { useAppStore } from '@/store/store';
import { computed, ref, watch } from 'vue';
const props = defineProps<{ income: Income }>();
const emit = defineEmits(['changed']);
const { accounts, funds }  = useAppStore();

const selectedAccount = ref<Account | undefined>(
  props.income.accountId 
    ? accounts.find(x => x.id === props.income.accountId)
    : undefined
);

watch(selectedAccount, async (account) => {
  emit('changed', {
    ...props.income, 
    accountId: account?.id,
    value: {
      ...props.income.value,
      currency: account?.balance.currency
    }
  });
});

const selectedFund = ref<Fund | undefined>(
  props.income.fundId 
    ? funds.find(x => x.id === props.income.fundId)
    : undefined
);

watch(selectedFund, async (fund) => {
  emit('changed', {
    ...props.income, 
    fundId: fund?.id
  });
});

const incomeDate = computed({
  get: () => props.income.date,
  set: (newValue) => {
    emit('changed', {
      ...props.income, 
      date: new Date(newValue).toLocaleDateString()
    });
  }
});
const incomeTitle = computed({
  get: () => props.income.title,
  set: (newValue) => {
    emit('changed', {
      ...props.income, 
      title: newValue
    });
  }
});
const incomeValue = computed({
  get: () => props.income.value.amount,
  set: (newValue) => {
    emit('changed', {
      ...props.income, 
      value: {
        ...props.income.value,
        amount: newValue
      }
    });
  }
});

</script>

<style lang="scss">
.income-input {
  display: flex;
  max-width: 100%;
  flex-wrap: wrap;
  gap: 1rem;
}
</style>
