<template>
  <div class="expense-input">
    <Calendar v-model="expenseDate" dateFormat="dd/mm/yy" />
    <InputText
      class="p-inputtext-sm"
      placeholder="Expense title"
      v-model="expenseTitle"
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
      class="p-inputtext-sm"
      id="accountBalance"
      v-model="expenseValue" 
      mode="currency"
      currencyDisplay="code"
      :allowEmpty="false"
      :currency="expenseCurrency" 
      :min="0"
      :maxFractionDigits="2"
      :max="1000000000"
    />
    <Dropdown
      class="p-inputtext-sm"
      id="selectedCurrency" 
      v-model="expenseCurrency" 
      :options="currencyCodeList" 
    />
  </div>
</template>
<script setup lang="ts">
import currencies from '@/assets/currencies.json';
import { Account } from '@/models/account';
import { Fund } from '@/models/fund';
import { Expense } from '@/models/expense';
import { useAppStore } from '@/store/store';
import { computed, ref, watch } from 'vue';
const props = defineProps<{ expense: Expense }>();
const emit = defineEmits(['changed']);
const { accounts, funds }  = useAppStore();

const currencyCodeList = Object.keys(currencies);
const selectedAccount = ref<Account | undefined>(
  props.expense.accountId 
    ? accounts.find(x => x.id === props.expense.accountId)
    : undefined
);

watch(selectedAccount, async (account) => {
  emit('changed', {
    ...props.expense, 
    accountId: account?.id,
    accountName: account?.name
  });
});

const selectedFund = ref<Fund | undefined>(
  props.expense.fundId 
    ? funds.find(x => x.id === props.expense.fundId)
    : undefined
);

watch(selectedFund, async (fund) => {
  emit('changed', {
    ...props.expense, 
    fundId: fund?.id,
    fundName: fund?.name
  });
});
const expenseDate = computed({
  get: () => props.expense.date,
  set: (newValue) => {
    emit('changed', {
      ...props.expense, 
      date: new Date(newValue)
    });
  }
});
const expenseTitle = computed({
  get: () => props.expense.title,
  set: (newValue) => {
    emit('changed', {
      ...props.expense, 
      title: newValue
    });
  }
});
const expenseValue = computed({
  get: () => props.expense.value.amount,
  set: (newValue) => {
    emit('changed', {
      ...props.expense, 
      value: {
        ...props.expense.value,
        amount: newValue
      }
    });
  }
});
const expenseCurrency = computed({
  get: () => props.expense.value.currency,
  set: (newValue) => {
    emit('changed', {
      ...props.expense, 
      value: {
        ...props.expense.value,
        currency: newValue
      }
    });
  }
});

</script>

<style lang="scss">
.expense-input {
  display: flex;
  max-width: 100%;
  flex-wrap: wrap;
  gap: 1rem;
}
</style>
