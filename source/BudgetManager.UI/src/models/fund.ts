export type Fund = {
  id?: string,
  name: string,
  balance?: { [currency: string]: number},
  isDefault?: boolean
}