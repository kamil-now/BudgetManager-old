import { ArrayUtils } from '../src/helpers/array-utils';

describe(ArrayUtils.name, () => {
  describe(ArrayUtils.move.name, () => {
    let source: number[];
    let target: number[];
    let condition: (x: number) => boolean;
    let expectedSource: number[];
    let expectedTarget: number[];

    beforeEach(() => {
      source = [1, 2, 3, 8, 9];
      target = [4, 5, 6, 7];
      condition = (x: number) => x > 3;

      expectedSource = [1, 2, 3];
      expectedTarget = [4, 5, 6, 7, 8, 9];
    });

    it('should add to target source elements that match specified condition', () => {
      ArrayUtils.move(source, target, condition);
      expect(target)
        .toEqual(expectedTarget);
    });

    it('should remove from source elements that match specified condition', () => {
      ArrayUtils.move(source, target, condition);
      expect(source)
        .toEqual(expectedSource);
    });
  });
});
