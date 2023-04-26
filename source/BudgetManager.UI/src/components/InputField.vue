<template>
  <input
    :type="type" 
    :value="modelValue"
    @input="onInput"
    @keydown.enter="onEnter"
  />
</template>

<script setup lang="ts">
import { defineProps } from 'vue';

defineProps(['type', 'modelValue']);
const emit = defineEmits(['update:modelValue']);

function onInput(event: Event) {
  const input = event.target as HTMLInputElement;
  emit('update:modelValue', input.value);
}

function onEnter(event: Event) {
  const input = event.target as HTMLInputElement;
  input.blur();
}

</script>

<style lang="scss">
// input {
//   @extend .text;
//   border-radius: 5px;
//   order: none;
//   padding: 0;
//   margin: 0;
//   outline: none;
//   border: none;
//   &:focus {
//     font-style: italic;
//   }
//   &[readonly]:focus {
//     font-style: normal;
//   }
// }
input {
  @extend .text;
  border: 0; 
  border-bottom: 2px solid var(--app-text-color);
  // border-radius: 8px;
  padding: 12px;
  font-size: 16px;
  outline: none;
  transition: border-color 0.3s ease-in-out;
  
  &:focus {
    border-color: var(--app-accent-color);
  }
}
</style>
