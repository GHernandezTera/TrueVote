using Microsoft.AspNetCore.Identity;
using TrueVote.Models;
using TrueVote.Utilities;

namespace TrueVote.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RoleService> _logger;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IConfiguration configuration, ILogger<RoleService> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task CreateRolesAsync()
        {
            var roles = _configuration.GetSection("Identity:Roles").Get<string[]>();

            if (roles == null || !roles.Any())
            {
                _logger.LogWarning("No roles found in configuration.");
                return;
            }

            foreach (var role in roles)
            {
                try
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        var creationRes = await _roleManager.CreateAsync(new IdentityRole(role));
                        if (creationRes.Succeeded)
                        {
                            _logger.LogInformation($"Role '{role}' created successfully.");
                        }
                        else
                        {
                            _logger.LogError($"Failed to create role '{role}': {string.Join(", ", creationRes.Errors.Select(e => e.Description))}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while creating role '{role}'.");
                }
            }
        }


        public async Task CreateAdminUserAsync()
        {
            var adminEmail = _configuration["Identity:Email"];
            var adminPassword = _configuration["Identity:Password"];
            var adminName = _configuration["Identity:Name"];
            var adminLastName = _configuration["Identity:LastName"];
            var adminRole = _configuration["Identity:Role"];

            var existingAdmin = await _userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin != null)
            {
                _logger.LogInformation("The admin user already exists in the database.");
                return;
            }

            if (!await _roleManager.RoleExistsAsync(adminRole))
            {
                _logger.LogError($"The role '{adminRole}' does not exist.");
                return;
            }

            var adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                Name = adminName,
                LastName = adminLastName
            };

            var creationRes = await _userManager.CreateAsync(adminUser, adminPassword);
            if (!creationRes.Succeeded)
            {
                var errorResponse = IdentityErrorHandler.HandleIdentityErrors(creationRes);
                _logger.LogError(errorResponse.ToString());
                return;
            }

            var roleResult = await _userManager.AddToRoleAsync(adminUser, adminRole);
            if (!roleResult.Succeeded)
            {
                var errorResponse = IdentityErrorHandler.HandleIdentityErrors(roleResult);
                _logger.LogError(errorResponse.ToString());
            }
            else
            {
                _logger.LogInformation($"User successfully created and assigned role '{adminRole}'.");
            }
        }
    }
}
