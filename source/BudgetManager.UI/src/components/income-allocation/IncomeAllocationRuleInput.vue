<template>
  <div class="income-allocation-rule-input">
    <InputGroup>
      <InputGroupAddon
        :class="{ selected: ruleType === IncomeAllocationRuleType.Fixed }"
      >
        <i
          class="pi pi-money-bill"
          @click="ruleType = IncomeAllocationRuleType.Fixed"
        />
      </InputGroupAddon>
      <InputGroupAddon
        :class="{ selected: ruleType === IncomeAllocationRuleType.Percent }"
      >
        <i
          class="pi pi-percentage"
          @click="ruleType = IncomeAllocationRuleType.Percent"
        />
      </InputGroupAddon>
      <InputNumber
        v-model="ruleValue"
        :allowEmpty="false"
        :highlightOnFocus="true"
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
import { IncomeAllocationRule } from '@/models/income-allocation-rule';
import { IncomeAllocationRuleType } from '@/models/income-allocation-rule-type.enum';
import { useAppStore } from '@/store/store';
import { InputNumberInputEvent } from 'primevue/inputnumber';
import { computed, toRefs } from 'vue';

const props = defineProps<{ currency: string; rule: IncomeAllocationRule }>();
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
  set: (newValue: IncomeAllocationRuleType) => {
    rule.value.type = newValue;
    emit('changed', {
      ...rule.value,
      type: newValue,
    });
  },
});

const ruleValue = computed({
  get: () => rule.value.value,
  set: (newValue: number) => {
    rule.value.value = newValue;
    emit('changed', {
      ...rule.value,
      value: newValue,
    });
  }
});

const valueSuffix = computed(() => ruleType.value === IncomeAllocationRuleType.Percent ? ' %' : '');
const valuePrefix = computed(() => ruleType.value === IncomeAllocationRuleType.Fixed ? `${props.currency} ` : '');

function onValueInput(event: InputNumberInputEvent) {
  let newValue = Number(event.value);
  if (ruleType.value === IncomeAllocationRuleType.Percent && newValue > 100) {
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
.income-allocation-rule-input {
  display: flex;
  gap: 1rem;
  align-items: center;

  .p-inputgroup {
    width: 10rem;
  }
  .p-inputgroup-addon {
    padding: 0;
  }

  i {
    font-size: 0.75rem;
    cursor: pointer;
  }
  .selected {
    background-color: var(--primary-color);
  }
}
</style>
