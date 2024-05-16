using AutoMapper;
using DAL.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Model.DataModels;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.VM;

namespace Services.ConcreteServices
{
    public class GradeService : BaseService, IGradeService
    {
        private readonly UserManager<User> _userManager;
        public GradeService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager)
            : base(dbContext, mapper, logger)
        {
            _userManager = userManager;
        }

        public GradeVm AddGradeToStudent(AddGradeToStudentVm addGradeToStudentVm)
        {
            try
            {
                var newGrade = new Grade
                {
                    DateOfIssue = DateTime.Now,
                    GradeValue = addGradeToStudentVm.GradeValue,
                    SubjectId = addGradeToStudentVm.SubjectId,
                    StudentId = addGradeToStudentVm.StudentId
                };

                DbContext.Grades.Add(newGrade);
                DbContext.SaveChanges();
                var gradeVm = Mapper.Map<GradeVm>(newGrade);
                return gradeVm;
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public GradesReportVm GetGradesReportForStudent(GetGradesReportVm getGradesVm)
        {
            try
            {
                var getterUser = DbContext.Users.FirstOrDefault(u => u.Id == getGradesVm.GetterUserId);

                if(getGradesVm.StudentId == getGradesVm.GetterUserId
                    || getterUser is Teacher
                    || getterUser is Parent && ((Parent)getterUser).Students.Any(s => s.Id == getGradesVm.StudentId))
                {
                    var gradeEntities = DbContext.Grades.Where(g => g.StudentId == getGradesVm.StudentId).ToList();
                    var gradeVms = Mapper.Map<IEnumerable<GradeVm>>(gradeEntities);

                    return new GradesReportVm
                    {
                        Grades = gradeVms
                    };

                }
                else
                {
                    throw new UnauthorizedAccessException("You are not allowed to view grades");
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
