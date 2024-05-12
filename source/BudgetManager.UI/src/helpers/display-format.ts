import { Money } from '@/models/money';

export class DisplayFormat {
  private static _userLocale = navigator.languages && navigator.languages.length ? navigator.languages[0] : 'en';
  static money(value: Money, addSign: boolean = false): string {
    return (addSign && value.amount > 0 ? '+' : '') + new Intl.NumberFormat(DisplayFormat._userLocale, { minimumFractionDigits: 2 }).format(value.amount) + ' ' + value.currency;
  }

  static rounded(value: number): string {
    return new Intl.NumberFormat(DisplayFormat._userLocale, { minimumFractionDigits: 2 }).format(value);
  }
}