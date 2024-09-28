namespace BudgetManager.Application.Features.BudgetManagement;

using System.Linq;
using AutoMapper;

public class IncomeDistributionTemplateMappingProfile : Profile
{
  public IncomeDistributionTemplateMappingProfile()
  {
    CreateMap<IncomeDistributionTemplateEntity, IncomeDistributionTemplate>()
  .ConstructUsing(src =>
    new IncomeDistributionTemplate(
      src.Id!,
      src.Name!,
      src.DefaultFundId!,
      src.Rules!.Select(x => new IncomeDistributionRule(x.Id!, (int)x.Value!, x.FundId!, (IncomeDistributionRuleType)x.Type!))
      )
    ).ForAllMembers(opt => opt.Ignore());

    CreateMap<IncomeDistributionTemplate, IncomeDistributionTemplateEntity>()
      .ForMember(x => x.Rules, opt => opt.MapFrom(x => x.Rules));


    CreateMap<IncomeDistributionTemplate, IncomeDistributionTemplateDto>()
      .ForMember(x => x.Rules, opt => opt.MapFrom(x => x.Rules))
      .ForMember(x => x.DefaultFundName, opt => opt.Ignore());

    CreateMap<IncomeDistributionRule, IncomeDistributionRuleDto>()
      .ForMember(x => x.FundName!, opt => opt.Ignore());

    CreateMap<IncomeDistributionRuleDto, IncomeDistributionRule>();
  }
}