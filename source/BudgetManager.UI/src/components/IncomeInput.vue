<template>
  <div class="income-input">
    <div class="income-input_content">
      <Calendar
        v-model="incomeDate"
        dateFormat="yy/mm/dd"
      />
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
    <IncomeDistributionForm
      v-if="incomeValue.amount > 0"
      :incomeDistribution="incomeDistribution"
      :income="income.value"
      @changed="onIncomeDistributionChange($event)"
    ></IncomeDistributionForm>
  </div>
</template>
<script setup lang="ts">
import IncomeDistributionForm from '@/components/IncomeDistributionForm.vue';
import MoneyInput from '@/components/MoneyInput.vue';
import { DateUtils } from '@/helpers/date-utils';
import { Account } from '@/models/account';
import { Income } from '@/models/income';
import { useAppStore } from '@/store/store';
import { computed, ref, watch } from 'vue';
import { IncomeDistribution } from '@/models/income-distribution';
import { IncomeDistributionRule } from '@/models/income-distribution-rule';

const props = defineProps<{ income: Income }>();
const emit = defineEmits(['changed']);
const { accounts } = useAppStore();
const incomeDistribution = ref<IncomeDistribution>({
  rules: [] as IncomeDistributionRule[],
});

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
  get: () => props.income.value,
  set: (newValue) => {
    emit('changed', {
      ...props.income,
      value: { ...newValue },
    });
  },
});

function onIncomeDistributionChange(
  changedIncomeDistribution: IncomeDistribution
) {
  incomeDistribution.value = changedIncomeDistribution;
}
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
  .income-distribution-form {
    border-top: 1px solid black;
    padding-top: 1rem;
  }
}
</style>
