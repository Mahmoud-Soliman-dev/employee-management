using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        public RoleManager<IdentityRole> _roleManager { get; }
        public UserManager<ApplicationUser> _userManager { get; }

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"The user with id {userId} can not be found";
                return View("_NotFound");
            }

            var existingUserClaims = await _userManager.GetClaimsAsync(user);

            var model = new UserClaimVM
            {
                UserId = userId
            };

            foreach (Claim claim in ClaimStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                if (existingUserClaims.Any(uc => uc.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }

                model.Claims.Add(userClaim);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimVM model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"The user with id {model.UserId} can not be found";
                return View("_NotFound");
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Can not remove existing user claims");
                return View(model);
            }

            var claimsToBeAdded = model.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.ClaimType));

            result = await _userManager.AddClaimsAsync(user, claimsToBeAdded);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Can not add new user claims");
                return View(model);
            }

            return RedirectToAction("EditUser", new { Id = model.UserId });
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleVM model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = model.RoleName
                };

                var roleResult = await _roleManager.CreateAsync(role);
                if (roleResult.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (IdentityError error in roleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"The user with id {id} can not be found";
                return View("_NotFound");
            }

            var model = new EditUserVM
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                City = user.City,
                Roles = (await _userManager.GetRolesAsync(user)).ToList(),
                Claims = (await _userManager.GetClaimsAsync(user)).Select(c => c.Value).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);

                if (user == null)
                {
                    ViewBag.ErrorMessage = $"The role with id {model.Id} can not be found";
                    return View("_NotFound");
                }
                else
                {
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    user.City = model.City;

                    var userResult = await _userManager.UpdateAsync(user);
                    if (userResult.Succeeded)
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }

                    foreach (IdentityError error in userResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"The role with id {id} can not be found";
                return View("_NotFound");
            }

            var model = new EditRoleVM
            {
                Id = role.Id,
                RoleName = role.Name,
                Users = usersInRole.Select(u => u.UserName).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleVM model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);

                if (role == null)
                {
                    ViewBag.ErrorMessage = $"The role with id {model.Id} can not be found";
                    return View("_NotFound");
                }

                role.Name = model.RoleName;

                var roleResult = await _roleManager.UpdateAsync(role);
                if (roleResult.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (IdentityError error in roleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"The role with id {roleId} can not be found";
                return View("_NotFound");
            }

            UserRoleVM model = new UserRoleVM();
            model.RoleId = role.Id;
            model.RoleName = role.Name;

            foreach (var user in _userManager.Users)
            {
                RoleUser userRole = new RoleUser
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                };
                model.Users.Add(userRole);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole (UserRoleVM model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"The role with id {model.RoleId} can not be found";
                return View("_NotFound");
            }

            for (int i = 0; i < model.Users.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model.Users[i].UserId);
                IdentityResult result = null;

                if (model.Users[i].IsSelected && !(await _userManager.IsInRoleAsync(user, model.RoleName)))
                {
                    result = await _userManager.AddToRoleAsync(user, model.RoleName);
                }
                else if (!model.Users[i].IsSelected && await _userManager.IsInRoleAsync(user, model.RoleName))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Users.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { Id = model.RoleId });
                    }
                }
            }
            return RedirectToAction("EditRole", new { Id = model.RoleId });
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"The user with id {userId} can not be found";
                return View("_NotFound");
            }

            ManageUserRolesVM model = new ManageUserRolesVM();
            model.UserId = user.Id;
            model.UserName = user.UserName;

            foreach (var role in _roleManager.Roles)
            {
                UserRole userRole = new UserRole
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                };
                model.Roles.Add(userRole);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesVM model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"The user with id {model.UserId} can not be found";
                return View("_NotFound");
            }

            for (int i = 0; i < model.Roles.Count; i++)
            {
                var role = await _roleManager.FindByIdAsync(model.Roles[i].RoleId);
                IdentityResult result = null;

                if (model.Roles[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model.Roles[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Roles.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditUser", new { Id = model.UserId });
                    }
                }
            }
            return RedirectToAction("EditUser", new { Id = model.UserId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"The user with id {id} can not be found";
                return View("_NotFound");
            }
            else
            {
                var userResult = await _userManager.DeleteAsync(user);

                if (userResult.Succeeded)
                {
                    return RedirectToAction("ListUsers", "Administration");
                }

                return View();
            }
        }

        [HttpPost]
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"The role with id {id} can not be found";
                return View("_NotFound");
            }
            else
            {
                var roleResult = await _roleManager.DeleteAsync(role);

                if (roleResult.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                return View();
            }
        }
    }
}
