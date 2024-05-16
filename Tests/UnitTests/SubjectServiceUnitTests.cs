using DAL.EF;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.VM;

namespace Tests.UnitTests
{
    public class SubjectServiceUnitTests : BaseUnitTests
    {
        private readonly ISubjectService _subjectService;
        public SubjectServiceUnitTests(ApplicationDbContext dbContext, ISubjectService subjectService) : base(dbContext)
        {
            _subjectService = subjectService;
        }

        [Fact]
        public void GetSubject()
        {
            var subject = _subjectService.GetSubject(x => x.Name == "Programowanie obiektowe");
            Assert.NotNull(subject);
        }

        [Fact]
        public void GetSubjects()
        {
            var subjects = _subjectService.GetSubjects(x => x.Id > 2 && x.Id <= 4).ToList();

            Assert.NotNull(subjects);
            Assert.NotEmpty(subjects);
            Assert.Equal(2, subjects.Count);
        }

        [Fact]
        public void GetAllSubjects()
        {
            var subjects = _subjectService.GetSubjects().ToList();

            Assert.NotNull(subjects);
            Assert.NotEmpty(subjects);
            Assert.Equal(DbContext.Subjects.Count(), subjects.Count);
        }

        [Fact]
        public void AddNewSubject()
        {
            var newSubjectVm = new AddOrUpdateSubjectVm
            {
                Name = "Zaawansowane programowanie internetowe",
                Description = "W ramach przedmiotu studenci tworzą rozwiązania w bibliotekach SPA",
                TeacherId = 1
            };

            var createdSubject = _subjectService.AddOrUpdateSubject(newSubjectVm);
            Assert.NotNull(createdSubject);
            Assert.Equal("Zaawansowane programowanie internetowe", createdSubject.Name);
        }
    }
}
