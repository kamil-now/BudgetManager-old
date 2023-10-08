import { MoneyOperation } from '@/models/money-operation';

export class MoneyOperationUtils {
  public static parseFromResponse<T extends MoneyOperation>(data: T): T {
    return {
      ...data,
      createdDate: new Date(data.createdDate),
      date: new Date(data.date)
    };
  }
  public static sort<T extends MoneyOperation>(data: T[]): T[] {
    return data.sort((a, b) => {
      const byDate = new Date(b.date).valueOf() - new Date(a.date).valueOf();
      return byDate === 0 ? new Date(b.createdDate).valueOf() - new Date(a.createdDate).valueOf() : byDate;
    });
  } 
}