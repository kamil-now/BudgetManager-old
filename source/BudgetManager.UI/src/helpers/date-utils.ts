export class DateUtils {

  public static createDateOnlyString(date: Date): string {
    let day = `${date.getDate()}`;
    if (day.length === 1) {
      day = `0${day}`;
    }
    let month = `${date.getMonth() + 1}`;
    if (month.length === 1) {
      month = `0${month}`;
    }
    return `${date.getFullYear()}/${month}/${day}`;
  }
}