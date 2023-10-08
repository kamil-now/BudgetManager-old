import { Money } from '@/models/money';

export class DisplayFormat {
  static money(value: Money): string {
    return new Intl.NumberFormat('en-US').format(value.amount) + ' ' + value.currency;
  }
}