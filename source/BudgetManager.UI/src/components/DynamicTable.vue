<template>
  <div class="data-table"
    @mouseenter="hover = true"
    @mouseleave="hover = false"
  >
    <DataTable 
      v-model:editingRows="editingRows"
      :value="items" 
      :editMode="'row'" 
      dataKey="id"
      columnResizeMode="expand"
      scrollable
      scrollHeight="flex"
      @row-edit-init="onRowEditInit"
      @row-edit-cancel="onRowEditCancel"
      @row-edit-save="onRowEditSave"
      @rowReorder="onRowReorder" 
    >
      <Column 
        v-if="allowReorder"
        rowReorder 
        header-class="data-table_header-column"
      />
      <Column class="data-table_content-column" :header="header">
        <template #body="{ data, index }">
          <div class="data-table_content-column_body">
            <slot name="body" :item="data" :index="index"></slot>
          </div>
        </template>
        <template #editor="{ data, index }">
          <div class="data-table_content-column_editor">
            <slot name="editor" :item="data" :index="index"></slot>
          </div>
        </template>
      </Column>
      <Column 
        :rowEditor="true"
        bodyStyle="text-align:center"
        class="data-table_action-column"
      >
        <template #header>
          <template v-if="allowAdd && hover && editingRows.length == 0">
            <Button
              icon="pi pi-plus" 
              text 
              rounded 
              aria-label="Add" 
              @click="addNew()" 
            />
          </template>
        </template>
      </Column>
    </DataTable>
  </div>
</template>
<script setup lang="ts">
import { vueModel } from '@/helpers/vue-model';
import { Ref, ref } from 'vue';
type RowEditEvent =  {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  data: any,
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  newData: any,
  index: number
};

type Props<T> = {
  header: string,
  modelValue: T[],
  allowEdit?: boolean,
  allowReorder?: boolean,
  allowAdd?: boolean,
  createNew: () => T,
  save: () => void,
  update: () => void,
  onReorder: () => void,
}// eslint-disable-next-line @typescript-eslint/no-explicit-any
const props = defineProps<Props<any & {id?: string, name: string}>>();

const emit = defineEmits(['update:modelValue']);
const items = vueModel(props, emit);
const hover = ref<boolean>(false);
// eslint-disable-next-line @typescript-eslint/no-explicit-any
const editingRows: Ref<any[]> = ref([]);

function onRowEditInit(event: RowEditEvent) {
  editingRows.value = [event.data];
}

function onRowEditSave(event: RowEditEvent) {
  const { newData, index } = event;
  items.value.splice(index, 1);
  if (newData.id) {
    props.update();
  } else {
    props.save();
  }
  editingRows.value = [];
}

function onRowEditCancel(event: RowEditEvent) {
  if (!event.newData.id) {
    items.value = items.value.filter(x => !!x.id);
    editingRows.value = [];
  }
}

function addNew() {
  const item = props.createNew();
  items.value.unshift(item);
  editingRows.value = [item];
}

function onRowReorder(event: {dragIndex: number, dropIndex: number}) {
  const { dragIndex, dropIndex } = event;
  const element = items.value[dragIndex];
  items.value.splice(dragIndex, 1);
  items.value.splice(dropIndex, 0, element);
  props.onReorder();
}
</script>

<style lang="scss">
$padding: 0.25rem;
$action-column-width: 6rem;
$header-column-width: 2rem;
.data-table {
  width: 100%;
  height: 100%;
  &_header-column {
    width: $header-column-width;
    max-width: $header-column-width;
  }
  .p-datatable-wrapper {
    overflow-x: hidden;
  }
  &_content-column {
    &_editor {
      display: flex;
    }
    &_body {
      display: flex;
    }
  }
  &_action-column {
    padding: 0 !important;
    .p-column-header-content {
      justify-content: center;
    }
    width: $action-column-width;
    min-width: $action-column-width;
  }
}
</style>
