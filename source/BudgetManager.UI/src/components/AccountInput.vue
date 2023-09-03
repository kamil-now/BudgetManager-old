<template>
  <div class="account-input">
    <InputText
      ref="input"
      class="p-inputtext-sm"
      id="accountName" 
      placeholder="Account name"
      v-model="accountName" 
    />
    <InputNumber 
      class="p-inputtext-sm"
      :disabled="!!account.id"
      id="accountBalance"
      v-model="accountBalance" 
      mode="currency"
      currencyDisplay="code"
      :allowEmpty="false"
      :currency="accountCurrency" 
      :min="0"
      :maxFractionDigits="2"
      :max="1000000000"
    />
    <Dropdown 
      :disabled="!!account.id"
      class="p-inputtext-sm"
      id="accountCurrency" 
      v-model="accountCurrency" 
      :options="currencyCodeList" 
    />
    <Dropdown
      v-if="!account.id && accountBalance > 0"
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
import { Account } from '@/models/account';
import { computed, nextTick, onMounted, ref, watch } from 'vue';
import currencies from '@/assets/currencies.json';
import { Fund } from '@/models/fund';
import { useAppStore } from '@/store/store';

const { funds }  = useAppStore();
const props = defineProps<{ account : Account }>();
const emit = defineEmits(['changed']);
const input = ref();

const currencyCodeList = Object.keys(currencies);
const selectedFund = ref<Fund | undefined>(
  props.account.initialFundId 
    ? funds.find(x => x.id === props.account.initialFundId )
    : funds[0]
);

onMounted(() => focusInput());

function focusInput() {
  nextTick(() => {
    input.value.$el.focus();
  });
}

watch(selectedFund, async (fund) => {
  emit('changed', {
    ...props.account, 
    initialFundId: fund?.id
  });
});
const accountName = computed({
  get: () => props.account.name,
  set: (newValue) => {
    emit('changed', {
      ...props.account, 
      name: newValue
    });
  }
});
const accountBalance = computed({
  get: () => props.account.balance.amount,
  set: (newValue) => {
    emit('changed', {
      ...props.account,
      balance: {
        ...props.account.balance,
        amount: newValue
      }
    });
  }
});
const accountCurrency = computed({
  get: () => props.account.balance.currency,
  set: (newValue) => {
    emit('changed', {
      ...props.account,
      balance: {
        ...props.account.balance,
        currency: newValue 
      }
    });
  }
});
</script>

<style lang="scss">
.account-input {
  display: flex;
  max-width: 100%;
  flex-wrap: wrap;
  gap: 1rem;
}
</style>
