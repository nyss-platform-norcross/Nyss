namespace RX.Nyss.Common.Configuration;

public interface IBlobConfig
{
    string GeneralBlobContainerName { get; set; }
    string SmsGatewayBlobContainerName { get; set; }
    string PlatformAgreementsContainerName { get; set; }
    string PublicStatsBlobContainerName { get; set; }
    string SmsContentResourcesBlobObjectName { get; set; }
    string StringsResourcesBlobObjectName { get; set; }
    string EmailContentResourcesBlobObjectName { get; set; }
    string PlatformAgreementBlobObjectName { get; set; }
    string PublicStatsBlobObjectName { get; set; }
}