using GymManagementSystem.BusinessLogic.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Helpers.BusinessErrors;
public static class TrainerBusinessErrors
{
    public static Error TrainerEmailAlreadyExists
    => new("Trainer.Email.AlreadyExists", "Email already exists on the system !");
    public static Error TrainerPhoneNumberAlreadyExists
        => new("Trainer.PhoneNumber.AlreadyExists", "Phone number already exists on the system !");
    public static Error TrainerDetailsNotFound
        => new("Trainer.Details.NotFound", "Trainer details are not found on the system !");
    public static Error TrainerToBeEditedNotFound
        => new("Trainer.Edit.NotFound", "Trainer you are trying to edit is not found on the system !");
    public static Error TrainerNotCreated
        => new("Trainer.Create.NotCreated", "Trainer is not created due to an internal error !");
    public static Error TrainerNotEdited
        => new("Trainer.Edit.NotEdited", "Trainer is not edited due to an internal error !");
    public static Error TrainerNotDeleted
        => new("Trainer.Delete.NotDeleted", "Trainer is not deleted due to an internal error !");
    public static Error TrainerToBeDeletedNotFound
        => new("Trainer.Delete.NotFound", "Trainer you are trying to delete is not found on the system !");
    public static Error TrainerWithScheduledSessionsCannotBeDeleted
        => new("Trainer.Delete.ScheduledSessions.CannotDelete", "This trainer is already assigned to scheduled sessions on the system and cannot be deleted !");
}
