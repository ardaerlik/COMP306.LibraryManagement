using System;
using COMP306.LibraryManagement.COM.Model;
using COMP306.LibraryManagement.DAL.Context;
using COMP306.LibraryManagement.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace COMP306.LibraryManagement.BUS.Service
{
    /// <summary>
    /// 
    /// </summary>
	public class UserService : IUserService
    {
        private readonly ApplicationContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public UserService(ApplicationContext context)
		{
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="sectionId"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
		public async Task<bool> IsAuthorized(string email, int sectionId, int actionId)
		{
            var isAuthorizedUser = await (from user in _context.AppUsers
                                          where user.Email == email
                                            && (from role in user.Roles
                                                where (from permission in role.Permissions
                                                       join action in _context.AppActions on permission.ActionId equals action.Id
                                                       join section in _context.AppSections on permission.SectionId equals section.Id
                                                       where action.Id == actionId && section.Id == sectionId
                                                       select permission).Any()
                                                select role).Any()
                                          select user).AnyAsync();

            return isAuthorizedUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseModel> Register(RegisterModel model)
        {
            try
            {
                var userExist = await (from user in _context.AppUsers
                                       where user.Email == model.email
                                       select user).AnyAsync();

                if (userExist)
                {
                    return new()
                    {
                        HasError = true,
                        ExceptionMessage = $"A user with an email {model.email} exist."
                    };
                }

                var rolesExist = await (from role in _context.AppRoles
                                        where model.roles.Any(t => t == role.Id)
                                        select role).ToListAsync();

                if (!rolesExist.Any())
                {
                    return new()
                    {
                        HasError = true,
                        ExceptionMessage = $"{model.roles.ToString()} are not valid roles."
                    };
                }

                var newUser = new AppUser
                {
                    Name = model.name,
                    Surname = model.surname,
                    Email = model.email,
                    Password = model.password,
                    RegisteredDate = DateTime.Now,
                    Roles = rolesExist
                };

                await _context.AppUsers.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return new()
                {
                    Data = newUser,
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    HasError = true,
                    ExceptionMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseModel> Login(LoginModel model)
        {
            try
            {
                var loginUser = await (from user in _context.AppUsers
                                       where user.Email == model.email
                                        && user.Password == model.password
                                       select user).FirstOrDefaultAsync();

                if (loginUser != null)
                {
                    return new()
                    {
                        Data = loginUser,
                        HasError = false
                    };
                }
                else
                {
                    return new()
                    {
                        HasError = true,
                        ExceptionMessage = $"{model.email} or password is wrong."
                    };
                }
            }
            catch (Exception ex)
            {
                return new()
                {
                    HasError = true,
                    ExceptionMessage = ex.Message
                };
            }
        }
	}

    /// <summary>
    /// 
    /// </summary>
	public interface IUserService
	{
        Task<bool> IsAuthorized(string email, int sectionId, int actionId);
        Task<ResponseModel> Register(RegisterModel model);
        Task<ResponseModel> Login(LoginModel model);
    }
}

