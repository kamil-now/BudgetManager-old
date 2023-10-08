<template>
  <div class="income-input">
    <Calendar v-model="incomeDate" dateFormat="yy/mm/dd" />
    <InputText
      class="p-inputtext-sm"
      placeholder="Income title"
      v-model="incomeTitle"
    />
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
      v-model="incomeValue" 
      mode="currency"
      currencyDisplay="code"
      :allowEmpty="false"
      :currency="incomeCurrency" 
      :min="0"
      :maxFractionDigits="2"
      :max="1000000000"
    />
    <Dropdown
      class="p-inputtext-sm"
      id="selectedCurrency" 
      v-model="incomeCurrency" 
      :options="currencyCodeList" 
    />
  </div>
</template>
<script setup lang="ts">
import currencies from '@/assets/currencies.json';
import { DateUtils } from '@/helpers/date-utils';
import { Account } from '@/models/account';
import { Income } from '@/models/income';
import { useAppStore } from '@/store/store';
import { computed, ref, watch } from 'vue';
const props = defineProps<{ income: Income }>();
const emit = defineEmits(['changed']);
const { accounts }  = useAppStore();

const currencyCodeList = Object.keys(currencies);
const selectedAccount = ref<Account | undefined>(
  props.income.accountId 
    ? accounts.find(x => x.id === props.income.accountId)
    : undefined
);

watch(selectedAccount, async (account) => {
  emit('changed', {
    ...props.income, 
    accountId: account?.id,
    accountName: account?.name
  });
});

const incomeDate = computed({
  get: () => props.income.date,
  set: (newValue) => {
    emit('changed', {
      ...props.income, 
      date: DateUtils.createDateOnlyString(new Date(newValue))
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
const incomeCurrency = computed({
  get: () => props.income.value.currency,
  set: (newValue) => {
    emit('changed', {
      ...props.income, 
      value: {
        ...props.income.value,
        currency: newValue
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
