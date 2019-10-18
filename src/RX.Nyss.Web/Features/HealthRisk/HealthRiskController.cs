using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RX.Nyss.Web.Utils;
using RX.Nyss.Data.Concepts;

namespace RX.Nyss.Web.Features.HealthRisk
{
    [Route("api/healthrisk")]
    public class HealthRiskController : BaseController
    {
        private readonly IHealthRiskService _healthRiskService;

        public HealthRiskController(IHealthRiskService healthRiskService)
        {
            _healthRiskService = healthRiskService;
        }

        /// <summary>
        /// Gets a list of all health risks.
        /// </summary>
        /// <returns>A list of health risks</returns>
        [Route("getHealthRisks")]
        [Roles(Role.Administrator, Role.GlobalCoordinator)]
        public async Task<IEnumerable<HealthRiskDto>> GetHealthRisks() => await _healthRiskService.GetHealthRisks();
    }
}