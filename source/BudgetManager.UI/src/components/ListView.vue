<template>
  <ConfirmPopup></ConfirmPopup>
  <div class="list-view">
    <DataTable :value="items" :editMode="'row'" dataKey="id" columnResizeMode="expand" scrollable scrollHeight="flex"
      @rowReorder="onRowReorder">
      <template #header>
        <div class="list-view_header" v-if="createNew">
          <span>{{ header }}</span>
          <Button v-if="allowAdd && !editing" icon="pi pi-plus" text rounded aria-label="Add" @click="add()" />
        </div>
      </template>
      <Column v-if="!!onReorder" rowReorder />
      <Column class="list-view_content-column">
        <template #body="{ data, index }">
          <div class="list-view_body" @mouseenter="hover = data" @mouseleave="hover = null">
            <div class="list-view_body-content" :class="{ blur: hover === data }" v-if="editing !== data">
              <slot name="content" :data="data"></slot>
            </div>
            <div class="list-view_body-editor" v-else>
              <slot name="editor" :data="data"></slot>
            </div>
            <div style="position: absolute; right: 0; display: flex;">
              <Button v-if="hover === data && editing !== data && !!copy" icon="pi pi-copy" text rounded aria-label="Copy"
                @click="createCopy(data)" />
              <Button v-if="hover === data && editing !== data" icon="pi pi-pencil" text rounded aria-label="Add"
                @click="editing = data" />
              <Button v-if="data.id && hover === data && editing !== data" icon="pi pi-times" severity="danger" text
                rounded aria-label="Remove" @click="removeAt($event, index)" />
              <Button v-if="editing === data" icon="pi pi-check" text rounded aria-label="Save"
                @click="save(data, index)" />
              <Button v-if="editing === data" icon="pi pi-times" text rounded aria-label="Discard"
                @click="discard(data)" />
            </div>
          </div>
        </template>
      </Column>
    </DataTable>
  </div>
</template>
<script setup lang="ts">
import { vueModel } from '@/helpers/vue-model';
import { ref } from 'vue';
import { useConfirm } from 'primevue/useconfirm';

const confirm = useConfirm();

type Props<T> = {
  header: string,
  modelValue: T[],
  allowAdd?: boolean,
  copy?: (item: T) => T,
  createNew?: () => T,
  save?: (item: T) => void,
  update?: (item: T) => void,
  onReorder?: (oldIndex: number, newIndex: number) => void,
  remove?: (itemId: string) => void
}// eslint-disable-next-line @typescript-eslint/no-explicit-any
const props = defineProps<Props<any & { id?: string, name: string }>>();

const emit = defineEmits(['update:modelValue']);
const items = vueModel(props, emit);
const hover = ref<object | null>(null);
const editing = ref<object | null>(null);

// eslint-disable-next-line @typescript-eslint/no-explicit-any
function createCopy(item: any) {
  if (!props.copy) {
    throw new Error('Copy delegate is undefined.');
  }
  const newItem = props.copy(item);
  items.value.unshift(newItem);
  editing.value = newItem;
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
function save(item: any, index: number) {
  if (!props.update || !props.save) {
    throw new Error();
  }
  if (item.id) {
    props.update(item);
  } else {
    props.save(item);
    items.value.splice(index, 1);
  }
  editing.value = null;
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
function discard(item: any) {
  if (!item.id) {
    items.value = items.value.filter(x => !!x.id);
  }
  editing.value = null;
}

function removeAt(event: MouseEvent, index: number) {
  const item = items.value[index];
  confirm.require({
    target: event.target as HTMLElement,
    message: 'Are you sure you want to remove it? This action is permanent.',
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    rejectClass: 'p-button-secondary',
    accept: () => {
      if (!props.remove) {
        throw new Error();
      }
      props.remove(item.id);
    }
  });
}

function add() {
  if (!props.createNew) {
    throw new Error();
  }
  const item = props.createNew();
  items.value.unshift(item);
  editing.value = item;
}

function onRowReorder(event: { dragIndex: number, dropIndex: number }) {
  if (!props.onReorder) {
    throw new Error('Copy delegate is undefined.');
  }
  const { dragIndex, dropIndex } = event;
  props.onReorder(dragIndex, dropIndex);
}
</script>

<style lang="scss">
$padding: 0.25rem;
$header-column-width: 2rem;

.list-view {
  .blur {
    opacity: 0.5;
    // filter: blur(1px);
  }

  width: 100%;
  height: 100%;

  &_header-column {
    width: $header-column-width;
    max-width: $header-column-width;
  }

  &_header {
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: space-between;
  }

  &_body {
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: space-between;

    &-content {
      width: 100%;
    }

    &-editor {
      padding-right: 6rem; // for 2 3rem width floating action buttons
    }
  }

  &_content-column {
    padding: 0.5rem !important;

    &_editor {
      display: flex;
    }

    &_body {
      display: flex;
    }
  }

  .p-datatable-wrapper {
    overflow-x: hidden;
  }

  .p-datatable-header {
    display: none;
  }

  .p-button {
    min-width: 3rem;
  }

  th {
    display: none;
  }
}
</style>
