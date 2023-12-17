<template>
  <i class="pi pi-arrow-right-arrow-left currency-exchange-icon">
    <slot></slot>
  </i>
</template>
<style lang="scss">
.currency-exchange-icon {
  @extend .budget-icon;
  @extend .transfer-icon;
}
</style>