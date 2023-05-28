<template>
  <form @submit.prevent="submit" class="create-budget">
    <h2>Create budget</h2>
    <div class="create-budget_section">
      <span class="p-float-label input-text">
        <InputText
          id="defaultFundName" 
          v-model="defaultFundName" 
        />
        <label for="defaultFundName">Default fund</label>
      </span>
      <span class="p-float-label dropdown-currency-code">
        <Dropdown 
          id="defaultCurrency" 
          v-model="defaultCurrency" 
          :options="currencyCodeList" 
        />
        <label for="defaultCurrency">Default currency</label>
      </span>
    </div>
    
    <Accordion :activeIndex="0">
      <AccordionTab header="Accounts">
        <AccountsList 
          :autofocus="true" 
          :accounts="accounts"
          :account-factory="() => createAccount()"
        />
      </AccordionTab>
      <AccordionTab header="Funds">
        <FundsList 
          :autofocus="true" 
          :funds="funds"
          :fund-factory="() => createFund()"
        />
      </AccordionTab>
    </Accordion>
    
    <Button class="submit-btn" type="submit" label="Submit" />
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
const defaultCurrency = ref<string>(currencyCodeList[0]);
const defaultFundName = ref<string>('Unallocated funds');
const accounts = ref<Account[]>([createAccount()]);
const funds = ref<Fund[]>([createFund()]);

const { createBudget } = useAppStore();

function submit(): void {
  createBudget(
    defaultFundName.value, 
    defaultCurrency.value, 
    accounts.value, 
    funds.value);
}

function createAccount(): Account {
  return {
    name: 'New Account',
    balance: {
      amount: 0,
      currency: defaultCurrency.value
    }
  };
}
    
function  createFund(): Fund {
  return {
    name: 'New Fund',
  };
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
