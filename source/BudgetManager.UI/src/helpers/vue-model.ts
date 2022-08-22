import { computed, WritableComputedRef } from "vue";

// https://www.vuemastery.com/blog/vue-3-data-down-events-up/
/** 
 * Instead of writing to the local state with v-model emit the event
*/

export function vueModel<T>(
  props: Readonly<{ modelValue: T }>,
  emit: (event: "update:modelValue", value: T) => void
): WritableComputedRef<T> {
  return computed({
    get: () => props.modelValue,
    set: (value) => emit("update:modelValue", value)
  });
}
