<template>
  <i class="pi pi-plus income-icon">
    <slot></slot>
  </i>
</template>
<style lang="scss">
.income-icon {
  @extend .budget-icon;
  color: limegreen;
}
</style>