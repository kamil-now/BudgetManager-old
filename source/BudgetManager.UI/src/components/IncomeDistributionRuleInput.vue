<template>
  <div class="income-distribution-rule-input">
    <InputText
      ref="input"
      class="p-inputtext-sm"
      placeholder="Rule name"
      v-model="name"
    />
    <Dropdown
      v-model="type"
      :options="incomeDistributionRuleTypes"
      style="width: 8rem"
    >
      <template #value="{ value }">
        <span :class="{ placeholder: value === 0}">
          {{ value === 0 ? "type" : IncomeDistributionRuleType[value] }}
        </span>
      </template>
      <template #option="{ option }">
        <span>{{ IncomeDistributionRuleType[option] }}</span>
      </template>
    </Dropdown>

    <MoneyInput
      v-if="type == IncomeDistributionRuleType.Fixed"
      :money="ruleValue"
    />

    <InputNumber
      v-else
      class="p-inputtext-sm"
      v-model="ruleValue.amount"
      :allowEmpty="false"
      :highlightOnFocus="true"
      :min="0"
      suffix=" %"
      :maxFractionDigits="2"
      :max="1"
    />
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
  </div>
</template>
<script setup lang="ts">
import { EnumUtils } from '@/helpers/enum-utils';
import { Fund } from '@/models/fund';
import MoneyInput from '@/components/MoneyInput.vue';
import { IncomeDistributionRule } from '@/models/income-distribution-rule';
import { IncomeDistributionRuleType } from '@/models/income-distribution-rule-type.enum';
import { useAppStore } from '@/store/store';
import { computed, ref, watch } from 'vue';

const props = defineProps<{ currency: string; rule: IncomeDistributionRule }>();
const emit = defineEmits(['changed']);
const { funds } = useAppStore();
const incomeDistributionRuleTypes = EnumUtils.getStringValues(
  IncomeDistributionRuleType
);
const selectedFund = ref<Fund | undefined>(
  props.rule.fundId
    ? funds.find((x) => x.id === props.rule.fundId)
    : funds.find((x) => !!x.id)
);

watch(selectedFund, async (fund) => {
  emit('changed', {
    ...props.rule,
    fundId: fund?.id,
    fundName: fund?.name,
  });
});
const ruleValue = computed({
  get: () => ({ amount: props.rule.value, currency: props.currency }),
  set: (newValue) => {
    emit('changed', {
      ...props.rule,
      value: newValue.amount,
    });
  },
});
const name = computed({
  get: () => props.rule.name,
  set: (newValue) => {
    emit('changed', {
      ...props.rule,
      name: newValue,
    });
  },
});
const type = computed({
  get: () => props.rule.type,
  set: (newValue: number) => {
    emit('changed', {
      ...props.rule,
      type: IncomeDistributionRuleType[
        IncomeDistributionRuleType[
          newValue
        ] as keyof typeof IncomeDistributionRuleType
      ],
    });
  },
});
</script>

<style lang="scss">
.income-distribution-rule-input {
  display: flex;
  max-width: 100%;
  flex-wrap: wrap;
  gap: 1rem;
  align-items: center;
}
</style>
