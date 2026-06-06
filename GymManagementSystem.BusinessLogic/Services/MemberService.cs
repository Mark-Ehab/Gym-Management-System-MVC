using GymManagementSystem.BusinessLogic.BusinessSpecifictions;
using GymManagementSystem.BusinessLogic.BusinessSpecifictions.MemberSpecifications;
using GymManagementSystem.BusinessLogic.Contracts.Services;
using GymManagementSystem.BusinessLogic.DTOs.HealthRecordDTOs;
using GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;
using GymManagementSystem.BusinessLogic.Helpers.BusinessErrors;
using GymManagementSystem.BusinessLogic.Results;
using GymManagementSystem.DataAccess.Enums;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Repositories.Contracts;
using GymManagementSystem.DataAccess.UoW.Contract;
using GymManagementSystem.DataAccess.ValueObjects;
using Microsoft.IdentityModel.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Services;

public sealed class MemberService : IMemberService
{
    /* Fields */
    private readonly IUnitOfWork _unitOfWork;

    /* Constructor */
    public MemberService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /* Methods */
    public async Task<IEnumerable<AllMembersDTO>> GetAllMembersAsync(CancellationToken ct = default)
    {
        var memberRepo = _unitOfWork.GetGenericRepository<Member>();

        var allMembers = await memberRepo.GetAllAsync(ct);

        if (allMembers is null)
            return [];

        return allMembers.Select(m => new AllMembersDTO
        {
            Id = m.Id,
            Photo = m.Photo,
            Name = m.Name,
            Email = m.Email,
            Phone = m.Phone,
            Gender = m.Gender
        });
    }

    public async Task<Result<MemberDetailsDTO>> GetMemberDetailsAsync(Guid id, CancellationToken ct = default)
    {
        var memberRepo = _unitOfWork.GetGenericRepository<Member>();
        var memberDetailsWithActiveMembershipSpecification = new MemberDetailsWithActiveMembershipSpecification(id);

        var member = await memberRepo.FirstOrDefaultAsync(ct,memberDetailsWithActiveMembershipSpecification);

        if(member is null)
            return Result<MemberDetailsDTO>.Failure(MemberBusinessErrors.MemberDetailsNotFound);

        var memberDetailsDTO = new MemberDetailsDTO()
        {
            Photo = member.Photo,
            Name = member.Name,
            Email = member.Email,
            Phone = member.Phone,
            DateOfBirth = member.DateOfBirth,
            Address = string.Join(" - ", member.Address.BuildingNumber, member.Address.Street, member.Address.City),
            Gender = member.Gender,
            MembershipStartDate = member.Memberships.FirstOrDefault()?.StartDate,
            MembershipEndDate = member.Memberships.FirstOrDefault()?.EndDate,
            PlanName = member.Memberships.FirstOrDefault()?.Plan.Name
        };

        //var membershipRepo = _unitOfWork.GetGenericRepository<Membership>();

        //var latestMemberActiveMembership = await membershipRepo.FirstOrDefaultAsync(ms => ms.MemberId == id,ct);

        //if (latestMemberActiveMembership is not null)
        //{
        //    var planRepo = _unitOfWork.GetGenericRepository<Plan>();
        //    var latestPlan = await planRepo.GetByIdAsync(latestMemberActiveMembership.PlanId, ct);
        //    var latestPlanName = latestPlan!.Name;
        //    memberDetailsDTO.MembershipStartDate = latestMemberActiveMembership.StartDate;
        //    memberDetailsDTO.MembershipEndDate = latestMemberActiveMembership.EndDate;
        //    memberDetailsDTO.PlanName = latestPlanName;
        //}

        return Result<MemberDetailsDTO>.Success(memberDetailsDTO);
    }

    public async Task<Result<HealthRecordDTO>> GetMemberHealthRecordDetailsAsync(Guid id, CancellationToken ct = default)
    {
        var healthRepo = _unitOfWork.GetGenericRepository<HealthRecord>();
        var memberHealthRecordDetailsSpecification = new MemberHealthRecordDetailsSpecification(id);
        var healthRecord = await healthRepo.FirstOrDefaultAsync(ct, memberHealthRecordDetailsSpecification);

        if(healthRecord is null)
            return Result<HealthRecordDTO>.Failure(MemberBusinessErrors.MemberHealthRecordDetailsNotFound);

        return Result<HealthRecordDTO>.Success(new()
        {
            Height = healthRecord.Height,
            Weight = healthRecord.Weight,
            BloodType = healthRecord.BloodType,
            Note = healthRecord?.Note
        });
    }

    public async Task<Result> AddMemberAsync(MemberCreateDTO memberDTO, CancellationToken ct = default)
    {
        var memberRepo = _unitOfWork.GetGenericRepository<Member>();

        var newMemberWithUniqueEmailSpecification = new NewMemberWithUniqueEmailSpecification(memberDTO.Email);
        var newMemberWithUniquePhoneNumberSpecification = new NewMemberWithUniquePhoneNumberSpecification(memberDTO.Phone);

        /* Check if passed Email already exits */
        if (await memberRepo.AnyAsync(ct,newMemberWithUniqueEmailSpecification))
            return Result.Failure(MemberBusinessErrors.MemberEmailAlreadyExists);

        /* Check if passed Phone already exits */
        if (await memberRepo.AnyAsync(ct,newMemberWithUniquePhoneNumberSpecification))
            return Result.Failure(MemberBusinessErrors.MemberPhoneNumberAlreadyExists);

        var member = new Member()
        {
            Name = memberDTO.Name,
            Email = memberDTO.Email,
            Phone = memberDTO.Phone,
            Gender = memberDTO.Gender,
            DateOfBirth = memberDTO.DateOfBirth,
            Address = new()
            {
                BuildingNumber = memberDTO.BuildingNumber,
                Street = memberDTO.Street,
                City = memberDTO.City
            },
            HealthRecord = new()
            {
                Height = memberDTO.HealthRecord.Height,
                Weight = memberDTO.HealthRecord.Weight,
                BloodType = memberDTO.HealthRecord.BloodType,
                Note = memberDTO.HealthRecord.Note
            }
        };

        memberRepo.Add(member);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
            return Result.Failure(MemberBusinessErrors.MemberNotCreated);

         return Result.Success();
    }

    public async Task<Result<MemberToBeEditedDTO>> GetMemberToBeEditedAsync(Guid id, CancellationToken ct = default)
    {
        var memberRepo = _unitOfWork.GetGenericRepository<Member>();

        var memberToBeEdited = await memberRepo.GetByIdAsync(id, ct);

        if (memberToBeEdited is null)
            return Result<MemberToBeEditedDTO>.Failure(MemberBusinessErrors.MemberNotEdited);

        return Result<MemberToBeEditedDTO>.Success(new()
        {
            Photo = memberToBeEdited.Photo,
            Name = memberToBeEdited.Name,
            Email = memberToBeEdited.Email,
            Phone = memberToBeEdited.Phone,
            BuildingNumber = memberToBeEdited.Address.BuildingNumber,
            Street = memberToBeEdited.Address.Street,
            City = memberToBeEdited.Address.City
        });
    }

    public async Task<Result> EditMemberAsync(Guid id, MemberToBeEditedDTO memberDTO, CancellationToken ct = default)
    {
        var memberRepo = _unitOfWork.GetGenericRepository<Member>();
        var editMemberWithUniqueEmailSpecification = new EditMemberWithUniqueEmailSpecification(id, memberDTO.Email);
        var editMemberWithUniquePhoneNumberSpecification = new EditMemberWithUniquePhoneNumberSpecification(id, memberDTO.Phone);

        /* Check if passed Email already exits */
        if (await memberRepo.AnyAsync(ct, editMemberWithUniqueEmailSpecification))
            return Result.Failure(MemberBusinessErrors.MemberEmailAlreadyExists);

        /* Check if passed Phone already exits */
        if (await memberRepo.AnyAsync(ct, editMemberWithUniquePhoneNumberSpecification))
            return Result.Failure(MemberBusinessErrors.MemberPhoneNumberAlreadyExists);

        var memberToBeEdited = await memberRepo.GetByIdAsync(id,ct);

        if(memberToBeEdited is null)
            return Result.Failure(MemberBusinessErrors.MemberToBeEditedNotFound);

        memberToBeEdited.Email = memberDTO.Email;
        memberToBeEdited.Phone = memberDTO.Phone;
        memberToBeEdited.Address.BuildingNumber = memberDTO.BuildingNumber;
        memberToBeEdited.Address.Street = memberDTO.Street;
        memberToBeEdited.Address.City = memberDTO.City;

        memberRepo.Update(memberToBeEdited);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
            return Result.Failure(MemberBusinessErrors.MemberNotEdited);

        return Result.Success();
    }

    public async Task<Result> DeleteMemberAsync(Guid id, CancellationToken ct = default)
    {
        var memberRepo = _unitOfWork.GetGenericRepository<Member>();

        var memberToBeDeleted = await memberRepo.GetByIdAsync(id, ct);

        if (memberToBeDeleted is null)
            return Result.Failure(MemberBusinessErrors.MemberToBeDeletedNotFound);

        var bookingRepo = _unitOfWork.GetGenericRepository<Booking>();
        var memberActiveCurrentOrFutureBookingsSpecification = new MemberActiveCurrentOrFutureBookingsSpecification(id);

        var memberHasActiveCurrentOrFutureBookings = await bookingRepo.AnyAsync(ct,memberActiveCurrentOrFutureBookingsSpecification);

        if (memberHasActiveCurrentOrFutureBookings)
            return Result.Failure(MemberBusinessErrors.MemberWithActiveCurrentOrFutureBookingsCannotBeDeleted);

        memberRepo.SoftDelete(memberToBeDeleted);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
            return Result.Failure(MemberBusinessErrors.MemberNotDeleted);

        return Result.Success();
    }
}
