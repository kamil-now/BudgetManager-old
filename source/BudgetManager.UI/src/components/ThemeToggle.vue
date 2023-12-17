<template>
  <Button
    class="theme-toggle"
    @click="switchTheme()"
  >
    <i
      v-if="isDark"
      class="pi pi-sun"
    />
    <i
      v-else
      class="pi pi-moon"
    />
  </Button>
</template>

<script setup lang="ts">
import { ref } from 'vue';

const isDark = ref(true); // set in index.html

function switchTheme() {
  const linkElementId = 'theme-link';
  const linkElement = document.getElementById(linkElementId) as HTMLLinkElement;
  if (!linkElement) {
    return;
  }
  const currentThemeUrl = linkElement.href;
  let newThemeUrl = null;
  if (currentThemeUrl.includes('light')) {
    newThemeUrl = currentThemeUrl.replace('light', 'dark');
  } else {
    newThemeUrl = currentThemeUrl.replace('dark', 'light');
  }
  linkElement.href = newThemeUrl;
  isDark.value = !isDark.value;
}
</script>
<style lang="scss">
.theme-toggle {
  $size: 1.5rem;

  background-color: var(--surface-card);
  color: var(--text-color-secondary);
  border-color: var(--text-color-secondary);
  box-shadow: none;
  &:focus {
    box-shadow: none;
    border-color: var(--text-color-secondary);
  }

  min-width: $size;
  min-height: $size;
  width: $size;
  height: $size;
  font-size: $size;
  line-height: $size;

  display: flex;
  align-items: center;
  justify-content: center;
}
</style>
