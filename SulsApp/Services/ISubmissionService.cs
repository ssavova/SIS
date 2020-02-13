using System;
using System.Collections.Generic;
using System.Text;

namespace SulsApp.Services
{
    public interface ISubmissionService
    {
        void Create(string userId,string problemId, string code);

        void Delete(string submissionId);
    }
}
