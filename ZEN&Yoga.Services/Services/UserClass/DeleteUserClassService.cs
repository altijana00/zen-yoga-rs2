using Microsoft.EntityFrameworkCore;
using ZEN_Yoga.Models;
using ZEN_Yoga.Services.Interfaces.Base;

namespace ZEN_Yoga.Services.Services.UserClass
{
    public class DeleteUserClassService : IDeleteService
    {
        private readonly ZenYogaDbContext _dbContext;

        public DeleteUserClassService(ZenYogaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Delete(int id)
        {
            var classRes = await _dbContext.UserClasses.FirstOrDefaultAsync(uc => uc.Id == id);

            if (classRes != null)
            {


                _dbContext.UserClasses.Remove(classRes);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
