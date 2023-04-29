<template>
  <label>
    <span>{{ label }}</span>  
    <input
      v-bind="$attrs"
      ref="input"
      :type="type" 
      :value="modelValue"
      @input="onInput"
      @keydown.enter="onEnter"
    />
  </label>
</template>
<script setup lang="ts">
withDefaults(
  defineProps<{
    type?: 'text' | 'number',
    label?: string,
    modelValue: string | number 
  }>(), 
  {
    type: 'text',
    label: '',
    modelValue: '' 
  });
const emit = defineEmits(['update:modelValue']);

function onInput(event: Event) {
  const input = event.target as HTMLInputElement;
  if (input.max && Number(input.value) > Number(input.max)) {
    input.value = input.max;
  } 
  if (input.min && Number(input.value) < Number(input.min)) {
    input.value = input.min;
  }
  emit('update:modelValue', input.value);
}

function onEnter(event: Event) {
  const input = event.target as HTMLInputElement;
  input.blur();
}
</script>

<style lang="scss">
label {
  display: flex;
  flex-direction: column;
  align-items: start;
  span {
    color: var(--app-accent-text-color);
  }
}
input {
  @extend .text;
  border: 0; 
  border-bottom: 2px solid var(--app-text-color);
  padding: 8px 0;
  font-size: 16px;
  outline: none;
  transition: border-color 0.3s ease-in-out;
  
  &:focus {
    border-color: var(--app-accent-color);
    font-weight: 400;
    font-style: italic;
  }
}
input[type=number]::-webkit-inner-spin-button, 
input[type=number]::-webkit-outer-spin-button { 
    -webkit-appearance: none;
    -moz-appearance: none;
    appearance: none;
    margin: 0; 
}
</style>
