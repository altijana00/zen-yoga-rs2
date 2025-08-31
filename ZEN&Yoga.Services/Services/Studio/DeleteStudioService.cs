using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEN_Yoga.Models;
using ZEN_Yoga.Services.Interfaces.Base;

namespace ZEN_Yoga.Services.Services.Studio
{
    public class DeleteStudioService : IDeleteService
    {
        private readonly ZenYogaDbContext _dbContext;

        public DeleteStudioService(ZenYogaDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }
        public async Task<bool> Delete(int id)
        {
            var studio = await _dbContext.Studios.FirstOrDefaultAsync(s => s.Id == id);
            var classes = await _dbContext.Classes.Where(c => c.StudioId == id).ToListAsync();
            var instructors = await _dbContext.Instructors.Where(i => i.StudioId == id).ToListAsync();


            if (studio != null)
            {
                foreach (var c in classes)
                {
                    _dbContext.Classes.Remove(c);
                }

                foreach (var i in instructors)
                {

                    _dbContext.Instructors.Remove(i);
                }

                _dbContext.Remove(studio);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
