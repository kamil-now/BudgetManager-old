<template>
  <div class="income-distribution-form">
    <div
      v-if="income"
      class="income-distribution-form_rules"
    >
      <div
        class="income-distribution-form_rule"
        v-for="rule in incomeDistributionRules"
        :key="rule.id"
      >
        <div class="income-distribution-form_rule-input">
          <IncomeDistributionRuleInput
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
    <div class="income-distribution-form_footer">
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
          props.incomeDistribution.rules.length === MAX_RULES
        "
        icon="pi pi-plus"
        text
        rounded
        aria-label="Add income distribution rule"
        @click="addRule()"
      />
    </div>
  </div>
</template>
<script setup lang="ts">
import IncomeDistributionRuleInput from '@/components/IncomeDistributionRuleInput.vue';
import MoneySpan from '@/components/MoneySpan.vue';
import { IncomeDistributionUtils } from '@/helpers/income-distribution-utils';
import { Fund } from '@/models/fund';
import { IncomeDistribution } from '@/models/income-distribution';
import { IncomeDistributionRule } from '@/models/income-distribution-rule';
import { IncomeDistributionRuleType } from '@/models/income-distribution-rule-type.enum';
import { Money } from '@/models/money';
import { useAppStore } from '@/store/store';
import { computed, ref, toRef, watch } from 'vue';

const MAX_RULES = 100;
const props = defineProps<{
  income: Money;
  incomeDistribution: IncomeDistribution;
}>();
const emit = defineEmits(['changed']);
const { funds } = useAppStore();
const leftover = ref({ ...props.income });

const incomeDistributionRules = toRef(props.incomeDistribution, 'rules');

const selectedFund = computed({
  get: () => props.incomeDistribution.defaultFundId
    ? funds.find((x) => x.id === props.incomeDistribution.defaultFundId)!
    : funds.find((x) => !!x.id)!,
  set: (newValue: Fund) => {
    emit('changed', {
      ...props.incomeDistribution,
      defaultFundId: newValue?.id,
      defaultFundName: newValue?.name,
    });
  }
});

watch(props, () => {
  updateCalculations();
});

const ruleCalculations = ref<{ [id: number]: string }>({});

function addRule() {
  (incomeDistributionRules.value = [
    ...props.incomeDistribution.rules,
    createNewIncomeDistributionRule(),
  ]),
  updateCalculations();
  emit('changed', {
    ...props.incomeDistribution,
    rules: incomeDistributionRules.value,
  });
}

function removeRule(rule: IncomeDistributionRule) {
  incomeDistributionRules.value = incomeDistributionRules.value.filter(
    (x) => x.id !== rule.id
  );
  updateCalculations();
  emit('changed', {
    ...props.incomeDistribution,
    rules: incomeDistributionRules.value,
  });
}

function createNewIncomeDistributionRule(): IncomeDistributionRule {
  const fund = funds.find(
    (x) => x.id !== props.incomeDistribution.defaultFundId
  );
  if (!fund) {
    throw new Error(
      'Income distribution is possible only when budget has multiple funds.'
    );
  }
  return {
    id: Date.now().valueOf(),
    type: IncomeDistributionRuleType.Fixed,
    value: leftover.value.amount,
    fundId: fund.id,
    fundName: fund.name,
  };
}

function onRuleChanged(rule: IncomeDistributionRule) {
  const index = incomeDistributionRules.value.findIndex(
    (x) => x.id === rule.id
  );
  incomeDistributionRules.value.splice(index, 1, rule);
  updateCalculations();
  emit('changed', {
    ...props.incomeDistribution,
    rules: incomeDistributionRules.value,
  });
}

function updateCalculations() {
  leftover.value.amount = props.income.amount;
  incomeDistributionRules.value.forEach((rule, index) => {
    if (leftover.value.amount < 0) {
      ruleCalculations.value[rule.id] = '-';
    } else {
      const { label, leftoverAmount } = IncomeDistributionUtils.calculate(
        index === 0 ? props.income.amount : leftover.value.amount,
        rule
      );
      ruleCalculations.value[rule.id] = label;
      leftover.value.amount = leftoverAmount;
    }
  });
}
</script>

<style lang="scss">
.income-distribution-form {
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
