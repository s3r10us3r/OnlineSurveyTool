﻿using Microsoft.EntityFrameworkCore;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class SurveyRepo : BaseRepo<Survey>, ISurveyRepo
    {
        public SurveyRepo(OSTDbContext dbContext) : base(dbContext)
        {
        }

        public new async Task<Survey?> GetOne(int id)
        {
            var survey = await Table.FindAsync(id);
            if (survey is not null && !survey.IsArchived)
            {
                return survey;
            }
            return null;
        }

        public new async Task<Survey?> GetOne(string token)
        {
            var survey = await Table.FirstOrDefaultAsync(s => s.Token == token);
            if (survey is not null && !survey.IsArchived)
            {
                return survey;
            }
            return null;

        }

        public new async Task<List<Survey>> GetAll()
        {
            return await Table.Where(s => !s.IsArchived).ToListAsync();
        }

        public new async Task<List<Survey>> GetOpen(int ownerId)
        {
            return await Table.Where(s => s.OwnerId == ownerId && !s.IsArchived && s.OwnerId == ownerId).ToListAsync();
        }
    }
}
