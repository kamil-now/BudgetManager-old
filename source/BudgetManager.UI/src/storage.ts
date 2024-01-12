export function getSelectedTheme() {
  return localStorage.getItem('theme');
}

export function saveSelectedTheme(value: 'dark' | 'light') {
  localStorage.setItem('theme', value);
}

export function getIncomeAllocationPreference() {
  return localStorage.getItem('use-income-allocation') === 'true';
}

export function saveIncomeAllocationPreference(value: boolean) {
  return localStorage.setItem('use-income-allocation', value.toString());
}