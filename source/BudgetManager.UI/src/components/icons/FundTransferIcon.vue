<template>
  <i class="pi pi-arrow-right-arrow-left fund-transfer-icon">
    <slot></slot>
  </i>
</template>
<style lang="scss">
.fund-transfer-icon {
  @extend .budget-icon;
  @extend .transfer-icon;
}
</style>