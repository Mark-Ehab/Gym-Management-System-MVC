using GymManagementSystem.DataAccess.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Seeders;

public static class IdentitySeeder
{
    public async static Task SeedIdentityAsync(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IConfiguration config)
    {
        // Create a list of required identity roles that shall exist in database
        var requiredIdentityRoles = new List<IdentityRole<Guid>>()
        {
            new()
            {
                Name = "SuperAdmin"
            },
            new()
            {
                Name = "admin"
            }
        };

        // Create required identity roles in database if not exist
        await CreateRolesIfNotExist(roleManager, requiredIdentityRoles);

        // Create a list of required identity users that shall exist in database
        var superAdminEmail = config.GetSection("IdentitySeeders:SuperAdminEmail").Value ?? throw new InvalidOperationException("SuperAdmin email is not found !");
        var superAdminPassword = config.GetSection("IdentitySeeders:SuperAdminPassword").Value ?? throw new InvalidOperationException("SuperAdmin password is not found !");
        var adminEmail = config.GetSection("IdentitySeeders:AdminEmail").Value ?? throw new InvalidOperationException("Admin email is not found !");
        var adminPassword = config.GetSection("IdentitySeeders:AdminPassword").Value ?? throw new InvalidOperationException("Admin password is not found !");

        var requiredIdentityUsers = new List<(ApplicationUser,string)>()
        {
            (new()
            {
                UserName = "SuperAdmin",
                FullName = "Super Administrator",
                Email = superAdminEmail
            },superAdminPassword),
            (new()
            {
                UserName = "Admin",
                FullName = "Administrator",
                Email = adminEmail
            },adminPassword)
        };

        // Create required identity users in database if not exist
        await CreateUsersIfNotExist(userManager, requiredIdentityUsers);

        // Assign roles to users if not
        for(int count = 0;  count < requiredIdentityUsers.Count; count++)
        {
            await AssignRoleToUserIfNot(userManager,requiredIdentityUsers[count].Item1, requiredIdentityRoles[count]);
        }
    }

    private static async Task CreateUsersIfNotExist(UserManager<ApplicationUser> userManager, IEnumerable<(ApplicationUser,string)> requiredIdentityUsers)
    {
        // Check if any identity user exists on database
        if (!await userManager.Users.AnyAsync())
        {
            // Create users in the database
            foreach (var requiredUser in requiredIdentityUsers)
            {
                // Create the role in database
                var userCreationResult = await userManager.CreateAsync(requiredUser.Item1,requiredUser.Item2);

                // Check if role creation is done successfully
                if (!userCreationResult.Succeeded)
                    throw new InvalidOperationException($"{requiredUser.Item1.FullName} user is not created successfully due to an internal error !");
            }

            return;
        }

        if (await userManager.Users.AnyAsync())
        {
            // Check that all required identity users exist in database and create them if not
            foreach (var requiredUser in requiredIdentityUsers)
            {
                if (await userManager.FindByEmailAsync(requiredUser.Item1.Email!) is null)
                {
                    // Create the role in database
                    var userCreationResult = await userManager.CreateAsync(requiredUser.Item1, requiredUser.Item2);

                    // Check if role creation is done successfully
                    if (!userCreationResult.Succeeded)
                        throw new InvalidOperationException($"{requiredUser.Item1.FullName} user is not created successfully due to an internal error !");
                }
            }

            return;
        }
    }

    private static async Task CreateRolesIfNotExist(RoleManager<IdentityRole<Guid>> roleManager, IEnumerable<IdentityRole<Guid>> requiredIdentityRoles)
    {
        // Check if any identity role exists on database
        if (!await roleManager.Roles.AnyAsync())
        {
            // Create roles in the database
            foreach (var requiredRole in requiredIdentityRoles)
            {
                // Create the role in database
                var roleCreationResult = await roleManager.CreateAsync(requiredRole);

                // Check if role creation is done successfully
                if (!roleCreationResult.Succeeded)
                    throw new InvalidOperationException($"{requiredRole.Name} role is not created successfully due to an internal error !");
            }

            return;
        }

        if (await roleManager.Roles.AnyAsync())
        {
            // Check that all required identity roles exist in database and create them if not
            foreach (var requiredRole in requiredIdentityRoles)
            {
                // Check if role exists
                if (!await roleManager.RoleExistsAsync(requiredRole.Name!))
                {
                    // Create the role in database
                    var roleCreationResult = await roleManager.CreateAsync(requiredRole);

                    // Check if role creation is done successfully
                    if (!roleCreationResult.Succeeded)
                        throw new InvalidOperationException($"{requiredRole.Name} role is not created successfully due to an internal error !");
                }
            }

            return;
        }
    }
    private static async Task AssignRoleToUserIfNot(UserManager<ApplicationUser> userManager, ApplicationUser applicationUser, IdentityRole<Guid> identityRole)
    {
        // Check if role not assigned to user
        var user = await userManager.FindByEmailAsync(applicationUser.Email!);

        if (user is null)
            throw new InvalidOperationException("User was not found in the database.");

        if (!await userManager.IsInRoleAsync(user, identityRole.Name!))
        {
            // Assign the role to the user
            var roleAssignmentResult = await userManager.AddToRoleAsync(applicationUser,identityRole.Name!);

            // Check if role is assigned successfully to user
            if(!roleAssignmentResult.Succeeded)
                throw new InvalidOperationException($"{applicationUser.FullName} user is not assgined to {identityRole.Name} role successfully due to an internal error !");
        }
    }
}
