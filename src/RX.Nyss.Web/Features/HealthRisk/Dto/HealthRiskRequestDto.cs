using System.Collections.Generic;
using FluentValidation;
using RX.Nyss.Data.Concepts;

namespace RX.Nyss.Web.Features.HealthRisk.Dto
{
    public class HealthRiskRequestDto
    {
        public int HealthRiskCode { get; set; }

        public HealthRiskType HealthRiskType { get; set; }

        public int? AlertRuleCountThreshold { get; set; }

        public int? AlertRuleDaysThreshold { get; set; }

        public int? AlertRuleKilometersThreshold { get; set; }

        public IEnumerable<HealthRiskLanguageContentDto> LanguageContent { get; set; }

        public class Validator : AbstractValidator<HealthRiskRequestDto>
        {
            public Validator()
            {
                RuleFor(hr => hr.HealthRiskCode).GreaterThan(0);
                RuleFor(hr => hr.HealthRiskType).IsInEnum();
                RuleFor(hr => hr.LanguageContent).NotEmpty();
                RuleFor(hr => hr.AlertRuleCountThreshold).GreaterThanOrEqualTo(0);
                RuleFor(hr => hr.AlertRuleDaysThreshold).GreaterThanOrEqualTo(0);
                RuleFor(hr => hr.AlertRuleKilometersThreshold).GreaterThanOrEqualTo(0);
            }
        }
    }
}
