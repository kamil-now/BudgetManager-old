<template>
  <div class="allocation-input">
    <Calendar v-model="allocationDate" />
    <InputText
      ref="input"
      class="p-inputtext-sm"
      placeholder="Allocation title"
      v-model="allocationTitle"
    />
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
    <Dropdown
      class="p-inputtext-sm"
      id="allocationCurrency"
      v-model="allocationCurrency"
      :options="currencyCodeList"
    />
    <InputNumber
      class="p-inputtext-sm"
      id="allocationValue"
      v-model="allocationValue"
      mode="currency"
      currencyDisplay="code"
      :highlightOnFocus="true"
      :allowEmpty="false"
      :currency="allocationCurrency"
      :min="-1000000"
      :maxFractionDigits="2"
      :max="1000000"
    />
  </div>
</template>
<script setup lang="ts">
import currencies from '@/assets/currencies.json';
import { DateUtils } from '@/helpers/date-utils';
import { Allocation } from '@/models/allocation';
import { Fund } from '@/models/fund';
import { useAppStore } from '@/store/store';
import { computed, nextTick, onMounted, ref, watch } from 'vue';

const props = defineProps<{ allocation: Allocation }>();
const emit = defineEmits(['changed']);
const input = ref();

const { funds } = useAppStore();

const currencyCodeList = Object.keys(currencies);
onMounted(() =>
  nextTick(() => {
    input.value.$el.focus();
  })
);

const selectedFund = ref<Fund | undefined>(
  props.allocation.targetFundId
    ? funds.find((x) => x.id === props.allocation.targetFundId)
    : undefined
);

watch(selectedFund, async (fund) => {
  emit('changed', {
    ...props.allocation,
    targetFundId: fund?.id,
    targetFundName: fund?.name,
  });
});

const allocationDate = computed({
  get: () => props.allocation.date,
  set: (newValue) => {
    emit('changed', {
      ...props.allocation,
      date: DateUtils.createDateOnlyString(new Date(newValue)),
    });
  },
});
const allocationTitle = computed({
  get: () => props.allocation.title,
  set: (newValue) => {
    emit('changed', {
      ...props.allocation,
      title: newValue,
    });
  },
});
const allocationValue = computed({
  get: () => props.allocation.value.amount,
  set: (newValue) => {
    emit('changed', {
      ...props.allocation,
      value: {
        ...props.allocation.value,
        amount: newValue,
      },
    });
  },
});

const allocationCurrency = computed({
  get: () => props.allocation.value.currency,
  set: (newValue) => {
    emit('changed', {
      ...props.allocation,
      value: {
        ...props.allocation.value,
        currency: newValue,
      },
    });
  },
});
</script>

<style lang="scss">
.allocation-input {
  display: flex;
  max-width: 100%;
  flex-wrap: wrap;
  gap: 1rem;
}
</style>
