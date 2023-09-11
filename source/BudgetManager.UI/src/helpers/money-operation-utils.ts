import { MoneyOperation } from '@/models/money-operation';
import { DateUtils } from './date-utils';

export class MoneyOperationUtils {
  public static parseFromResponse<T extends MoneyOperation>(data: T): T {
    return {
      ...data, 
      createdDate: new Date(data.createdDate),
      date: DateUtils.createFromDateOnlyString(data.date)
    };
  }
}