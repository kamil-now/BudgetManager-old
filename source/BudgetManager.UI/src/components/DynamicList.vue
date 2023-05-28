
<template>
  <div class="form-list">
    <div 
      class="form-list_item"
      v-for="(item, index) in items" :key="item.id" 
    >
    <slot :item="item" :index="index"></slot> 
      <Divider >
        <ConfirmPopup></ConfirmPopup>
        <Button 
          v-if="items.length !== 1"
          icon="pi pi-times" 
          severity="danger" 
          text 
          rounded 
          aria-label="Remove" 
          @click="removeAt($event, index)" 
        />
        <Button 
          v-if="index === items.length - 1"
          icon="pi pi-plus" 
          text 
          rounded 
          aria-label="Add" 
          @click="addNew()" 
        />
      </Divider>
    </div>
  </div>
</template>
<script setup lang="ts">
import { computed } from 'vue';
import { useConfirm } from 'primevue/useconfirm';

type Props<T> = {
  items: T[],
  canBeDeleted?: (item: T) => boolean,
  createNew: () => T,
  max: number
}
// eslint-disable-next-line @typescript-eslint/no-explicit-any
const props = defineProps<Props<any & {id?: string, name: string}>>();

const emit = defineEmits(['update']);

const confirm = useConfirm();

const items = computed({
  get: () => props.items,
  set: (newValue) => emit('update', newValue)
});


function removeAt(event: MouseEvent, index: number) {
  const accept = () => items.value.splice(index, 1);
  const item = items.value[index];
  if (!item.id && (item.name.length === 0 || props.canBeDeleted && props.canBeDeleted(item))) {
    accept();
    return;
  }
  confirm.require({
    target: event.target as HTMLElement,
    message: `Delete '${item.name}'?`,
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    rejectClass: 'p-button-secondary',
    accept
  });
}


function addNew() {
  items.value.push(props.createNew());
}

</script>

<style lang="scss">
.form-list {
  display: flex;
  flex-direction: column;
  align-items: flex-end;

  &_item {
    width: 100%;
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 0.25rem;

    &:not(:first-of-type) {
      margin-top: 0.5rem;
    }
  }
}
</style>
