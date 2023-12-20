export function getSelectedTheme() {
  return localStorage.getItem('theme');
}

export function saveSelectedTheme(value: 'dark' | 'light') {
  localStorage.setItem('theme', value);
}
