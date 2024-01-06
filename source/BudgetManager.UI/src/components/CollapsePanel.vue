<template>
  <div
    class="collapse-panel"
    :class="{ collapsed: isCollapsed }"
  >
    <div class="collapse-panel_header" @click="toggle()">
      <i>></i>
      <slot name="header"></slot>
    </div>
    <div class="collapse-panel_content">
      <slot name="content"></slot>
    </div>
  </div>
</template>
<script setup lang="ts">
import { ref } from 'vue';

const isCollapsed = ref(true);

function toggle() {
  isCollapsed.value = !isCollapsed.value;
}
</script>
<style lang="scss">
.collapse-panel {
  min-height: 3rem;
  height: auto;
  overflow: hidden;
  display: flex;
  width: 100%;
  align-items: center;
  justify-content: start;
  flex-direction: column;
  * {
    transition: all 2s;
  }
  &.collapsed {
    height: 0;
    .collapse-panel_header {
      i {
        transform: rotate(90deg);
      }
    }
  }
  &_header {
    i {
      // transition: all 2s;
      padding-right: 1rem;
    }
    height: 2rem;
    padding: 0.5rem 0;
    cursor: pointer;
  }
  &_content {
    padding: 1rem;
  }
}
</style>
