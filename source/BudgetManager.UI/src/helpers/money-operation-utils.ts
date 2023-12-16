import { MoneyOperation } from '@/models/money-operation';
import { DateUtils } from './date-utils';

export class MoneyOperationUtils {

  public static copy(operation: MoneyOperation): MoneyOperation {
    return {
      ...operation,
      date: DateUtils.createDateOnlyString(new Date()),
      id: undefined,
    };
  }

  public static parseFromResponse<T extends MoneyOperation>(data: T): T {
    return {
      ...data,
      createdDate: new Date(data.createdDate).toLocaleString(),
    };
  }

  public static sort<T extends MoneyOperation>(data: T[]): T[] {
    return data.sort((a, b) => {
      const byDate = new Date(b.date).valueOf() - new Date(a.date).valueOf();
      return byDate === 0 ? b.type - a.type : byDate;
    });
  }
}