import { MoneyOperation } from '@/models/money-operation';
import { DateUtils } from './date-utils';

export class MoneyOperationUtils {
  public static parseFromResponse<T extends MoneyOperation>(data: T): T {
    return {
      ...data,
      createdDate: new Date(data.createdDate),
      date: DateUtils.createFromDateOnlyString(data.date as string)
    };
  }
  public static sort<T extends MoneyOperation>(data: T[]): T[] {
    return data.sort((a, b) => {
      const byDate = new Date(b.date).valueOf() - new Date(a.date).valueOf();
      return byDate === 0 ? new Date(b.createdDate).valueOf() - new Date(a.createdDate).valueOf() : byDate;
    });
  } 
}