using GymManagementSystem.BusinessLogic.Contracts.Services;
using GymManagementSystem.BusinessLogic.DTOs.HealthRecordDTOs;
using GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;
using GymManagementSystem.BusinessLogic.Helpers.BusinessErrors;
using GymManagementSystem.BusinessLogic.Results;
using GymManagementSystem.DataAccess.Enums;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Repositories.Contracts;
using GymManagementSystem.DataAccess.ValueObjects;
using Microsoft.IdentityModel.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Services;

public sealed class MemberService : IMemberService
{
    /* Fields */
    private readonly IGenericRepository<Member> _memberRepo;
    private readonly IGenericRepository<HealthRecord> _healthRepo;
    private readonly IGenericRepository<Plan> _planRepo;
    private readonly IGenericRepository<Membership> _membershipRepo;
    private readonly IGenericRepository<Booking> _bookingRepo;

    /* Constructor */
    public MemberService(IGenericRepository<Member> memberRepo,
        IGenericRepository<Plan> planRepo,
        IGenericRepository<Booking> bookingRepo,
        IGenericRepository<HealthRecord> healthRecordRepo,
        IGenericRepository<Membership> membershipRepo
        )
    {
        _memberRepo = memberRepo;
        _healthRepo = healthRecordRepo;
        _planRepo = planRepo;
        _bookingRepo = bookingRepo;
        _membershipRepo = membershipRepo;
    }

    /* Methods */
    public async Task<IEnumerable<AllMembersDTO>> GetAllMembersAsync(CancellationToken ct = default)
    {
        var allMembers = await _memberRepo.GetAllAsync(ct);

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
        var member = await _memberRepo.GetByIdAsync(id,ct);

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
            Gender = member.Gender
        };

        var latestMemberActiveMembership = await _membershipRepo.FirstOrDefaultAsync(ms => ms.MemberId == id,ct);

        if (latestMemberActiveMembership is not null)
        {
            var latestPlan = await _planRepo.GetByIdAsync(latestMemberActiveMembership.PlanId, ct);
            var latestPlanName = latestPlan!.Name;
            memberDetailsDTO.MembershipStartDate = latestMemberActiveMembership.StartDate;
            memberDetailsDTO.MembershipEndDate = latestMemberActiveMembership.EndDate;
            memberDetailsDTO.PlanName = latestPlanName;
        }

        return Result<MemberDetailsDTO>.Success(memberDetailsDTO);
    }

    public async Task<Result<HealthRecordDTO>> GetMemberHealthRecordDetailsAsync(Guid id, CancellationToken ct = default)
    {
        var healthRecord = await _healthRepo.FirstOrDefaultAsync(hr => hr.MemberId == id,ct);

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
        /* Check if passed Email already exits */
        if(await _memberRepo.AnyAsync(m => m.Email == memberDTO.Email,ct))
            return Result.Failure(MemberBusinessErrors.MemberEmailAlreadyExists);

        /* Check if passed Phone already exits */
        if (await _memberRepo.AnyAsync(m => m.Phone == memberDTO.Phone,ct))
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

        _memberRepo.Add(member);

        var numOfRowsAffected = await _memberRepo.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
            return Result.Failure(MemberBusinessErrors.MemberNotCreated);

         return Result.Success();
    }

    public async Task<Result<MemberToBeEditedDTO>> GetMemberToBeEditedAsync(Guid id, CancellationToken ct = default)
    {
        var memberToBeEdited = await _memberRepo.GetByIdAsync(id, ct);

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
        /* Check if passed Email already exits */
        if (await _memberRepo.AnyAsync(m => m.Email == memberDTO.Email && m.Id != id, ct))
            return Result.Failure(MemberBusinessErrors.MemberEmailAlreadyExists);

        /* Check if passed Phone already exits */
        if (await _memberRepo.AnyAsync(m => m.Phone == memberDTO.Phone && m.Id != id, ct))
            return Result.Failure(MemberBusinessErrors.MemberPhoneNumberAlreadyExists);

        var memberToBeEdited = await _memberRepo.GetByIdAsync(id,ct);

        if(memberToBeEdited is null)
            return Result.Failure(MemberBusinessErrors.MemberToBeEditedNotFound);

        memberToBeEdited.Email = memberDTO.Email;
        memberToBeEdited.Phone = memberDTO.Phone;
        memberToBeEdited.Address.BuildingNumber = memberDTO.BuildingNumber;
        memberToBeEdited.Address.Street = memberDTO.Street;
        memberToBeEdited.Address.City = memberDTO.City;

        _memberRepo.Update(memberToBeEdited);

        var numOfRowsAffected = await _memberRepo.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
            return Result.Failure(MemberBusinessErrors.MemberNotEdited);

        return Result.Success();
    }

    public async Task<Result> DeleteMemberAsync(Guid id, CancellationToken ct = default)
    {
        var memberToBeDeleted = await _memberRepo.GetByIdAsync(id, ct);

        if(memberToBeDeleted is null)
            return Result.Failure(MemberBusinessErrors.MemberToBeDeletedNotFound);

        var memberHasActiveBookings = await _bookingRepo.AnyAsync(b => b.MemberId == id,ct);

        if (memberHasActiveBookings)
            return Result.Failure(MemberBusinessErrors.MemberWithActiveBookingsCannotBeDeleted);

        _memberRepo.SoftDelete(memberToBeDeleted);

        var numOfRowsAffected = await _memberRepo.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
            return Result.Failure(MemberBusinessErrors.MemberNotDeleted);

        return Result.Success();
    }
}
