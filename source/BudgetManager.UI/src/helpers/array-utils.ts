export class ArrayUtils {
  static move<T>(
    source: T[],
    target: T[],
    condition: (item: T) => boolean
  ): void {
    target.push(...source.filter(condition));

    let i = source.length;
    while (--i) {
      if (condition(source[i])) {
        source.splice(i, 1);
      }
    }
  }

  static getLast<T>(array: T[]): T {
    return array[array.length - 1];
  }
}
