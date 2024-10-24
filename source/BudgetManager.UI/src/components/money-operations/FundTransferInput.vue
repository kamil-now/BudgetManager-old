<template>
  <div class="fundTransfer-input">
    <Calendar v-model="fundTransferDate" />
    <InputText
      class="p-inputtext-sm"
      placeholder="Fund transfer title"
      v-model="fundTransferTitle"
    />
    <Dropdown
      class="p-inputtext-sm"
      v-model="selectedSourceFund"
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
      v-model="selectedTargetFund"
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
      id="fundTransferCurrency"
      v-model="fundTransferCurrency"
      :options="currencyCodeList"
    />
    <InputNumber
      class="p-inputtext-sm"
      id="fundTransferValue"
      v-model="fundTransferValue"
      mode="currency"
      currencyDisplay="code"
      :highlightOnFocus="true"
      :allowEmpty="false"
      :currency="fundTransferCurrency"
      :min="0"
      :maxFractionDigits="2"
      :max="1000000000"
    />
  </div>
</template>
<script setup lang="ts">
import currencies from '@/assets/currencies.json';
import { DateUtils } from '@/helpers/date-utils';
import { Fund } from '@/models/fund';
import { FundTransfer } from '@/models/fund-transfer';
import { useAppStore } from '@/store/store';
import { computed, ref, watch } from 'vue';
const props = defineProps<{ fundTransfer: FundTransfer }>();
const emit = defineEmits(['changed']);
const { funds } = useAppStore();
const currencyCodeList = Object.keys(currencies);

const selectedSourceFund = ref<Fund | undefined>(
  props.fundTransfer.fundId
    ? funds.find((x) => x.id === props.fundTransfer.fundId)
    : undefined
);

watch(selectedSourceFund, async (fund) => {
  emit('changed', {
    ...props.fundTransfer,
    fundId: fund?.id,
    fundName: fund?.name,
  });
});

const selectedTargetFund = ref<Fund | undefined>(
  props.fundTransfer.targetFundId
    ? funds.find((x) => x.id === props.fundTransfer.targetFundId)
    : undefined
);

watch(selectedTargetFund, async (fund) => {
  emit('changed', {
    ...props.fundTransfer,
    targetFundId: fund?.id,
    targetFundName: fund?.name,
  });
});

const fundTransferDate = computed({
  get: () => props.fundTransfer.date,
  set: (newValue) => {
    emit('changed', {
      ...props.fundTransfer,
      date: DateUtils.createDateOnlyString(new Date(newValue)),
    });
  },
});
const fundTransferTitle = computed({
  get: () => props.fundTransfer.title,
  set: (newValue) => {
    emit('changed', {
      ...props.fundTransfer,
      title: newValue,
    });
  },
});
const fundTransferValue = computed({
  get: () => props.fundTransfer.value.amount,
  set: (newValue) => {
    emit('changed', {
      ...props.fundTransfer,
      value: {
        ...props.fundTransfer.value,
        amount: newValue,
      },
    });
  },
});

const fundTransferCurrency = computed({
  get: () => props.fundTransfer.value.currency,
  set: (newValue) => {
    emit('changed', {
      ...props.fundTransfer,
      value: {
        ...props.fundTransfer.value,
        currency: newValue,
      },
    });
  },
});
</script>

<style lang="scss">
.fundTransfer-input {
  display: flex;
  max-width: 100%;
  flex-wrap: wrap;
  gap: 1rem;
}
</style>
