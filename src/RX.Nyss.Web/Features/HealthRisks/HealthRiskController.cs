using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RX.Nyss.Common.Utils.DataContract;
using RX.Nyss.Data.Concepts;
using RX.Nyss.Web.Features.HealthRisks.Dto;
using RX.Nyss.Web.Utils;

namespace RX.Nyss.Web.Features.HealthRisks
{
    [Route("api/healthRisk")]
    public class HealthRiskController : BaseController
    {
        private readonly IHealthRiskService _healthRiskService;

        public HealthRiskController(IHealthRiskService healthRiskService)
        {
            _healthRiskService = healthRiskService;
        }

        /// <summary>
        /// Gets a list with basic information of all health risks.
        /// </summary>
        /// <returns>A list of basic information of health risks</returns>
        [HttpGet, Route("list")]
        [NeedsRole(Role.Administrator, Role.GlobalCoordinator)]
        public async Task<Result<IEnumerable<HealthRiskListItemResponseDto>>> List() =>
            await _healthRiskService.List();

        /// <summary>
        /// Gets a health risk with all values for editing.
        /// </summary>
        /// <param name="id">An identifier of a health risk</param>
        /// <returns>A health risk</returns>
        [HttpGet, Route("{id:int}/get")]
        [NeedsRole(Role.Administrator, Role.GlobalCoordinator)]
        public async Task<Result<HealthRiskResponseDto>> Get(int id) =>
            await _healthRiskService.Get(id);

        /// <summary>
        /// Creates a new health risk.
        /// </summary>
        /// <param name="healthRiskRequestDto"></param>
        /// <returns>An identifier of the created health risk</returns>
        [HttpPost, Route("create"), NeedsRole(Role.Administrator, Role.GlobalCoordinator)]
        public async Task<Result> Create([FromBody] HealthRiskRequestDto healthRiskRequestDto) =>
            await _healthRiskService.Create(healthRiskRequestDto);

        /// <summary>
        /// Edits a health risk.
        /// </summary>
        /// <param name="id">An identifier of a health risk</param>
        /// <param name="healthRiskRequestDto">A health risk</param>
        /// <returns></returns>
        [HttpPost, Route("{id:int}/edit"), NeedsRole(Role.Administrator, Role.GlobalCoordinator)]
        public async Task<Result> Edit(int id, [FromBody] HealthRiskRequestDto healthRiskRequestDto) =>
            await _healthRiskService.Edit(id, healthRiskRequestDto);

        /// <summary>
        /// Deletes a health risk.
        /// </summary>
        /// <param name="id">An identifier of a health risk</param>
        /// <returns></returns>
        [HttpPost, Route("{id:int}/delete"), NeedsRole(Role.Administrator, Role.GlobalCoordinator)]
        public async Task<Result> Delete(int id) =>
            await _healthRiskService.Delete(id);


        /// <summary>
        /// Get the data required to build a "create new health risk" form
        /// </summary>
        /// <param name="healthRiskId">The id of the health risk</param>
        /// <returns>An object containing a list of suspected diseases</returns>
        /*[HttpGet, Route("getFormData")]
        [NeedsRole(Role.Administrator, Role.TechnicalAdvisor, Role.Manager, Role.Coordinator)]
        public async Task<Result<HealthRiskFormDataResponseDto>> GetFormData(int healthRiskId) =>
            await _healthRiskService.GetFormData(healthRiskId);*/
    }
}
