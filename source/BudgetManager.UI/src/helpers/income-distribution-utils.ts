import { IncomeDistribution } from '@/models/income-distribution';
import { IncomeDistributionRule } from '@/models/income-distribution-rule';
import { IncomeDistributionRuleType } from '@/models/income-distribution-rule-type.enum';
import { DisplayFormat } from './display-format';

export class IncomeDistributionUtils {
  static calculate(baseValue: number, rule: IncomeDistributionRule): {label: string, leftoverAmount: number} {
    if (rule.type === IncomeDistributionRuleType.Fixed) {
      const leftoverAmount = baseValue - rule.value;
      return { label: `${DisplayFormat.rounded(baseValue)} - ${DisplayFormat.rounded(rule.value)} = ${DisplayFormat.rounded(leftoverAmount)} left`, leftoverAmount };
    } else if (rule.type === IncomeDistributionRuleType.Percent) {
      const leftoverAmount = baseValue * (1 - (rule.value / 100));
      const value =  baseValue * (rule.value / 100);
      return { label: `${DisplayFormat.rounded(baseValue)} x ${DisplayFormat.rounded(rule.value)}% = ${DisplayFormat.rounded(value)} → (${DisplayFormat.rounded(leftoverAmount)} left)`, leftoverAmount };
    }
    throw new Error('Unhandled distribution rule type.');
  }

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
