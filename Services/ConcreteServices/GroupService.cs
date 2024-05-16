using AutoMapper;
using DAL.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.DataModels;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ViewModels.VM;

namespace Services.ConcreteServices
{
    public class GroupService : BaseService, IGroupService
    {
        private readonly UserManager<User> _userManager;
        public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager)
            : base(dbContext, mapper, logger)
        {
            _userManager = userManager;
        }

        public GroupVm AddOrUpdateGroup(AddOrUpdateGroupVm addOrUpdateGroupVm)
        {
            try
            {
                if(addOrUpdateGroupVm == null)
                {
                    throw new ArgumentNullException(nameof(addOrUpdateGroupVm));
                }

                var newGroup = Mapper.Map<Group>(addOrUpdateGroupVm);

                if(!addOrUpdateGroupVm.Id.HasValue || addOrUpdateGroupVm.Id == 0)
                {
                    DbContext.Groups.Add(newGroup);
                }
                else
                {
                    DbContext.Groups.Update(newGroup);
                }

                DbContext.SaveChanges();

                var groupVm = Mapper.Map<GroupVm>(addOrUpdateGroupVm);
                return groupVm;
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public StudentVm AttachStudentToGroup(AttachDetachStudentToGroupVm attachStudentToGroupVm)
        {
            try
            {
                if(attachStudentToGroupVm == null)
                {
                    throw new ArgumentNullException();
                }

                var studentEntity = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == attachStudentToGroupVm.StudentId);
                var groupEntity = DbContext.Groups.FirstOrDefault(g => g.Id == attachStudentToGroupVm.GroupId);

                if(groupEntity == null || studentEntity == null)
                {
                    throw new NullReferenceException();
                }

                if(groupEntity.Students.Any(s => s.Id == attachStudentToGroupVm.StudentId))
                {
                    throw new Exception("Student is already assigned to a group");
                }

                groupEntity.Students.Add(studentEntity);
                DbContext.SaveChanges();

                var studentVm = Mapper.Map<StudentVm>(studentEntity);
                return studentVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public GroupVm AttachSubjectToGroup(AttachDetachSubjectGroupVm attachSubjectToGroupVm)
        {
            try
            {
                if(attachSubjectToGroupVm == null)
                {
                    throw new ArgumentNullException();
                }

                var subjectGroup = Mapper.Map<SubjectGroup>(attachSubjectToGroupVm);
                var groupEntity = DbContext.Groups.Include(g => g.SubjectGroups).FirstOrDefault(g => g.Id == attachSubjectToGroupVm.GroupId);
                var subjectEntity = DbContext.Subjects.FirstOrDefault(s => s.Id == attachSubjectToGroupVm.SubjectId);

                if(subjectGroup == null || groupEntity == null || subjectEntity == null)
                {
                    throw new NullReferenceException();
                }

                subjectGroup.Subject = subjectEntity;
                groupEntity.SubjectGroups.Add(subjectGroup);
                DbContext.SaveChanges();

                var groupVm = Mapper.Map<GroupVm>(groupEntity);
                return groupVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public SubjectVm AttachTeacherToSubject(AttachDetachSubjectToTeacherVm attachSubjectToTeacherVm)
        {
            try
            {
                if(attachSubjectToTeacherVm == null)
                {
                    throw new ArgumentNullException();
                }

                var subjectEntity = DbContext.Subjects.FirstOrDefault(s => s.Id == attachSubjectToTeacherVm.SubjectId);
                var teacherEntity = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == attachSubjectToTeacherVm.TeacherId);

                if(subjectEntity == null || teacherEntity == null)
                {
                    throw new NullReferenceException();
                }

                subjectEntity.Teacher = teacherEntity;
                DbContext.SaveChanges();

                var subjectVm = Mapper.Map<SubjectVm>(subjectEntity);
                return subjectVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public StudentVm DetachStudentFromGroup(AttachDetachStudentToGroupVm detachStudentFromGroupVm)
        {
            try
            {
                if (detachStudentFromGroupVm == null)
                {
                    throw new ArgumentNullException();
                }

                var studentEntity = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == detachStudentFromGroupVm.StudentId);
                var groupEntity = DbContext.Groups.FirstOrDefault(g => g.Id == detachStudentFromGroupVm.GroupId);

                if (groupEntity == null || studentEntity == null)
                {
                    throw new NullReferenceException();
                }

                if (!groupEntity.Students.Any(s => s.Id == detachStudentFromGroupVm.StudentId))
                {
                    throw new Exception("Student is already assigned to a group");
                }

                groupEntity.Students.Remove(studentEntity);
                studentEntity.Group = null;
                
                DbContext.SaveChanges();

                var studentVm = Mapper.Map<StudentVm>(studentEntity);
                return studentVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public GroupVm DetachSubjectFromGroup(AttachDetachSubjectGroupVm detachSubjectFromGroupVm)
        {
            try
            {
                if (detachSubjectFromGroupVm == null)
                {
                    throw new ArgumentNullException();
                }

                var subjectGroup = DbContext.SubjectGroups.FirstOrDefault(sg => sg.SubjectId == detachSubjectFromGroupVm.SubjectId);
                var groupEntity = DbContext.Groups.Include(g => g.SubjectGroups).FirstOrDefault(g => g.Id == detachSubjectFromGroupVm.GroupId);

                if (subjectGroup == null || groupEntity == null)
                {
                    throw new NullReferenceException();
                }

                groupEntity.SubjectGroups.Remove(subjectGroup);
                DbContext.SaveChanges();

                var groupVm = Mapper.Map<GroupVm>(groupEntity);
                return groupVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        
        public SubjectVm DetachTeacherFromSubject(AttachDetachSubjectToTeacherVm detachSubjectToTeacherVm)
        {
            try
            {
                if (detachSubjectToTeacherVm == null)
                {
                    throw new ArgumentNullException();
                }

                var subjectEntity = DbContext.Subjects.Include(s => s.Teacher).FirstOrDefault(s => s.Id == detachSubjectToTeacherVm.SubjectId);
                var teacherEntity = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == detachSubjectToTeacherVm.TeacherId);

                if (subjectEntity == null || teacherEntity == null)
                {
                    throw new NullReferenceException();
                }

                if(subjectEntity.Teacher != teacherEntity)
                {
                    throw new Exception("Teacher does not teach this subject");
                }

                subjectEntity.Teacher = null;
                //subjectEntity.TeacherId = null;
                DbContext.SaveChanges();

                var subjectVm = Mapper.Map<SubjectVm>(subjectEntity);
                return subjectVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public GroupVm GetGroup(Expression<Func<Group, bool>> filterPredicate)
        {
            try
            {
                if (filterPredicate == null)
                {
                    throw new ArgumentNullException("FilterExpression is null");
                }

                var groupEntity = DbContext.Groups.FirstOrDefault(filterPredicate);
                var groupVm = Mapper.Map<GroupVm>(groupEntity);
                return groupVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<GroupVm> GetGroups(Expression<Func<Group, bool>>? filterPredicate = null)
        {
            try
            {
                var groupEntities = DbContext.Groups.AsQueryable();

                if (filterPredicate != null)
                {
                    groupEntities = groupEntities.Where(filterPredicate);
                }

                var groupVms = Mapper.Map<IEnumerable<GroupVm>>(groupEntities);
                return groupVms;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
