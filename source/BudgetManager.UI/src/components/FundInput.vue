<template>
  <div class="fund-input">
    <span class="p-float-label input-text">
      <InputText
        class="p-inputtext-sm"
        id="fundName" 
        placeholder="Fund name"
        v-model="fundName"
      />
      <label v-if="displayLabel" for="fundName">Fund</label>
    </span>
    
    <Checkbox v-model="isDefault" inputId="isDefault" :binary="true"/>
    <label class="fund-input_is-default-label" for="isDefault">default</label>
  </div>
</template>
<script setup lang="ts">
import { Fund } from '@/models/fund';
import { computed } from 'vue';
const props = defineProps<{
  fund: Fund,
  displayLabel?: boolean,
}>();
const emit = defineEmits(['changed']);
const fundName = computed({
  get: () => props.fund.name,
  set: (newValue) => {
    emit('changed', {
      ...props.fund, 
      name: newValue
    });
  }
});
const isDefault = computed({
  get: () => props.fund.isDefault,
  set: (newValue) => {
    emit('changed', {
      ...props.fund, 
      isDefault: newValue
    });
  }
});
</script>

<style lang="scss">
.fund-input {
  display: flex;
  align-items: center;
  &_is-default-label{
    margin: 0.25rem;
    font-size: 0.75rem;
  }
}
</style>
