namespace BudgetManager.Application.Features.BudgetManagement;

using System.Linq;
using AutoMapper;

public class IncomeAllocationTemplateMappingProfile : Profile
{
  public IncomeAllocationTemplateMappingProfile()
  {
    CreateMap<IncomeAllocationTemplateEntity, IncomeAllocationTemplate>()
  .ConstructUsing(src =>
    new IncomeAllocationTemplate(
      src.Id!,
      src.Name!,
      src.DefaultFundId!,
      src.Rules!.Select(x => new IncomeAllocationRule(x.Id!, (int)x.Value!, x.FundId!, (IncomeAllocationRuleType)x.Type!))
      )
    ).ForAllMembers(opt => opt.Ignore());

    CreateMap<IncomeAllocationTemplate, IncomeAllocationTemplateEntity>()
      .ForMember(x => x.Rules, opt => opt.MapFrom(x => x.Rules));


    CreateMap<IncomeAllocationTemplate, IncomeAllocationTemplateDto>()
      .ForMember(x => x.Rules, opt => opt.MapFrom(x => x.Rules))
      .ForMember(x => x.DefaultFundName, opt => opt.Ignore());

    CreateMap<IncomeAllocationRule, IncomeAllocationRuleDto>()
      .ForMember(x => x.FundName!, opt => opt.Ignore());

    CreateMap<IncomeAllocationRuleDto, IncomeAllocationRule>();
  }
}