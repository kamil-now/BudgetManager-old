<template>
  <div class="income-distribution-form">
    <div v-if="income" class="income-distribution-form_rules">
      <div
        class="income-distribution-form_rule"
        v-for="rule in incomeDistribution.rules"
        :key="rule.id"
      >
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
        <span>{{ ruleCalculations[rule.id] }}</span>
      </div>
    </div>
    <div class="income-distribution-form_footer">
      <span
        class="money"
        v-if="incomeDistribution.rules.length === 0"
      >
        {{ income !== undefined ? DisplayFormat.money(income) : "All" }}
      </span>
      <span
        class="money"
        v-else
      >
        {{ income !== undefined ? DisplayFormat.money(leftover) : "Rest" }}
      </span>
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
        v-if="funds.length > 1 || props.incomeDistribution.rules.length === MAX_RULES"
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
import { DisplayFormat } from '@/helpers/display-format';
import { IncomeDistributionUtils } from '@/helpers/income-distribution-utils';
import { Fund } from '@/models/fund';
import { IncomeDistribution } from '@/models/income-distribution';
import { IncomeDistributionRule } from '@/models/income-distribution-rule';
import { IncomeDistributionRuleType } from '@/models/income-distribution-rule-type.enum';
import { Money } from '@/models/money';
import { useAppStore } from '@/store/store';
import { computed, ref, watch } from 'vue';

const MAX_RULES = 100;
const props = defineProps<{
  income: Money;
  incomeDistribution: IncomeDistribution;
}>();
const emit = defineEmits(['changed']);
const { funds } = useAppStore();

const leftover = ref<Money>({ ...props.income });

const selectedFund = ref<Fund | undefined>(
  props.incomeDistribution.defaultFundId
    ? funds.find((x) => x.id === props.incomeDistribution.defaultFundId)
    : funds.find((x) => !!x.id)
);


watch(selectedFund, async (fund) => {
  emit('changed', {
    ...props.incomeDistribution,
    defaultFundId: fund?.id,
    defaultFundName: fund?.name,
  });
});

const ruleCalculations = computed<{[id: number]: string}>({
  get: () => {
    const calculations:{[id: number]: string} = {};
    let leftover = props.income.amount;
    
    props.incomeDistribution.rules.forEach((rule) => {
      if (leftover < 0) {
        calculations[rule.id] = '-';
      } else {
        const { label, leftoverAmount } = IncomeDistributionUtils.calculate(leftover, rule);
        calculations[rule.id] = label;
        leftover = leftoverAmount;
      }
    });
    return calculations;
  },
  set: () => {}
});

function addRule() {
  emit('changed', {
    ...props.incomeDistribution,
    rules: [
      ...props.incomeDistribution.rules,
      createNewIncomeDistributionRule(),
    ],
  });
}

function removeRule(rule: IncomeDistributionRule) {
  emit('changed', {
    ...props.incomeDistribution,
    rules: props.incomeDistribution.rules.filter((x) => x.id !== rule.id),
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
    name: 'Rule ' + (props.incomeDistribution.rules.length + 1),
    type: IncomeDistributionRuleType.Percent,
    value: props.income ? (leftover.value.amount / props.income.amount * 100) : 0,
    fundId: fund.id,
    fundName: fund.name,
  };
}

function onRuleChanged(rule: IncomeDistributionRule) {
  const index = props.incomeDistribution.rules.findIndex(
    (x) => x.id === rule.id
  );
  const rules = [...props.incomeDistribution.rules];
  updateCalculations(rules);
  rules.splice(index, 1, rule);
  emit('changed', {
    ...props.incomeDistribution,
    rules,
  });
}

function updateCalculations(rules: IncomeDistributionRule[]) {
  leftover.value.amount = props.income.amount;
  rules.forEach((rule, index) => {
    if (leftover.value.amount < 0) {
      ruleCalculations.value[rule.id] = '-';
    } else {
      const { label, leftoverAmount } = IncomeDistributionUtils.calculate(index === 0 ? props.income.amount : leftover.value.amount, rule);
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
    align-items: center;
    flex-wrap: wrap;
    span {
      text-wrap: nowrap;
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
