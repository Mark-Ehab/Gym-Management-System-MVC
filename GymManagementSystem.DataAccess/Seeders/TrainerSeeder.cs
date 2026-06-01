using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Enums;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Seeders;

public static class TrainerSeeder
{
    public async static Task SeedTrainer(GymDbContext context)
    {
        var trainerDbSet = context.Trainers;
        if (await trainerDbSet.AnyAsync())
            return;

        var trainers = new List<Trainer>
        {
            new()
            {
                Name = "Mohamed Salah",
                Email = "m.salah@gympro.com",
                Phone = "01011223344",
                Gender = Gender.Male,
                DateOfBirth = new DateOnly(1988, 6, 15),
                HireDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-5)),
                Speciality = Speciality.GeneralFitness,
                Address = new Address
                {
                    BuildingNumber = "15",
                    Street = "El Hegaz Street",
                    City = "Cairo"
                }
            },

            new()
            {
                Name = "Nourhan Ahmed",
                Email = "n.ahmed@gympro.com",
                Phone = "01122334455",
                Gender = Gender.Female,
                DateOfBirth = new DateOnly(1993, 4, 20),
                HireDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-3)),
                Speciality = Speciality.Yoga,
                Address = new Address
                {
                    BuildingNumber = "42",
                    Street = "Tahrir Street",
                    City = "Giza"
                }
            },

            new()
            {
                Name = "Karim Mostafa",
                Email = "k.mostafa@gympro.com",
                Phone = "01233445566",
                Gender = Gender.Male,
                DateOfBirth = new DateOnly(1990, 11, 8),
                HireDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-4)),
                Speciality = Speciality.Nutrition,
                Address = new Address
                {
                    BuildingNumber = "8",
                    Street = "Sea Road",
                    City = "Alexandria"
                }
            },

            new()
            {
                Name = "Mariam Fathy",
                Email = "m.fathy@gympro.com",
                Phone = "01544556677",
                Gender = Gender.Female,
                DateOfBirth = new DateOnly(1995, 2, 14),
                HireDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-2)),
                Speciality = Speciality.GeneralFitness,
                Address = new Address
                {
                    BuildingNumber = "21",
                    Street = "University Street",
                    City = "Mansoura"
                }
            },

            new()
            {
                Name = "Ahmed Sherif",
                Email = "a.sherif@gympro.com",
                Phone = "01055667788",
                Gender = Gender.Male,
                DateOfBirth = new DateOnly(1987, 9, 25),
                HireDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-6)),
                Speciality = Speciality.Boxing,
                Address = new Address
                {
                    BuildingNumber = "5",
                    Street = "Military Road",
                    City = "Tanta"
                }
            }
        };

        trainerDbSet.AddRange(trainers);
        await context.SaveChangesAsync();
    }
}
