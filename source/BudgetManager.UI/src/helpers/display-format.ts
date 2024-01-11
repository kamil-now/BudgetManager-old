import { Money } from '@/models/money';

export class DisplayFormat {
  static money(value: Money, addSign: boolean = false): string {
    return (addSign && value.amount > 0 ? '+' : '') + new Intl.NumberFormat('en-US', { minimumFractionDigits: 2 }).format(value.amount) + ' ' + value.currency;
  }

  static rounded(value: number): string {
    return new Intl.NumberFormat('en-US', { minimumFractionDigits: 2 }).format(value);
  }
}