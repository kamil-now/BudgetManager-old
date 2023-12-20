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
import { onBeforeMount, ref } from 'vue';
import { getSelectedTheme, saveSelectedTheme } from '../storage';

const isDark = ref(true); 

onBeforeMount(() => {
  // dark is default - see index.html
  if (getSelectedTheme() === 'light') {
    replaceInThemeUrl('dark', 'light');
    isDark.value = false;
  }
});

function switchTheme() {
  isDark.value = !isDark.value;
  const currentTheme = getSelectedTheme() === 'light' ? 'light' : 'dark';
  const selectedTheme = currentTheme === 'light' ? 'dark' : 'light';
  replaceInThemeUrl(currentTheme, selectedTheme);
  saveSelectedTheme(selectedTheme);
}

function replaceInThemeUrl(oldTheme: string, newTheme: string) {
  const linkElementId = 'theme-link';
  const linkElement = document.getElementById(linkElementId) as HTMLLinkElement;
  if (!linkElement) {
    return;
  }
  const currentThemeUrl = linkElement.href;
  linkElement.href = currentThemeUrl.replace(oldTheme, newTheme);
}
</script>
<style lang="scss">
.theme-toggle {
  $size: 2rem;

  background-color: var(--surface-card);
  color: var(--text-color-secondary);
  border: none;
  box-shadow: none;
  
  &:focus, &:focus-visible {
    border: none;
    box-shadow: none !important;
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
