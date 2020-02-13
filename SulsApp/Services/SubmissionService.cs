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
        private readonly Random random;
        public SubmissionService(SulsDbContext db, Random random)
        {
            this.db = db;
            this.random = random;
        }

        public void Create(string userId, string problemId, string code)
        {
            var problem = this.db.Problems.FirstOrDefault(p => p.Id == problemId);

            Submission submission = new Submission()
            {
                CreatedOn = DateTime.UtcNow,
                UserId = userId,
                ProblemId = problemId,
                Code = code,
                AchievedResult = random.Next(0,problem.Points + 1),
            };

            this.db.Submissions.Add(submission);
            this.db.SaveChanges();
        }

        public void Delete(string submissionId)
        {
            var searchedSubmission = this.db.Submissions.FirstOrDefault(s => s.Id == submissionId);
            this.db.Submissions.Remove(searchedSubmission);
            this.db.SaveChanges();
        }
    }
}
