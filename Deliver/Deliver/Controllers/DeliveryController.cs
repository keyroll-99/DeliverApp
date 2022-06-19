using Deliver.CustomAttribute;
using Microsoft.AspNetCore.Mvc;
using Models.Db.ConstValues;
using Models.Exceptions;
using Models.Request.Deliver;
using Models.Response._Core;
using Models.Response.Deliver;
using Services.Interface;

namespace Deliver.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliverService;

        public DeliveryController(IDeliveryService deliverService)
        {
            _deliverService = deliverService;
        }

        [HttpPost]
        [Authorize(SystemRoles.Admin, SystemRoles.CompanyAdmin, SystemRoles.CompanyOwner, SystemRoles.Dispatcher)]
        public async Task<DeliverResponse> CreateDelivery(CreateDeliveryRequest request)
        {
            return await _deliverService.CreateDelivery(request);
        }

        [HttpGet("List")]
        [Authorize(SystemRoles.Admin, SystemRoles.CompanyAdmin, SystemRoles.CompanyOwner, SystemRoles.Dispatcher, SystemRoles.Driver)]
        public async Task<List<DeliverResponse>> GetAll()
        {
            return await _deliverService.GetAllDeliveries();
        }

        [HttpGet("{hash}")]
        [Authorize(SystemRoles.Admin, SystemRoles.CompanyAdmin, SystemRoles.CompanyOwner, SystemRoles.Dispatcher, SystemRoles.Driver)]
        public async Task<DeliverResponse> GetByHash(Guid hash)
        {
            return await _deliverService.GetDeliveryByHash(hash);
        }

        [HttpPut("Status")]
        [Authorize(SystemRoles.Admin, SystemRoles.CompanyAdmin, SystemRoles.CompanyOwner, SystemRoles.Dispatcher)]
        public async Task<DeliverResponse> ChangeStatus(ChangeDeliveryStatusRequest changeDeliverStatusRequest)
        {
            if(!Enum.IsDefined(typeof(DeliveryStatusEnum), changeDeliverStatusRequest.NewStatus))
            {
                return BaseRespons<DeliverResponse>.Fail(ErrorMessage.InvalidData);
            }
            return await _deliverService.ChangeStatus(changeDeliverStatusRequest);
        }

        [HttpPut]
        [Authorize(SystemRoles.Admin, SystemRoles.CompanyAdmin, SystemRoles.CompanyOwner, SystemRoles.Dispatcher)]
        public async Task<DeliverResponse> UpdateDelivery(UpdateDeliveryRequest updateDeliveryRequest)
        {
            return await _deliverService.UpdateDelivery(updateDeliveryRequest);
        }
    }
}
