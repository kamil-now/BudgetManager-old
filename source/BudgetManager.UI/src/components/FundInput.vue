<template>
  <div class="fund-input">
    <InputText
      class="p-inputtext-sm"
      id="fundName" 
      placeholder="Fund name"
      v-model="fundName"
    />
  </div>
</template>
<script setup lang="ts">
import { Fund } from '@/models/fund';
import { computed } from 'vue';
const props = defineProps<{ fund: Fund }>();
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
.fund-input {
  display: flex;
  flex-direction: row;
  gap: 0.25rem;
}
</style>
