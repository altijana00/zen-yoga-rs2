using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEN_Yoga.Models;
using ZEN_Yoga.Models.Responses;
using ZEN_Yoga.Services.Interfaces.UserClass;

namespace ZEN_Yoga.Services.Services.UserClass
{
    public class GetUserClassService : IGetUserClassService
    {
        private readonly ZenYogaDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetUserClassService(ZenYogaDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<List<UserClassesResponse>> GetAll()
        {
            var classes = await _dbContext.UserClasses
                .Include(uc => uc.Class)
                .Include(uc => uc.User)
                .Select(uc => new UserClassesResponse
                {
                    Id = uc.Id,
                    UserId = uc.UserId,
                    ClassId = uc.ClassId,
                    StudioId = uc.Class.StudioId,
                    Name = uc.Class.Name,
                    StartDate = uc.Class.StartDate,
                    EndDate = uc.Class.EndDate,
                    MaxParticipants = uc.Class.MaxParticipants,
                    InstructorId = uc.Class.InstructorId,
                    YogaTypeId = uc.Class.YogaTypeId,
                    Description = uc.Class.Description,
                    Location = uc.Class.Location,
                    JoinedAt = uc.JoinedAt

                }).ToListAsync();

            return _mapper.Map<List<UserClassesResponse>>(classes);
        }

        public async Task<UserClassesResponse> GetById(int id)
        {
            var userClasses = await GetAll();

            return userClasses.FirstOrDefault(uc => uc.Id == id);
        }
    }
}
