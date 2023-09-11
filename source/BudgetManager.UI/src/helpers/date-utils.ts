export class DateUtils {
  /**
   * @param dateString string in dd/mm/yy format
   * @returns Date
   */
  public static  createFromDateOnlyString(dateString:string ): Date {
    const [day, month, year] = dateString.split('/').map(Number);
    return new Date(year, month - 1, day);
  }
}