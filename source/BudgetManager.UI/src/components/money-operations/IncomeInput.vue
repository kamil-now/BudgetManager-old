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
    <div class="income-input_distribute-income-checkbox">
      <Checkbox id="distributeIncomeCheckbox" v-model="distributeIncome" :binary="true"></Checkbox>
      <label for="distributeIncomeCheckbox"> Allocate income </label>
    </div>
    <IncomeDistributionForm
      v-if="distributeIncome"
      :incomeDistribution="incomeDistribution"
      :income="incomeValue"
      @changed="onIncomeDistributionChange($event)"
    ></IncomeDistributionForm>
  </div>
</template>
<script setup lang="ts">
import IncomeDistributionForm from '@/components/money-operations/IncomeDistributionForm.vue';
import MoneyInput from '@/components/money-operations/MoneyInput.vue';
import { DateUtils } from '@/helpers/date-utils';
import { Account } from '@/models/account';
import { Income } from '@/models/income';
import { useAppStore } from '@/store/store';
import { computed, ref, watch } from 'vue';
import { IncomeDistribution } from '@/models/income-distribution';
import { IncomeDistributionRule } from '@/models/income-distribution-rule';
import { saveIncomeDistributionPreference, getIncomeDistributionPreference } from '@/storage';

const props = defineProps<{ income: Income }>();
const emit = defineEmits(['changed']);
const { accounts, funds } = useAppStore();

const incomeRef = ref<Income>(props.income);
const distributeIncomePreferenceRef = ref<boolean>(getIncomeDistributionPreference());
const distributeIncome = computed({
  get: () => distributeIncomePreferenceRef.value,
  set: (newValue) => {
    saveIncomeDistributionPreference(newValue);
    distributeIncomePreferenceRef.value = newValue;
  }
});
const incomeDistribution = ref<IncomeDistribution>({ defaultFundId: funds[0].id,  rules: [] as IncomeDistributionRule[] });

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

function onIncomeDistributionChange(
  changedIncomeDistribution: IncomeDistribution
) {
  // TODO
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
  &_distribute-income-checkbox {
    display: flex;
    align-items: center;
    gap: 1rem;
  }
}
</style>
