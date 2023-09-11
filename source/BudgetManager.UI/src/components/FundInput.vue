<template>
  <div class="fund-input">
    <InputText
      ref="input"
      class="p-inputtext-sm"
      id="fundName" 
      placeholder="Fund name"
      v-model="fundName"
    />
  </div>
</template>
<script setup lang="ts">
import { Fund } from '@/models/fund';
import InputText from 'primevue/inputtext';
import { computed, nextTick, onMounted, ref } from 'vue';
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
const input = ref();
onMounted(() => focusInput());

function focusInput() {
  nextTick(() => {
    input.value.$el.focus();
  });
}
</script>

<style lang="scss">
.fund-input {
  display: flex;
  flex-direction: row;
  gap: 0.25rem;
}
</style>
