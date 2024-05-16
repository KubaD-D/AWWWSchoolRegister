﻿using AutoMapper;
using DAL.EF;
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
    public class StudentService : BaseService, IStudentService
    {
        public StudentService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger)
        {
        }

        public StudentVm GetStudent(Expression<Func<Student, bool>> filterPredicate)
        {
            try
            {
                if(filterPredicate == null)
                {
                    throw new ArgumentNullException("Filter predicate is null");
                }

                var studentEntity = DbContext.Users.OfType<Student>().FirstOrDefault(filterPredicate);
                var studentVm = Mapper.Map<StudentVm>(studentEntity);
                return studentVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<StudentVm> GetStudents(Expression<Func<Student, bool>>? filterPredicate = null)
        {
            try
            {
                var studentEntities = DbContext.Users.OfType<Student>().AsQueryable();
                if (filterPredicate != null)
                {
                    studentEntities = studentEntities.Where(filterPredicate);
                }

                var studentVms = Mapper.Map<IEnumerable<StudentVm>>(studentEntities);
                return studentVms;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
