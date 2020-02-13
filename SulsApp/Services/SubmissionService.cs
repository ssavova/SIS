using SulsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SulsApp.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly SulsDbContext db;
        public SubmissionService(SulsDbContext db)
        {
            this.db = db;
        }

        public void CreateSubmission(string userId, string problemId, string code)
        {
            var problemPoints = this.db.Problems.FirstOrDefault(p => p.Id == problemId).Points;

            Submission submission = new Submission()
            {
                CreatedOn = DateTime.UtcNow,
                UserId = userId,
                ProblemId = problemId,
                Code = code,
                AchievedResult = new Random().Next(0,problemPoints +1),
            };

            this.db.Submissions.Add(submission);
            db.SaveChanges();
        }

        public void Delete(string submissionId)
        {
            var searchedSubmission = this.db.Submissions.FirstOrDefault(s => s.Id == submissionId);
            this.db.Submissions.Remove(searchedSubmission);
            this.db.SaveChanges();
        }
    }
}
