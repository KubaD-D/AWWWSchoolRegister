using AutoMapper;
using Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.VM;

namespace Services.Configuration.AutoMapperProfiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<Subject, SubjectVm>()
                .ForMember(dest => dest.TeacherName,
                           x => x.MapFrom(src => src.Teacher == null ? null : $"{src.Teacher.FirstName} {src.Teacher.LastName}"))
                .ForMember(dest => dest.Groups, x => x.MapFrom(src => src.SubjectGroups.Select(y => y.Group)));

            CreateMap<AddOrUpdateSubjectVm, Subject>();

            CreateMap<Group, GroupVm>()
                .ForMember(dest => dest.Students, x => x.MapFrom(src => src.Students))
                .ForMember(dest => dest.Subjects, x => x.MapFrom(src => src.SubjectGroups.Select(s => s.Subject)));

            CreateMap<SubjectVm, AddOrUpdateSubjectVm>().ReverseMap();

            CreateMap<Student, StudentVm>()
                .ForMember(dest => dest.GroupName, x => x.MapFrom(src => src.Group == null ? null : src.Group.Name))
                .ForMember(dest => dest.ParentName,
                           x => x.MapFrom(src => src.Parent == null ? null : $"{src.Parent.FirstName} {src.Parent.LastName}"));

            CreateMap<Teacher, TeacherVm>()
                .ForMember(dest => dest.Name, x => x.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<Grade, GradeVm>();

            CreateMap<AddOrUpdateGroupVm, Group>();

            CreateMap<AddOrUpdateGroupVm, GroupVm>();

            CreateMap<AttachDetachSubjectGroupVm, SubjectGroup>();
        }
    }
}
