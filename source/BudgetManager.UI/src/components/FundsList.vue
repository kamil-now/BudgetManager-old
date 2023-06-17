<template>
  <div class="funds-list">
    <DynamicList 
      v-slot="slotProps"
      :items="funds"
      :createNew="fundFactory"
      :max="100"
      :canBeRemovedWithoutConfirmation="fund => !fund.isDefault"
      @removed="onRemoved($event)"
    >
      <FundInput 
        :fund="slotProps.item" 
        @changed="onFundChanged($event, slotProps.index)"
      />
    </DynamicList>
  </div>
</template>
<script setup lang="ts">
import DynamicList from '@/components/DynamicList.vue';
import { Fund } from '@/models/fund';
import { computed } from 'vue';
import FundInput from './FundInput.vue';

const props = defineProps<{
  funds: Fund[],
  fundFactory:() => Fund
}>();

const emit = defineEmits(['update']);
const funds = computed({
  get: () => props.funds,
  set: (newValue) => emit('update', newValue)
});

function onFundChanged(fund: Fund, index: number) {
  if (fund.isDefault) {
    funds.value.forEach(fund => fund.isDefault = false);
  } else {
    funds.value[0].isDefault = true;
  }
  funds.value[index] = fund;
}
function onRemoved(fund: Fund) {
  if (fund.isDefault) {
    funds.value[0].isDefault = true;
  }
}
</script>

<style lang="scss">
</style>
