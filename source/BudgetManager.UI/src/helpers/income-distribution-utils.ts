import { IncomeDistribution } from '@/models/income-distribution';

export class IncomeDistributionUtils {
  static createNew(): IncomeDistribution {
    return {
      rules: []
    };
  }

  static copy(incomeDistribution: IncomeDistribution): IncomeDistribution {
    return {
      ...incomeDistribution,
      rules: [...incomeDistribution.rules.map(x => ({ ...x }))]
    };
  }
}
