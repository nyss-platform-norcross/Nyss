using System.Linq;
using FluentValidation;
using RX.Nyss.Common.Utils.DataContract;
using RX.Nyss.Data.Concepts;
using RX.Nyss.Web.Features.DataCollectors.Commands;
using RX.Nyss.Web.Utils.Extensions;

namespace RX.Nyss.Web.Features.DataCollectors.Validation
{
    public class CreateDataCollectorCommandValidator : AbstractValidator<CreateDataCollectorCommand>
    {
        public CreateDataCollectorCommandValidator(IDataCollectorValidationService dataCollectorValidationService)
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty();

            RuleFor(dc => dc.DataCollectorType)
                .IsInEnum();

            RuleFor(dc => dc.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(dc => dc.PhoneNumber)
                .MaximumLength(20);

            RuleFor(dc => dc.PhoneNumber)
                .MustAsync(async (model, phoneNumber, t) => !await dataCollectorValidationService.PhoneNumberExists(phoneNumber))
                .WithMessageKey(ResultKey.DataCollector.PhoneNumberAlreadyExists);

            RuleFor(dc => dc.AdditionalPhoneNumber)
                .MaximumLength(20);

            RuleFor(dc => dc.SupervisorId)
                .GreaterThan(0);

            RuleFor(dc => dc.SupervisorId)
                .MustAsync(async (model, supervisorId, t) => await dataCollectorValidationService.IsAllowedToCreateForSupervisor(supervisorId))
                .WithMessageKey(ResultKey.DataCollector.NotAllowedToSelectSupervisor);

            When(dc => dc.DataCollectorType == DataCollectorType.Human, () =>
            {
                RuleFor(dc => dc.DisplayName).NotEmpty().MaximumLength(100);
                RuleFor(dc => dc.Sex).IsInEnum();
            });

            RuleFor(dc => dc.Deployed)
                .NotNull();

            RuleFor(dc => dc.Locations)
                .NotNull()
                .Must(locations => locations.Any());

            RuleForEach(dc => dc.Locations)
                .Must(dcl => dcl.VillageId > 0
                    && dcl.Latitude >= -90 && dcl.Latitude <= 90
                    && dcl.Longitude >= -180 && dcl.Longitude <= 180);

            RuleForEach(dc => dc.Locations)
                .Must((model, location, t) => model.Locations.Count(l => l.VillageId == location.VillageId && l.ZoneId == location.ZoneId) == 1)
                .WithMessageKey(ResultKey.DataCollector.DuplicateLocation);

            RuleFor(dc => dc.LinkedToHeadSupervisor)
                .NotNull();
        }
    }
}
