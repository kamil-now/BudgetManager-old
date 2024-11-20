<template>
  <div class="income-input">
    <div class="income-input_content">
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
    <div class="income-input_distribute-income-checkbox">
      <Checkbox
        id="distributeIncomeCheckbox"
        v-model="distributeIncome"
        :binary="true"
      ></Checkbox>
      <label for="distributeIncomeCheckbox">Allocate income</label>
    </div>
    <IncomeAllocationForm
      v-if="distributeIncome"
      :incomeAllocation="incomeAllocation"
      :income="incomeValue"
    ></IncomeAllocationForm>
  </div>
</template>
<script setup lang="ts">
import IncomeAllocationForm from '@/components/money-operations/IncomeAllocationForm.vue';
import MoneyInput from '@/components/money-operations/MoneyInput.vue';
import { DateUtils } from '@/helpers/date-utils';
import { Account } from '@/models/account';
import { Income } from '@/models/income';
import { useAppStore } from '@/store/store';
import { computed, ref, watch } from 'vue';
import { IncomeAllocation } from '@/models/income-allocation';
import { saveIncomeAllocationPreference, getIncomeAllocationPreference } from '@/storage';

const props = defineProps<{ income: Income }>();
const emit = defineEmits(['changed']);
const { accounts, incomeAllocationTemplates } = useAppStore();

const incomeRef = ref<Income>(props.income);
const distributeIncomePreferenceRef = ref<boolean>(getIncomeAllocationPreference());
const distributeIncome = computed({
  get: () => distributeIncomePreferenceRef.value,
  set: (newValue) => {
    saveIncomeAllocationPreference(newValue);
    distributeIncomePreferenceRef.value = newValue;
  },
});
const incomeAllocation = ref<IncomeAllocation>(incomeAllocationTemplates[0]);

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
  flex-direction: column;
  gap: 1rem;

  &_content {
    display: flex;
    flex-wrap: wrap;
    gap: 1rem;
  }
  .income-allocation-form {
    border-top: 1px solid black;
    padding-top: 1rem;
  }

  &_distribute-income-checkbox {
    display: flex;
    align-items: center;
    gap: 1rem;
  }
}
</style>
