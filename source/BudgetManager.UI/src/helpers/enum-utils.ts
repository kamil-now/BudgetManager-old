export class EnumUtils {
  public static getStringValues(enumType: object): string[] {
    return Object.keys(enumType).filter(
      (item) => !isNaN(Number(item)) && item !== '0'
    );
  }
}