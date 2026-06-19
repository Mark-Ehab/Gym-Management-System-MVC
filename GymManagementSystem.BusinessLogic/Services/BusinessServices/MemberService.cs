using AutoMapper;
using GymManagementSystem.BusinessLogic.BusinessSpecifictions;
using GymManagementSystem.BusinessLogic.BusinessSpecifictions.HealthRecordSpecifications;
using GymManagementSystem.BusinessLogic.BusinessSpecifictions.MemberSpecifications;
using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.Contracts.AttachmentService;
using GymManagementSystem.BusinessLogic.Contracts.BusinessServices;
using GymManagementSystem.BusinessLogic.DTOs.HealthRecordDTOs;
using GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;
using GymManagementSystem.BusinessLogic.Helpers.BusinessErrors;
using GymManagementSystem.DataAccess.Enums;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Repositories.Contracts;
using GymManagementSystem.DataAccess.UoW.Contract;
using GymManagementSystem.DataAccess.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Services.BusinessServices;

public sealed class MemberService : IMemberService
{
    /* Fields */
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<MemberService> _logger;
    private readonly IAttachmentService _attachmentService;

    /* Constructor */
    public MemberService(IUnitOfWork unitOfWork,
                         IMapper mapper,
                         ILogger<MemberService> logger,
                         IAttachmentService attachmentService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _attachmentService = attachmentService;
    }

    /* Methods */
    public async Task<IEnumerable<AllMembersDTO>> GetAllMembersAsync(CancellationToken ct = default)
    {
        var memberRepo = _unitOfWork.GetGenericRepository<Member>();

        var allMembers = await memberRepo.GetAllAsync(ct: ct);

        if (allMembers is null || allMembers.Count == 0)
        {
            _logger.LogWarning("No members are retrieved when member's index page is hitted");
            return []; 
        }

        var allMembersDTOs = _mapper.Map<IEnumerable<AllMembersDTO>>(allMembers);

        _logger.LogInformation("Members: {@Members} are retrieved successfully", allMembersDTOs.Select(m => new
        {
            m.Id,
            m.Name
        }));  
        return allMembersDTOs;
    }

    public async Task<Result<MemberDetailsDTO>> GetMemberDetailsAsync(Guid id, CancellationToken ct = default)
    {
        var memberRepo = _unitOfWork.GetGenericRepository<Member>();
        var memberDetailsWithActiveMembershipSpecification = new MemberDetailsWithActiveMembershipSpecification(id);

        var member = await memberRepo.FirstOrDefaultAsync(memberDetailsWithActiveMembershipSpecification, ct: ct);

        if(member is null)
        {
            _logger.LogWarning("Member of Id ({@Id}) is not found",id);
            return Result<MemberDetailsDTO>.Failure(MemberBusinessErrors.MemberDetailsNotFound);
        }
        var memberDetailsDTO = _mapper.Map<MemberDetailsDTO>(member);

        _logger.LogInformation("Member {@Member} is retrieved successfully", new { Id = id ,memberDetailsDTO.Name});
        return Result<MemberDetailsDTO>.Success(memberDetailsDTO);
    }

    public async Task<Result<HealthRecordDTO>> GetMemberHealthRecordDetailsAsync(Guid id, CancellationToken ct = default)
    {
        var healthRepo = _unitOfWork.GetGenericRepository<HealthRecord>();
        var memberHealthRecordDetailsSpecification = new MemberHealthRecordDetailsSpecification(id);
        var healthRecord = await healthRepo.FirstOrDefaultAsync(memberHealthRecordDetailsSpecification, ct: ct);

        if(healthRecord is null)
        {
            _logger.LogWarning("No health record was found for member of Id ({@Id})",id);
            return Result<HealthRecordDTO>.Failure(MemberBusinessErrors.MemberHealthRecordDetailsNotFound); 
        }

        var healthRecordDTO = _mapper.Map<HealthRecordDTO>(healthRecord);

        _logger.LogInformation("Health record {@HealthRecord} for member of Id {@Id} is retrieved successfully", healthRecordDTO, id);
        return Result<HealthRecordDTO>.Success(healthRecordDTO);
    }

    public async Task<Result> AddMemberAsync(MemberCreateDTO memberToBeCreatedDTO, CancellationToken ct = default)
    {
        var memberRepo = _unitOfWork.GetGenericRepository<Member>();

        /* Check if passed Email already exits */
        var newMemberWithUniqueEmailSpecification = new NewMemberWithUniqueEmailSpecification(memberToBeCreatedDTO.Email);
        if (await memberRepo.AnyAsync(newMemberWithUniqueEmailSpecification, ct))
        {
            _logger.LogWarning("Email ({@Email}) of new member is not unique and already exists on the system", memberToBeCreatedDTO.Email);
            return Result.Failure(MemberBusinessErrors.MemberEmailAlreadyExists); 
        }
        
        /* Check if passed Phone already exits */
        var newMemberWithUniquePhoneNumberSpecification = new NewMemberWithUniquePhoneNumberSpecification(memberToBeCreatedDTO.Phone);
        if (await memberRepo.AnyAsync(newMemberWithUniquePhoneNumberSpecification,ct))
        {
            _logger.LogWarning("Phone number ({@PhoneNumber}) of new member is not unique and already exists on the system", memberToBeCreatedDTO.Phone);
            return Result.Failure(MemberBusinessErrors.MemberPhoneNumberAlreadyExists);
        }

        var memberToBeCreated = _mapper.Map<Member>(memberToBeCreatedDTO);

        /* Upload member profile picture if any */
        if(memberToBeCreatedDTO.PhotoFile is not null)
        {
            var result = await _attachmentService.SaveFileAsync(memberToBeCreatedDTO.PhotoFile,"MemberImages",true,ct);

            if (result.IsFailure)
                return Result.Failure(result.Error!);

            memberToBeCreated.Photo = result.Value;
        }

        memberRepo.Add(memberToBeCreated);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
        {
            /* Delete member profile picture if any */
            if (memberToBeCreated.Photo is not null)
            {
                _attachmentService.DeleteFile(memberToBeCreated.Photo, "MemberImages");
            }
            _logger.LogError("Member {@Name} is not created due to an internal DB error", memberToBeCreatedDTO.Name);
            return Result.Failure(MemberBusinessErrors.MemberNotCreated);
        }

        _logger.LogInformation("Member {@Name} is created successfully with Id:{@Id}", memberToBeCreatedDTO.Name, memberToBeCreated.Id);
        return Result.Success();
    }

    public async Task<Result<MemberToBeEditedDTO>> GetMemberToBeEditedAsync(Guid id, CancellationToken ct = default)
    {
        var memberRepo = _unitOfWork.GetGenericRepository<Member>();

        var memberToBeEdited = await memberRepo.GetByIdAsync(id, ct: ct);

        if (memberToBeEdited is null)
        {
            _logger.LogWarning("Member to be edited of Id ({@Id}) is not found", id);
            return Result<MemberToBeEditedDTO>.Failure(MemberBusinessErrors.MemberToBeEditedNotFound);
        }

        _logger.LogInformation("Member to be edited of Id ({@Id}) is retrieved successfully", id);
        return Result<MemberToBeEditedDTO>.Success(_mapper.Map<MemberToBeEditedDTO>(memberToBeEdited));
    }

    public async Task<Result> EditMemberAsync(Guid id, MemberToBeEditedDTO memberToBeEditedDTO, CancellationToken ct = default)
    {
        var memberRepo = _unitOfWork.GetGenericRepository<Member>();

        /* Check if passed Email already exits */
        var editMemberWithUniqueEmailSpecification = new EditMemberWithUniqueEmailSpecification(id, memberToBeEditedDTO.Email);
        if (await memberRepo.AnyAsync(editMemberWithUniqueEmailSpecification, ct))
        {
            _logger.LogWarning("Updated email ({@Email}) for member to be edited of Id ({@id}) is not unique and already exists on the system", memberToBeEditedDTO.Email,id);
            return Result.Failure(MemberBusinessErrors.MemberEmailAlreadyExists);
        }

        /* Check if passed Phone already exits */
        var editMemberWithUniquePhoneNumberSpecification = new EditMemberWithUniquePhoneNumberSpecification(id, memberToBeEditedDTO.Phone);
        if (await memberRepo.AnyAsync(editMemberWithUniquePhoneNumberSpecification, ct))
        {
            _logger.LogWarning("Updated phone number ({@PhoneNumber}) for member to be edited of Id ({@id}) is not unique and already exists on the system", memberToBeEditedDTO.Phone,id);
            return Result.Failure(MemberBusinessErrors.MemberPhoneNumberAlreadyExists);
        }

        var memberToBeEdited = await memberRepo.GetByIdAsync(id,ct: ct);

        if(memberToBeEdited is null)
        {
            _logger.LogWarning("Member to be edited of Id ({@Id}) is not found", id);
            return Result.Failure(MemberBusinessErrors.MemberToBeEditedNotFound);
        }

        _mapper.Map(memberToBeEditedDTO,memberToBeEdited);

        /* Check if member profile picture is edited */
        if(memberToBeEditedDTO.PhotoFile is not null)
        {
            if(memberToBeEdited.Photo is not null)
                _attachmentService.DeleteFile(memberToBeEdited.Photo, "MemberImages");

            var result = await _attachmentService.SaveFileAsync(memberToBeEditedDTO.PhotoFile, "MemberImages", true, ct);

            if (result.IsFailure)
                return Result.Failure(result.Error!);

            memberToBeEdited.Photo = result.Value;
        }

        memberRepo.Update(memberToBeEdited);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
        {
            _logger.LogError("Member of Id ({@Id}) is not updated due to an internal DB error", id);
            return Result.Failure(MemberBusinessErrors.MemberNotEdited);
        }

        _logger.LogInformation("Member of Id ({@Id}) is updated successfully",id);
        return Result.Success();
    }

    public async Task<Result> DeleteMemberAsync(Guid id, CancellationToken ct = default)
    {
        var memberRepo = _unitOfWork.GetGenericRepository<Member>();

        var memberToBeDeleted = await memberRepo.GetByIdAsync(id, ct: ct);

        if (memberToBeDeleted is null)
        {
            _logger.LogWarning("Member to be deleted of Id ({@Id}) is not found", id);
            return Result.Failure(MemberBusinessErrors.MemberToBeDeletedNotFound);
        }

        var bookingRepo = _unitOfWork.GetGenericRepository<Booking>();
        var memberActiveCurrentOrFutureBookingsSpecification = new MemberActiveCurrentOrFutureBookingsSpecification(id);

        var memberHasActiveCurrentOrFutureBookings = await bookingRepo.AnyAsync(memberActiveCurrentOrFutureBookingsSpecification,ct);

        if (memberHasActiveCurrentOrFutureBookings)
        {
            _logger.LogWarning("Member of Id {@Id} has active current or future bookings", id);
            return Result.Failure(MemberBusinessErrors.MemberWithActiveCurrentOrFutureBookingsCannotBeDeleted);
        }

        memberRepo.SoftDelete(memberToBeDeleted);

        var healthRecordRepo = _unitOfWork.GetGenericRepository<HealthRecord>();
        var healthRecordOfMemberSpecification = new HealthRecordOfMemberSpecification(id);
        var memberToBeDeletedHealthRecord = await healthRecordRepo.FirstOrDefaultAsync(healthRecordOfMemberSpecification, ct: ct);

        if (memberToBeDeletedHealthRecord is not null)
            healthRecordRepo.SoftDelete(memberToBeDeletedHealthRecord);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
        {
            _logger.LogError("Member of Id {@Id} is not deleted due to an internal DB error", id);
            return Result.Failure(MemberBusinessErrors.MemberNotDeleted);
        }

        if (memberToBeDeleted.Photo is not null)
            _attachmentService.DeleteFile(memberToBeDeleted.Photo, "MemberImages");

        _logger.LogInformation("Member of Id {@Id} is deleted successfully", id);
        return Result.Success();
    }
}
