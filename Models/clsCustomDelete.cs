using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Models
{

    public static class clsCustomDelete<T> where T : class
    {
        public static ErpservicesContext db = new ErpservicesContext();

        public static async Task<JsonResult> RemoveByAjax(int id, DbSet<T> dbSet, ErpservicesContext context)
        {
            try
            {
                var obj = await dbSet.FindAsync(id);
                if (obj != null)
                {
                    dbSet.Remove(obj);
                    await context.SaveChangesAsync();
                    return new JsonResult(new { success = true });
                }
                else
                {
                    return new JsonResult(new { success = false, errorMessage = "An error occurred while deleting the item." });
                }
            }
            catch (DbUpdateException ex)
            {
                if (IsForeignKeyViolationError(ex))
                {
                    // Handle the foreign key violation error here
                    var errorMessage = "This record cannot be deleted because it has dependent records";
                    return new JsonResult(new { success = false, errorMessage });
                }
                else
                {
                    // Handle other errors here
                    var errorMessage = "An error occurred while deleting the item.";
                    return new JsonResult(new { success = false, errorMessage });
                }
            }
        }


        private static bool IsForeignKeyViolationError(DbUpdateException ex)
        {
            // Check if the exception message or inner exception message contains specific keywords or patterns that indicate a foreign key violation error
            // You can customize this logic based on your database provider and error messages
            if (ex.Message.Contains("foreign key constraint") || ex.InnerException?.Message?.Contains("foreign key constraint") == true || ex.InnerException.Message.Contains("DELETE statement conflicted with the REFERENCE constraint"))
            {
                return true;
            }

            return false;
        }
    }
}