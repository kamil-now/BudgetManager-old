<template>
  <form @submit.prevent class="create-budget">
    <h2>Create budget</h2>
    <Accordion :activeIndex="0">
      <AccordionTab header="Funds">
        <FundsList 
          :autofocus="true" 
          :funds="funds"
          :fund-factory="() => createFund()"
        />
      </AccordionTab>
      <AccordionTab header="Accounts">
        <AccountsList 
          :autofocus="true" 
          :accounts="accounts"
          :account-factory="() => createAccount()"
        />
      </AccordionTab>
    </Accordion>
    <Button 
      class="submit-btn" 
      @click="submit()" 
      label="Submit" 
    />
  </form>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import currencies from '@/assets/currencies.json';
import AccountsList from '@/components/AccountsList.vue';
import FundsList from './FundsList.vue';
import { useAppStore } from '@/store/store';
import { Account } from '@/models/account';
import { Fund } from '@/models/fund';

const currencyCodeList = Object.keys(currencies);
const accounts = ref<Account[]>([createAccount(currencyCodeList[0])]);
const funds = ref<Fund[]>([createFund(true)]);

const { createBudget } = useAppStore();

function submit(): void {
  createBudget(
    accounts.value, 
    funds.value);
}

function createAccount(currency?: string): Account {
  return {
    name: 'New Account',
    balance: {
      amount: 0,
      currency: currency ?? getDefaultCurrency()
    }
  };
}
    
function  createFund(isDefault = false): Fund {
  return {
    name: 'New Fund',
    isDefault
  };
}

function getDefaultCurrency(): string {
  return accounts?.value 
    ? accounts.value[accounts.value.length - 1].balance.currency
    : currencyCodeList[0];
}
</script>

<style lang="scss">
.create-budget {
  padding: 0.5rem;
  width: 100%;
  max-height: 100%;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  align-items: flex-start;

  > h2 {
    margin: 0.5rem 0 2rem 0.5rem;
  }

  &_section {
    width: 100%;
    display: flex;
    flex-direction: row; 
    justify-content: space-between;
  }
  .submit-btn {
    align-self: flex-end;
  }
  .p-accordion {
    width: 100%;
    &-content {
      max-height: 50vh;
      overflow-y: auto;
    }
  }
}

</style>
