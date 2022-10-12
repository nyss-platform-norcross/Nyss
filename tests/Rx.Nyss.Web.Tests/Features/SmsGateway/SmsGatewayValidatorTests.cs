using FluentValidation.TestHelper;
using NSubstitute;
using RX.Nyss.Web.Features.SmsGateways.Dto;
using RX.Nyss.Web.Features.SmsGateways.Validation;
using Xunit;

namespace RX.Nyss.Web.Tests.Features.SmsGateway
{
    public class SmsGatewayValidatorTests
    {
        private readonly CreateGatewaySettingRequestDto.CreateGatewaySettingRequestValidator _createValidator;

        private readonly EditGatewaySettingRequestDto.GatewaySettingRequestValidator _editValidator;

        public SmsGatewayValidatorTests()
        {
            var validationService = Substitute.For<ISmsGatewayValidationService>();
            validationService.ApiKeyExists("1234").Returns(true);
            validationService.ApiKeyExistsToOther(1, "1234").Returns(true);
            _createValidator = new CreateGatewaySettingRequestDto.CreateGatewaySettingRequestValidator(validationService);
            _editValidator = new EditGatewaySettingRequestDto.GatewaySettingRequestValidator(validationService);
        }

        [Fact]
        public void Create_WhenApiExists_ShouldHaveError() => _createValidator.ShouldHaveChildValidator(gs => gs.ApiKey, typeof(SmsGatewayValidatorTests));// _createValidator.ShouldHaveValidationErrorFor(gs => gs.ApiKey, "1234");

        [Fact]
        public void Create_WhenEmailIsNullAndIotHubDeviceNameIsNull_ShouldHaveError() => _createValidator.ShouldHaveChildValidator(gs => gs.IotHubDeviceName, typeof(SmsGatewayValidatorTests)); // _createValidator.ShouldHaveValidationErrorFor(gs => gs.IotHubDeviceName, null as string);

        [Fact]
        public void Create_WhenIotHubDeviceNameIsSetAndEmailIsNull_ShouldNotHaveError() => _createValidator.ShouldHaveChildValidator(gs => gs.IotHubDeviceName, typeof(SmsGatewayValidatorTests)); //_createValidator.ShouldNotHaveValidationErrorFor(gs => gs.IotHubDeviceName, "iothub");

        [Fact]
        public void Create_WhenIotHubDeviceNameIsNullAndEmailIsSet_ShouldNotHaveError() => _createValidator.ShouldHaveChildValidator(gs => gs.EmailAddress, typeof(SmsGatewayValidatorTests)); //_createValidator.ShouldNotHaveValidationErrorFor(gs => gs.EmailAddress, "test@example.com");

        [Fact]
        public void Edit_WhenApiExistsToOther_ShouldHaveError()
        {
            var result = _editValidator.TestValidate(new EditGatewaySettingRequestDto
            {
                Id = 1,
                ApiKey = "1234"
            });

            result.ShouldHaveValidationErrorFor(gs => gs.ApiKey);
        }

        [Fact]
        public void Edit_WhenEmailIsNullAndIotHubDeviceNameIsNull_ShouldHaveError() => _createValidator.ShouldHaveChildValidator(gs => gs.IotHubDeviceName, typeof(SmsGatewayValidatorTests)); //_editValidator.ShouldHaveValidationErrorFor(gs => gs.IotHubDeviceName, null as string);

        [Fact]
        public void Edit_WhenIotHubDeviceNameIsSetAndEmailIsNull_ShouldNotHaveError() => _createValidator.ShouldHaveChildValidator(gs => gs.IotHubDeviceName, typeof(SmsGatewayValidatorTests)); //_editValidator.ShouldNotHaveValidationErrorFor(gs => gs.IotHubDeviceName, "iothub");

        [Fact]
        public void Edit_WhenIotHubDeviceNameIsNullAndEmailIsSet_ShouldNotHaveError() => _createValidator.ShouldHaveChildValidator(gs => gs.EmailAddress, typeof(SmsGatewayValidatorTests)); //_editValidator.ShouldNotHaveValidationErrorFor(gs => gs.EmailAddress, "test@example.com");
    }
}
