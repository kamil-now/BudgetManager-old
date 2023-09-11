import { Money } from '@/models/money';

export class DisplayFormat {
  static money(value: Money): string {
    return new Intl.NumberFormat('en-US').format(value.amount) + ' ' + value.currency;
  }
  static dateOnly(date: Date): string {
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();

    return `${day}/${month}/${year}`;
  }
}