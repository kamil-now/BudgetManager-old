<template>
  <div class="income-distribution-rule-input">
    <InputGroup>
      <InputGroupAddon
        :class="{ selected: ruleType === IncomeDistributionRuleType.Fixed }"
      >
        <i
          class="pi pi-money-bill"
          @click="ruleType = IncomeDistributionRuleType.Fixed"
        />
      </InputGroupAddon>
      <InputGroupAddon
        :class="{ selected: ruleType === IncomeDistributionRuleType.Percent }"
      >
        <i
          class="pi pi-percentage"
          @click="ruleType = IncomeDistributionRuleType.Percent"
        />
      </InputGroupAddon>
      <InputNumber
        v-model="ruleValue"
        :allowEmpty="false"
        :highlightOnFocus="true"
        :min="0"
        :suffix="valueSuffix"
        :prefix="valuePrefix"
        :maxFractionDigits="2"
        @input="onValueInput($event)"
      />
    </InputGroup>
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
import { Fund } from '@/models/fund';
import { IncomeDistributionRule } from '@/models/income-distribution-rule';
import { IncomeDistributionRuleType } from '@/models/income-distribution-rule-type.enum';
import { useAppStore } from '@/store/store';
import { InputNumberInputEvent } from 'primevue/inputnumber';
import { computed, toRefs } from 'vue';

const props = defineProps<{ currency: string; rule: IncomeDistributionRule }>();
const { rule } = toRefs(props);
const emit = defineEmits(['changed']);
const { funds } = useAppStore();

const selectedFund = computed({
  get: () =>
    props.rule.fundId
      ? funds.find((x) => x.id === props.rule.fundId)!
      : funds.find((x) => !!x.id)!,
  set: (fund: Fund) => {
    emit('changed', {
      ...rule.value,
      fundId: fund?.id,
      fundName: fund?.name,
    });
  },
});

const ruleType = computed({
  get: () => rule.value.type,
  set: (newValue: IncomeDistributionRuleType) => {
    rule.value.type = newValue;
    emit('changed', {
      ...rule.value,
      type: newValue,
    });
  },
});

const ruleValue = computed(() => rule.value.value);

const valueSuffix = computed(() => ruleType.value === IncomeDistributionRuleType.Percent ? ' %' : '');
const valuePrefix = computed(() => ruleType.value === IncomeDistributionRuleType.Fixed ? `${props.currency} ` : '');

function onValueInput(event: InputNumberInputEvent) {
  let newValue = Number(event.value);
  if (ruleType.value === IncomeDistributionRuleType.Percent && newValue > 100) {
    newValue = 100;
  } else if (newValue < 0) {
    newValue = 0;
  }

  rule.value.value = newValue;
  emit('changed', {
    ...rule.value,
    value: newValue,
  });
}
</script>

<style lang="scss">
.income-distribution-rule-input {
  display: flex;
  gap: 1rem;
  align-items: center;

  i {
    font-size: 0.75rem;
    cursor: pointer;
  }
  .selected {
    background-color: var(--primary-color);
  }
}
</style>
