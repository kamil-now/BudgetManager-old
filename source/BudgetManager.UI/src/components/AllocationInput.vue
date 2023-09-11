<template>
  <div class="allocation-input">
    <Calendar v-model="allocationDate" dateFormat="dd/mm/yy" />
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
    <InputNumber 
      v-if="selectedFund && budgetBalance"
      class="p-inputtext-sm"
      id="allocationValue"
      v-model="allocationValue" 
      mode="currency"
      currencyDisplay="code"
      :allowEmpty="false"
      :currency="allocationCurrency" 
      :min="-1000000"
      :maxFractionDigits="2"
      :max="1000000"
    />
    <Dropdown
      class="p-inputtext-sm"
      id="allocationCurrency" 
      v-model="allocationCurrency" 
      :options="currencyCodeList" 
    />
  </div>
</template>
<script setup lang="ts">
import currencies from '@/assets/currencies.json';
import { Allocation } from '@/models/allocation';
import { Fund } from '@/models/fund';
import { useAppStore } from '@/store/store';
import { computed, nextTick, onMounted, ref, watch } from 'vue';

const props = defineProps<{ allocation: Allocation }>();
const emit = defineEmits(['changed']);
const input = ref();

const { funds, budgetBalance }  = useAppStore();

const currencyCodeList = Object.keys(currencies);
onMounted(() => nextTick(() => {
  input.value.$el.focus();
}));

const selectedFund = ref<Fund | undefined>(
  props.allocation.targetFundId 
    ? funds.find(x => x.id === props.allocation.targetFundId)
    : undefined
);

watch(selectedFund, async (fund) => {
  emit('changed', {
    ...props.allocation, 
    targetFundId: fund?.id
  });
});

const allocationDate = computed({
  get: () => props.allocation.date,
  set: (newValue) => {
    emit('changed', {
      ...props.allocation, 
      date: new Date(newValue)
    });
  }
});
const allocationTitle = computed({
  get: () => props.allocation.title,
  set: (newValue) => {
    emit('changed', {
      ...props.allocation, 
      title: newValue
    });
  }
});
const allocationValue = computed({
  get: () => props.allocation.value.amount,
  set: (newValue) => {
    emit('changed', {
      ...props.allocation, 
      value: {
        ...props.allocation.value,
        amount: newValue
      }
    });
  }
});

const allocationCurrency = computed({
  get: () => props.allocation.value.currency,
  set: (newValue) => {
    emit('changed', {
      ...props.allocation,
      value: {
        ...props.allocation.value,
        currency: newValue 
      }
    });
  }
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
