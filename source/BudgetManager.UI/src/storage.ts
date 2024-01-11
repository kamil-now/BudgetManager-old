export function getSelectedTheme() {
  return localStorage.getItem('theme');
}

export function saveSelectedTheme(value: 'dark' | 'light') {
  localStorage.setItem('theme', value);
}

export function getIncomeDistributionPreference() {
  return localStorage.getItem('use-income-distribution') === 'true';
}

export function saveIncomeDistributionPreference(value: boolean) {
  return localStorage.setItem('use-income-distribution', value.toString());
}