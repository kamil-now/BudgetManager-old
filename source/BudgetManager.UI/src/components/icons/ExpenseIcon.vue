<template>
  <i class="pi pi-minus expense-icon">
    <slot></slot>
  </i>
</template>
<style lang="scss">
.expense-icon {
  @extend .budget-icon;
  color: crimson;
}
</style>