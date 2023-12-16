<template>
  <i class="pi pi-arrow-right-arrow-left account-transfer-icon">
    <slot></slot>
  </i>
</template>
<style lang="scss">
.account-transfer-icon {
  @extend .operation-icon;
  color: var(--highlight-text-color);
  font-weight: bold;
}
</style>