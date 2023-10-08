import currencies from '@/assets/currencies.json';
import { MoneyOperation } from '@/models/money-operation';
import { MoneyOperationType } from '@/models/money-operation-type.enum';
import { DateUtils } from './date-utils';

export class MoneyOperationUtils {
  public static createNew(type: MoneyOperationType): MoneyOperation {
    return {
      id: undefined,
      title: '',
      type,
      date: DateUtils.createDateOnlyString(new Date()),
      value: {
        currency: Object.keys(currencies)[0],
        amount: 0,
      },
      createdDate: new Date().toString(),
    };
  }

  public static createCopy(operation: MoneyOperation): MoneyOperation {
    return {
      ...operation,
      date: DateUtils.createDateOnlyString(new Date()),
      id: undefined,
    };
  }

  public static parseFromResponse<T extends MoneyOperation>(data: T): T {
    return {
      ...data,
      createdDate: new Date(data.createdDate).toDateString(),
    };
  }
  public static sort<T extends MoneyOperation>(data: T[]): T[] {
    return data.sort((a, b) => {
      const byDate = new Date(b.date).valueOf() - new Date(a.date).valueOf();
      return byDate === 0 ? new Date(b.createdDate).valueOf() - new Date(a.createdDate).valueOf() : byDate;
    });
  }
}