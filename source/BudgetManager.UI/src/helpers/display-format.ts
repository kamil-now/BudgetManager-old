import { Money } from '@/models/money';

export class DisplayFormat {
  static money(value: Money): string {
    return new Intl.NumberFormat('en-US', { minimumFractionDigits: 2 }).format(value.amount) + ' ' + value.currency;
  }
}