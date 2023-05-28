<template>
  <div class="funds-list">
    <div 
      class="funds-list_item" 
      v-for="(fund, index) in funds" :key="fund.id"
    >
      <FundInput 
        :fund="fund" 
        :display-label="index === 0"
        :autofocus="autofocus && index === 0"
        @changed="onFundChanged($event, index)"
      />
      <div class="funds-list_item-actions">
        <ConfirmPopup></ConfirmPopup>
        <Button 
          v-if="funds.length !== 1"
          icon="pi pi-times" 
          severity="danger" 
          text 
          rounded 
          aria-label="Remove" 
          @click="removeAt($event, index)" 
        />
        <Button 
          v-if="index === funds.length - 1"
          icon="pi pi-plus" 
          text 
          rounded 
          aria-label="Add" 
          @click="addNew()" 
        />
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { Fund } from '@/models/fund';
import { useConfirm } from 'primevue/useconfirm';
import { computed } from 'vue';
import FundInput from './FundInput.vue';

const props = defineProps<{
  funds: Fund[],
  autofocus?: boolean,
  fundFactory:() => Fund
}>();

const emit = defineEmits(['update']);
const funds = computed({
  get: () => props.funds,
  set: (newValue) => emit('update', newValue)
});


const confirm = useConfirm();

function onFundChanged(fund: Fund, index: number) {
  funds.value[index] = fund;
}

function removeAt(event: MouseEvent, index: number) {
  const accept = () => funds.value.splice(index, 1);
  const fund = funds.value[index];
  if (!!fund.id || fund.name.length === 0) {
    accept();
    return;
  }
  confirm.require({
    target: event.target as HTMLElement,
    message: 'Delete this fund?',
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    rejectClass: 'p-button-secondary',
    accept
  });
}

function addNew() {
  if (funds.value.length >= 100) {
    alert('You cannot create more than 100 funds');
    return;
  }
  funds.value.push(props.fundFactory());
}

</script>

<style lang="scss">
.funds-list {
  display: flex;
  flex-direction: column;
  align-items: flex-end;

  &_item {
    width: 100%;
    display: flex;
    flex-direction: row;
    align-items: center;
    gap: 0.25rem;

    &:not(:first-of-type) {
      margin-top: 0.5rem;
    }

    &-actions {
      display: flex;
      flex-direction: column;
      .p-button {
        padding: 0 !important;
        height: 1.5rem !important;
        width: 1.5rem !important;
      }
    }
  }
}
</style>
