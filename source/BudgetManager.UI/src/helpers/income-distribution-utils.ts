import { IncomeDistribution } from '@/models/income-distribution';
import { IncomeDistributionRule } from '@/models/income-distribution-rule';
import { IncomeDistributionRuleType } from '@/models/income-distribution-rule-type.enum';
import { DisplayFormat } from './display-format';

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

  static calculate(baseValue: number, rule: IncomeDistributionRule): {label: string, leftoverAmount: number} {
    if (rule.type === IncomeDistributionRuleType.Fixed) {
      const leftoverAmount = baseValue - rule.value;
      return { label: this.getFixedRuleLabel(baseValue, rule.value, leftoverAmount), leftoverAmount };
    } else if (rule.type === IncomeDistributionRuleType.Percent) {
      const leftoverAmount = baseValue * (1 - (rule.value / 100));
      return { label: this.getPercentRuleLabel(baseValue, rule.value, leftoverAmount), leftoverAmount };
    }
    throw new Error(`Unhandled distribution rule type ${rule.type}.`);
  }

  private static getFixedRuleLabel(baseValue: number, ruleValue: number, leftover: number): string {
    return `${DisplayFormat.rounded(baseValue)} - ${DisplayFormat.rounded(ruleValue)} = ${DisplayFormat.rounded(leftover)} left`;
  }

  private static getPercentRuleLabel(baseValue: number, ruleValue: number, leftover: number): string {
    const value =  baseValue * (ruleValue / 100);
    return `${DisplayFormat.rounded(baseValue)} x ${DisplayFormat.rounded(ruleValue)}% = ${DisplayFormat.rounded(value)} → ${DisplayFormat.rounded(leftover)} left`;
  }
}
