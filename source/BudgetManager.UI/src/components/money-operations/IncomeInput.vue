<template>
  <div class="income-input">
    <Calendar v-model="incomeDate" />
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
    <MoneyInput
      :money="incomeValue"
      @changed="incomeValue = $event"
    />
  </div>
</template>
<script setup lang="ts">
import MoneyInput from '@/components/money-operations/MoneyInput.vue';
import { DateUtils } from '@/helpers/date-utils';
import { Account } from '@/models/account';
import { Income } from '@/models/income';
import { useAppStore } from '@/store/store';
import { computed, ref, watch } from 'vue';

const props = defineProps<{ income: Income }>();
const emit = defineEmits(['changed']);
const { accounts } = useAppStore();

const incomeRef = ref<Income>(props.income);
const selectedAccount = ref<Account | undefined>(
  props.income.accountId
    ? accounts.find((x) => x.id === props.income.accountId)
    : undefined
);


watch(selectedAccount, async (account) => {
  emit('changed', {
    ...props.income,
    accountId: account?.id,
    accountName: account?.name,
  });
});

const incomeDate = computed({
  get: () => props.income.date,
  set: (newValue) => {
    emit('changed', {
      ...props.income,
      date: DateUtils.createDateOnlyString(new Date(newValue)),
    });
  },
});
const incomeTitle = computed({
  get: () => props.income.title,
  set: (newValue) => {
    emit('changed', {
      ...props.income,
      title: newValue,
    });
  },
});
const incomeValue = computed({
  get: () => incomeRef.value.value,
  set: (newValue) => {
    incomeRef.value.value = { ...newValue };
    emit('changed', {
      ...props.income,
      value: { ...newValue },
    });
  },
});
</script>

<style lang="scss">
.income-input {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
}
</style>
