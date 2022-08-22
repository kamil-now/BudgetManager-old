export class GestureDetector {
  onSwipeLeft: (() => void)[] = []
  onSwipeRight: (() => void)[] = []
  onSwipeUp: (() => void)[] = []
  onSwipeDown: (() => void)[] = []
  onTap: (() => void)[] = []

  private readonly MIN_DIFF = 50;
  private touchStartX = 0;
  private touchEndX = 0;
  private touchStartY = 0;
  private touchEndY = 0;
  private touchStartHandler = (event: TouchEvent) => this.detectTouchStart(event)
  private touchEndHandler = (event: TouchEvent) => this.detectTouchEnd(event)

  constructor(private readonly element: HTMLElement) {
    element.addEventListener(
      "touchstart",
      this.touchStartHandler,
      false
    );

    element.addEventListener(
      "touchend",
      this.touchEndHandler,
      false
    );
  }
  destroy() {
    this.element.removeEventListener(
      "touchstart",
      this.touchStartHandler
    );
    this.element.removeEventListener(
      "touchend",
      this.touchEndHandler
    );
  }

  private detectTouchStart(event: TouchEvent): void {
    this.touchStartX = event.changedTouches[0].screenX;
    this.touchStartY = event.changedTouches[0].screenY;
  }

  private detectTouchEnd(event: TouchEvent): void {
    this.touchEndX = event.changedTouches[0].screenX;
    this.touchEndY = event.changedTouches[0].screenY;
    this.handleGesture();
  }

  private handleGesture(): void {
    if (this.touchEndX < this.touchStartX
      && this.touchStartX - this.touchEndX > this.MIN_DIFF
    ) {
      this.onSwipeLeft.forEach(action => action());
    }

    if (this.touchEndX > this.touchStartX
      && this.touchEndX - this.touchStartX > this.MIN_DIFF
    ) {
      this.onSwipeRight.forEach(action => action());
    }

    if (this.touchEndY < this.touchStartY
      && this.touchStartY - this.touchEndY > this.MIN_DIFF
    ) {
      this.onSwipeUp.forEach(action => action());
    }

    if (this.touchEndY > this.touchStartY
      && this.touchEndY - this.touchStartY > this.MIN_DIFF
    ) {
      this.onSwipeDown.forEach(action => action());
    }

    if (this.touchEndY === this.touchStartY) {
      this.onTap.forEach(action => action());
    }
  }
}