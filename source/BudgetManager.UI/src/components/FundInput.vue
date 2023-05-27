<template>
  <div class="fund-input">
    <span class="p-float-label input-text">
      <InputText
        id="fundName" 
        v-model="fundName" 
        :autofocus="autofocus"
      />
      <label v-if="displayLabel" for="fundName">Fund</label>
    </span>
  </div>
</template>
<script setup lang="ts">
import { Fund } from '@/models/fund';
import { computed } from 'vue';
const props = defineProps<{
  fund: Fund,
  displayLabel: boolean,
  autofocus?: boolean
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
</script>

<style lang="scss">
</style>
