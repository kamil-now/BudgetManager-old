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
    <div class="create-budget_section">
      <AccountsList 
        :autofocus="true" 
        :accounts="accounts"
        :account-factory="() => createAccount()"
      />
    </div>
    <div class="create-budget_section">
      <FundsList 
        :autofocus="true" 
        :funds="funds"
        :fund-factory="() => createFund()"
      />
    </div>
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
    [], 
    []);
}

function createAccount(): Account {
  return {
    name: '',
    balance: {
      amount: 0,
      currency: defaultCurrency.value
    }
  };
}
    
function  createFund(): Fund {
  return {
    name: '',
  };
}
</script>

<style lang="scss">
.create-budget {
  width: 100%;
  display: flex;
  flex-direction: column;
  align-items: start;

  > h2 {
    margin: 3rem;
    align-self: center;
  }

  &_section {
    width: 100%;
    display: flex;
    flex-direction: row; 
    &:not(:first-of-type) {
      margin-top: 1.5rem;
    }
  }
  .submit-btn {
    margin: 3rem;
    align-self: center;
  }

  .accounts-list {
    width: calc(100% - 2rem);
  }
  .funds-list {
    width: calc(100% - 2rem);
  }
}

</style>
