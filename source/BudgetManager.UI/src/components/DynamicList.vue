
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
        <Button 
          v-if="index !== 0"
          icon="pi pi-chevron-up" 
          text 
          rounded 
          aria-label="Up" 
          @click="moveUp(index)" 
        />
        <Button 
          v-if="index !== items.length - 1"
          icon="pi pi-chevron-down" 
          text 
          rounded 
          aria-label="Down" 
          @click="moveDown(index)" 
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
  canBeRemovedWithoutConfirmation?: (item: T) => boolean,
  createNew: () => T,
  max: number
}
// eslint-disable-next-line @typescript-eslint/no-explicit-any
const props = defineProps<Props<any & {id?: string, name: string}>>();

const emit = defineEmits(['update', 'removed']);

const confirm = useConfirm();

const items = computed({
  get: () => props.items,
  set: (newValue) => emit('update', newValue)
});


function removeAt(event: MouseEvent, index: number) {
  const accept = () =>  emit('removed', items.value.splice(index, 1)[0]);
  const item = items.value[index];
  if (!item.id && (item.name.length === 0 
    || props.canBeRemovedWithoutConfirmation && props.canBeRemovedWithoutConfirmation(item))) {
    accept();
    return;
  }
  confirm.require({
    target: event.target as HTMLElement,
    message: `Remove '${item.name}'?`,
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    rejectClass: 'p-button-secondary',
    accept
  });
}

function moveUp(index: number) {
  moveItem(index, index - 1);
}

function moveDown(index: number) {
  moveItem(index, index + 1);
}

function addNew() {
  items.value.push(props.createNew());
}

function moveItem(fromIndex: number, toIndex: number) {
  const element = items.value[fromIndex];
  items.value.splice(fromIndex, 1);
  items.value.splice(toIndex, 0, element);
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
    align-items: start;
    gap: 0.25rem;

    &:not(:first-of-type) {
      margin-top: 0.5rem;
    }
    .p-divider.p-divider-horizontal {
      margin: 0;
    }
  }
}
</style>
