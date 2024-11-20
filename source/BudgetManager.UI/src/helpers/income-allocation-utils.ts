import { IncomeAllocation } from '@/models/income-allocation';
import { IncomeAllocationRule } from '@/models/income-allocation-rule';
import { IncomeAllocationRuleType } from '@/models/income-allocation-rule-type.enum';
import { DisplayFormat } from './display-format';
import { Fund } from '@/models/fund';

export class IncomeAllocationUtils {
  static createNew(defaultFund: Fund): IncomeAllocation {
    return {
      id: undefined,
      name: '',
      rules: [],
      defaultFundId: defaultFund.id,
      defaultFundName: defaultFund.name
    };
  }

  static copy(incomeAllocation: IncomeAllocation): IncomeAllocation {
    return {
      ...incomeAllocation,
      rules: [...incomeAllocation.rules.map(x => ({ ...x }))]
    };
  }

  static calculate(baseValue: number, rule: IncomeAllocationRule): {label: string, leftoverAmount: number} {
    if (rule.type === IncomeAllocationRuleType.Fixed) {
      const leftoverAmount = baseValue - rule.value;
      return { label: this.getFixedRuleLabel(baseValue, rule.value, leftoverAmount), leftoverAmount };
    } else if (rule.type === IncomeAllocationRuleType.Percent) {
      const leftoverAmount = baseValue * (1 - (rule.value / 100));
      return { label: this.getPercentRuleLabel(baseValue, rule.value, leftoverAmount), leftoverAmount };
    }
    throw new Error(`Unhandled allocation rule type ${rule.type}.`);
  }

  private static getFixedRuleLabel(baseValue: number, ruleValue: number, leftover: number): string {
    return `${DisplayFormat.rounded(baseValue)} - ${DisplayFormat.rounded(ruleValue)} = ${DisplayFormat.rounded(leftover)} left`;
  }

  private static getPercentRuleLabel(baseValue: number, ruleValue: number, leftover: number): string {
    const value =  baseValue * (ruleValue / 100);
    return `${DisplayFormat.rounded(baseValue)} x ${DisplayFormat.rounded(ruleValue)}% = ${DisplayFormat.rounded(value)} → ${DisplayFormat.rounded(leftover)} left`;
  }
}
