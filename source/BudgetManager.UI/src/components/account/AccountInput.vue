<template>
  <div class="account-input">
    <InputText
      ref="input"
      class="p-inputtext-sm"
      id="accountName" 
      placeholder="Account name"
      v-model="accountName" 
    />
    <div class="account-input_value" v-for="(amount, currency) in initialBalance" :key="currency">
      <MoneyInput :money="({amount, currency} as Money)" @changed="onMoneyChanged($event)"/>
      <Button 
          v-if="accountCurrencies.length > 0"
          icon="pi pi-times" 
          severity="danger" 
          text 
          rounded 
          aria-label="Remove" 
          @click="remove(currency as string)" 
        />
    </div>
    
    <div v-if="availableCurrencies.length > 0">
      <Dropdown
        class="p-inputtext-sm"
        v-model="selectedCurrency" 
        :options="availableCurrencies" 
      />
      <Button
        icon="pi pi-plus" 
        text 
        rounded 
        aria-label="Add" 
        @click="add()" 
      />
    </div>
  </div>
</template>
<script setup lang="ts">
import currencies from '@/assets/currencies.json';
import { Account } from '@/models/account';
import { computed, nextTick, onMounted, ref } from 'vue';
import MoneyInput from '@/components/money-operations/MoneyInput.vue';
import { Money } from '@/models/money';

const props = defineProps<{ account : Account }>();
const emit = defineEmits(['changed']);
const input = ref();

const accountCurrencies = computed(() => Object.keys(props.account.initialBalance));
const availableCurrencies = computed(
  () => Object.keys(currencies).filter(x => !accountCurrencies.value.includes(x))
);
const selectedCurrency = ref<string>(availableCurrencies.value.length > 0 ? availableCurrencies.value[0] : '');

onMounted(() => focusInput());

function focusInput() {
  nextTick(() => {
    input.value.$el.focus();
  });
}

const accountName = computed({
  get: () => props.account.name,
  set: (newValue) => {
    emit('changed', {
      ...props.account, 
      name: newValue
    });
  }
});
const initialBalance = computed(() => {
  return props.account.initialBalance;}
);
function add() {
  emit('changed', {
    ...props.account, 
    initialBalance: {
      ...props.account.initialBalance,
      [selectedCurrency.value]: 0
    }
  });
  selectedCurrency.value = availableCurrencies.value.length > 0 ? availableCurrencies.value[0] : '';
}
function remove(currency: string) {
  const newInitialBalance = { ...props.account.initialBalance };
  delete newInitialBalance[currency];
  emit('changed', {
    ...props.account, 
    initialBalance: newInitialBalance
  });
}
function onMoneyChanged(money: Money) {
  emit('changed', {
    ...props.account, 
    initialBalance: {
      ...props.account.initialBalance,
      [money.currency]: money.amount
    }
  });
} 
</script>

<style lang="scss">
.account-input {
  display: flex;
  max-width: 100%;
  flex-wrap: wrap;
  gap: 1rem;
  &_value {
    display: flex;
  }
}
</style>
