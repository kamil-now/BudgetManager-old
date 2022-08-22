export class Debouncer<T> {
  private _timeoutRef!: number
  constructor(
    private readonly _func: (args?: T) => void,
    private readonly _wait: number = 1000
  ) {
  }

  run(args?: T): void {
    this.cancel();
    this._timeoutRef = window.setTimeout(() => this._func(args), this._wait);
  }

  cancel(): void {
    clearTimeout(this._timeoutRef);
  }
}
