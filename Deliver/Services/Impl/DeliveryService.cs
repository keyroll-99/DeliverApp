using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using Models.Db;
using Models.Db.ConstValues;
using Models.Exceptions;
using Models.Mapper;
using Models.Request.Deliver;
using Models.Request.Utils.Role;
using Models.Response.Deliver;
using Repository.Repository.Interface;
using Services.Interface;
using Services.Interface.Utils;

namespace Services.Impl;

public class DeliveryService : IDeliveryService
{
    private readonly ILocationUtils _locationUtils;
    private readonly IRoleUtils _roleUtils;
    private readonly ICompanyUtils _companyUtils;
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly LoggedUser _loggedUser;

    public DeliveryService(
        ILocationUtils locationUtils,
        IRoleUtils roleUtils,
        ICompanyUtils companyUtils,
        IDeliveryRepository deliveryRepository,
        IOptions<LoggedUser> loggedUser)
    {
        _locationUtils = locationUtils;
        _roleUtils = roleUtils;
        _deliveryRepository = deliveryRepository;
        _companyUtils = companyUtils;
        _loggedUser = loggedUser.Value;
    }


    public async Task<DeliverResponse> CreateDelivery(CreateDeliveryRequest createDeliverRequest)
    {
        if (createDeliverRequest is null || !createDeliverRequest.IsValid)
        {
            throw new AppException(ErrorMessage.InvalidData);
        }

        if (!await HasPermissionToDeliveryAction(PermissionActionEnum.Create))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        var fromLocation = await _locationUtils.GetLoctaionByHash(createDeliverRequest.FromLocationHash);
        var toLocation = await _locationUtils.GetLoctaionByHash(createDeliverRequest.ToLocationHash);

        var delivery = createDeliverRequest.CreateDelivery();
        delivery.From = fromLocation;
        delivery.To = toLocation;

        await _deliveryRepository.AddAsync(delivery);

        return delivery.AsResponse();
    }

    public async Task<List<DeliverResponse>> GetAllDeliveries()
    {
        if (!await HasPermissionToDeliveryAction(PermissionActionEnum.Get))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        var deliveries = await _deliveryRepository
            .GetAll()
            .Include(x => x.To)
            .ThenInclude(y => y.Company)
            .ThenInclude(z => z.Users)
            .Include(x => x.From)
            .ThenInclude(y => y.Company)
            .ThenInclude(z => z.Users)
            .Include(x => x.Car)
            .Where(x => x.From.Company.Users.Any(x => x.Id == _loggedUser.Id) || x.To.Company.Users.Any(x => x.Id == _loggedUser.Id))
            .Select(x => x.AsResponse())
            .ToListAsync();

        return deliveries;
    }

    public async Task<DeliverResponse> ChangeStatus(ChangeDeliveryStatusRequest changeDeliverStatusRequest)
    {
        var delivery = await _deliveryRepository.GetAll().FirstOrDefaultAsync(x => x.Hash == changeDeliverStatusRequest.DeliveryHash);

        if (delivery is null)
        {
            throw new AppException(ErrorMessage.InvalidData);
        }

        if (!await HasPermissionToDeliveryAction(PermissionActionEnum.Update, delivery))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        delivery.Status = ((int)changeDeliverStatusRequest.NewStatus);

        await _deliveryRepository.UpdateAsync(delivery);

        return delivery.AsResponse();
    }

    public async Task<DeliverResponse> GetDeliveryByHash(Guid hash)
    {
        var delivery = await _deliveryRepository
            .GetAll()
            .FirstOrDefaultAsync(x => x.Hash == hash);

        if (delivery is null)
        {
            throw new AppException(ErrorMessage.InvalidData);
        }

        if (!await HasPermissionToDeliveryAction(PermissionActionEnum.Get, delivery))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        return delivery.AsResponse();
    }

    public async Task<DeliverResponse> UpdateDelivery(UpdateDeliveryRequest updateDeliverRequest)
    {
        if (!updateDeliverRequest.IsValid)
        {
            throw new AppException(ErrorMessage.InvalidData);
        }

        var delivery = await _deliveryRepository.GetAll().FirstOrDefaultAsync(x => x.Hash == updateDeliverRequest.DeliveryHash);

        if (delivery is null)
        {
            throw new AppException(ErrorMessage.InvalidData);
        }

        if (!await HasPermissionToDeliveryAction(PermissionActionEnum.Update, delivery))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        var fromLocation = await _locationUtils.GetLoctaionByHash(updateDeliverRequest.FromLocationHash);
        var toLocation = await _locationUtils.GetLoctaionByHash(updateDeliverRequest.ToLocationHash);

        delivery = delivery.UpdateDelivery(updateDeliverRequest);
        delivery.From = fromLocation;
        delivery.To = toLocation;

        await _deliveryRepository.UpdateAsync(delivery);

        return delivery.AsResponse();
    }

    private async Task<bool> HasPermissionToDeliveryAction(PermissionActionEnum action, Delivery? deliver = null)
    {
        var hasPermission = await _roleUtils.HasPermission(new HasPermissionRequest
        {
            Action = action,
            PermissionTo = PermissionToEnum.Deliver,
            Roles = _loggedUser.Roles
        });

        var isSameCompany = true;
        if (deliver is not null)
        {
            var userCompany = await _companyUtils.GetUserCompany(_loggedUser.Id);

            isSameCompany =
                (userCompany.Id == deliver.To.CompanyId
                || userCompany.Id == deliver.From.CompanyId);
        }

        return hasPermission && isSameCompany;
    }
};
