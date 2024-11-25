<template>
  <div class="income-allocation-form">
    <div
      v-if="income"
      class="income-allocation-form_rules"
    >
      <div
        class="income-allocation-form_rule"
        v-for="rule in incomeAllocationRules"
        :key="rule.id"
      >
        <div class="income-allocation-form_rule-input">
          <IncomeAllocationRuleInput
            :rule="rule"
            :currency="leftover.currency"
            @changed="onRuleChanged($event)"
          />
          <Button
            icon="pi pi-times"
            severity="danger"
            text
            rounded
            size="small"
            aria-label="Remove"
            @click="removeRule(rule)"
          />
        </div>
        <span>{{ ruleCalculations[rule.id] }}</span>
      </div>
    </div>
    <div class="income-allocation-form_footer">
      <MoneySpan :money="leftover" />
      <i class="pi pi-arrow-right" />
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
      <Button
        v-if="
          funds.length > 1 ||
          props.incomeAllocation.rules.length === MAX_RULES
        "
        icon="pi pi-plus"
        text
        rounded
        aria-label="Add income allocation rule"
        @click="addRule()"
      />
    </div>
  </div>
</template>
<script setup lang="ts">
import MoneySpan from '@/components/MoneySpan.vue';
import IncomeAllocationRuleInput from './IncomeAllocationRuleInput.vue';
import { IncomeAllocationUtils } from '@/helpers/income-allocation-utils';
import { Fund } from '@/models/fund';
import { IncomeAllocation } from '@/models/income-allocation';
import { IncomeAllocationRule } from '@/models/income-allocation-rule';
import { IncomeAllocationRuleType } from '@/models/income-allocation-rule-type.enum';
import { Money } from '@/models/money';
import { useAppStore } from '@/store/store';
import { computed, ref, toRef, watch } from 'vue';

const MAX_RULES = 100;
const props = defineProps<{
  income: Money;
  incomeAllocation: IncomeAllocation;
}>();
const emit = defineEmits(['changed']);
const { funds } = useAppStore();
const leftover = ref({ ...props.income });

const incomeAllocationRules = toRef(props.incomeAllocation, 'rules');

const selectedFund = computed({
  get: () => props.incomeAllocation.defaultFundId
    ? funds.find((x) => x.id === props.incomeAllocation.defaultFundId)!
    : funds.find((x) => !!x.id)!,
  set: (newValue: Fund) => {
    emit('changed', {
      ...props.incomeAllocation,
      defaultFundId: newValue?.id,
      defaultFundName: newValue?.name,
    });
  }
});

watch(props, () => {
  updateCalculations();
});

const ruleCalculations = ref<{ [id: string]: string }>({});

function addRule() {
  (incomeAllocationRules.value = [
    ...props.incomeAllocation.rules,
    createNewIncomeAllocationRule(),
  ]),
  updateCalculations();
  emit('changed', {
    ...props.incomeAllocation,
    rules: incomeAllocationRules.value,
  });
}

function removeRule(rule: IncomeAllocationRule) {
  incomeAllocationRules.value = incomeAllocationRules.value.filter(
    (x) => x.id !== rule.id
  );
  updateCalculations();
  emit('changed', {
    ...props.incomeAllocation,
    rules: incomeAllocationRules.value,
  });
}

function createNewIncomeAllocationRule(): IncomeAllocationRule {
  const fund = funds.find(
    (x) => x.id !== props.incomeAllocation.defaultFundId
  );
  if (!fund) {
    throw new Error(
      'Income allocation is possible only when budget has multiple funds.'
    );
  }
  return {
    id: Date
      .now()
      .valueOf()
      .toString(),
    type: IncomeAllocationRuleType.Fixed,
    value: leftover.value.amount,
    fundId: fund.id,
    fundName: fund.name,
  };
}

function onRuleChanged(rule: IncomeAllocationRule) {
  const index = incomeAllocationRules.value.findIndex(
    (x) => x.id === rule.id
  );
  incomeAllocationRules.value.splice(index, 1, rule);
  updateCalculations();
  emit('changed', {
    ...props.incomeAllocation,
    rules: incomeAllocationRules.value,
  });
}

function updateCalculations() {
  leftover.value.amount = props.income.amount;
  incomeAllocationRules.value.forEach((rule, index) => {
    if (leftover.value.amount < 0) {
      ruleCalculations.value[rule.id] = '-';
    } else {
      const { label, leftoverAmount } = IncomeAllocationUtils.calculate(
        index === 0 ? props.income.amount : leftover.value.amount,
        rule
      );
      ruleCalculations.value[rule.id] = label;
      leftover.value.amount = leftoverAmount;
    }
  });
}
// function updateCalculations() {
//   // percent rules should take the same base value as the percent rules directly before them
//   /* 
//     e.g. for income 1500 and rules:
//     1: allocates fixed 200 to fund A
//     2: allocates 30% to fund B
//     3: allocates 70% to fund C
//     4: fixed 300 to fund D
//     4: leftover to fund E
//     then allocations should be 
//     200 -> fund A
//     300 (30% of 1000) -> fund B
//     700 (70% of 1000) -> fund C
//     300 -> fund D
//     0 leftover to E
//   */
//   let leftoverAfterLastFixedAllocation = props.income.amount;
//   leftover.value.amount = props.income.amount;
//   incomeAllocationRules.value.forEach(rule => {
//     if (leftover.value.amount < 0) {
//       ruleCalculations.value[rule.id] = '-';
//     } else {
//       const { label, leftoverAmount } = IncomeAllocationUtils.calculate(
//         rule.type === IncomeAllocationRuleType.Percent ? leftoverAfterLastFixedAllocation : leftover.value.amount,
//         rule
//       );
//       ruleCalculations.value[rule.id] = label;
//       if (rule.type === IncomeAllocationRuleType.Fixed) {
//         leftover.value.amount = leftoverAmount;
//         leftoverAfterLastFixedAllocation = leftoverAmount;
//       } else {
//         // TODO
//         leftover.value.amount = leftover.value.amount - leftoverAfterLastFixedAllocation - leftoverAmount;
//       }
//     }
//   });
// }
</script>

<style lang="scss">
.income-allocation-form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  &_rules {
    display: flex;
    flex-direction: column;
    max-width: 100%;
    flex-wrap: wrap;
    gap: 1rem;
  }
  &_rule {
    display: flex;
    align-items: start;
    gap: 1rem;
    flex-wrap: wrap;
    flex-direction: column;
    span {
      text-wrap: nowrap;
    }
    &-input {
      display: flex;
      align-items: center;
    }
  }
  &_footer {
    width: 100%;
    display: flex;
    align-items: center;
    gap: 1rem;
  }
}
</style>
