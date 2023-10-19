﻿using RX.Nyss.Web.Services.EidsrClient.Dto;

namespace RX.Nyss.Common.Services.DhisClient.Dto;
public class DhisRegisterReportRequestTemplate
{
    public string Program { get; set; }

    public EidsrApiProperties EidsrApiProperties { get; set; }

    public string ReportLocationDataElementId { get; set; }
    
    public string ReportSuspectedDiseaseDataElementId { get; set; }

    public string ReportHealthRiskDataElementId { get; set; }

    public string ReportStatusDataElementId { get; set; }

    public string ReportGenderDataElementId { get; set; }

    public string ReportAgeAtLeastFiveDataElementId { get; set; }

    public string ReportAgeBelowFiveDataElementId { get; set; }
}
