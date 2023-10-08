export class DateUtils {

  public static createDateOnlyString(date: Date): string {
    return `${date.getFullYear()}/${date.getMonth() + 1}/${date.getDate()}`;
  }
}