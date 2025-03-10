using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static ERP.Models.clsEnum;


namespace ERP.Models
{
    public  class ViewModel
    {

        private readonly ErpservicesContext _context;
        int year, month, daysInMonth;
        DateTime firstDayOfTheYear, lastDayOfTheYear, firstDayOfTheMonth, lastDayOfTheMonth;
        // Constructor injection of CommitteesAndCouncilsContext
        public ViewModel(ErpservicesContext context)
        {
            _context = context;
             year = DateTime.Now.Year;
             month = DateTime.Now.Month;
            // First day of the current year
            firstDayOfTheYear = new DateTime(year, 1, 1);
            // Last day of the current year
            lastDayOfTheYear = new DateTime(year, 12, 31);
            // First day of the current month
            firstDayOfTheMonth = new DateTime(year, month, 1);
            // Last day of the current month
            daysInMonth = DateTime.DaysInMonth(year, month);
            lastDayOfTheMonth = new DateTime(year, month, daysInMonth);
        }



        public  async Task<string> UploadImageFile(IFormFile file, string uploadingPath)
        {
            try
            {
                string filePath = string.Empty;
                string url = string.Empty;
                if (file.Length > 0)
                {
                    string fileName = file.FileName;
                    string extension = Path.GetExtension(fileName);
                    string dynamicFileName = Guid.NewGuid().ToString() + extension;

                    if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                    {
                        filePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), uploadingPath));
                        using (var fileStream = new FileStream(Path.Combine(filePath, dynamicFileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        url = uploadingPath.Replace("wwwroot/", "/") + "/" + dynamicFileName;
                    }
                    else
                    {
                        throw new Exception("File must be either .png , .jpg , jpeg");
                    }
                }
                return url;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public  async Task<string> UploadAttachment(IFormFile file, string uploadingPath)
        {
            try
            {
                string filePath = string.Empty;
                string url = string.Empty;
                if (file.Length > 0)
                {
                    string fileName = file.FileName;
                    string extension = Path.GetExtension(fileName);
                    string dynamicFileName = Guid.NewGuid().ToString() + extension;

                    filePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), uploadingPath));
                    using (var fileStream = new FileStream(Path.Combine(filePath, dynamicFileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    url = uploadingPath.Replace("wwwroot/", "/") + "/" + dynamicFileName;
                }
                return url;
            }
            catch (Exception)
            {
                throw;
            }
        }
		public  async Task<string> GetAbsoluteUriAsync(HttpContext httpContext)
		{
			var request = httpContext.Request;
			return await Task.FromResult(request.GetDisplayUrl());
		}
		public  async Task<List<IdentityUser>> GetUsersInRoleAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, string roleName)
		{
			if (string.IsNullOrWhiteSpace(roleName) || !await roleManager.RoleExistsAsync(roleName))
			{
				throw new ArgumentException("Invalid role name.", nameof(roleName));
			}

			var usersInRole = new List<IdentityUser>();
			var users = userManager.Users.ToList();

			foreach (var user in users)
			{
				if (await userManager.IsInRoleAsync(user, roleName))
				{
					usersInRole.Add(user);
				}
			}

			return usersInRole;
		}
	
      
		public  List<string> GetErrorListFromModelState(ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;
            return query.ToList();
        } 
       
       

     
      
        private string GenerateRandomCode(int length)
        {
            var random = new Random();
            return random.Next((int)Math.Pow(10, length - 1), (int)Math.Pow(10, length)).ToString();
        }
        public string GenerateUniqueToken<T>(DbSet<T> table) where T : class
        {
            string token;
            do
            {
                token = GenerateRandomCode(6); // Generate a 6-digit random code
            } while (table.Any(x => EF.Property<string>(x, "Token") == token)); // Ensure uniqueness

            return token;
        }
    }
}
