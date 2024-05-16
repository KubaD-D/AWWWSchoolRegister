using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataModels
{
    public class Student : User
    {
        public virtual Group? Group { get; set; }
        public int GroupId { get; set; }
        public virtual IList<Grade> Grades { get; set; } = new List<Grade>();
        public virtual Parent? Parent { get; set; }
        public int? ParentId { get; set; }
        public double AverageGrade
        {
            get
            {
                double averageGrarde = 0.0;

                foreach (var grade in Grades)
                {
                    averageGrarde += (int)grade.GradeValue;
                }

                return averageGrarde / Grades.Count;
            }
        }
        public IDictionary<string, double> AverageGradePerSubject
        {
            get
            {
                var averageGradePerSubject = new Dictionary<string, double>();

                foreach (var grade in Grades)
                {
                    if (grade.Subject != null && !averageGradePerSubject.ContainsKey(grade.Subject.Name))
                    {
                        var subjectName = grade.Subject.Name;
                        averageGradePerSubject.Add(subjectName, 0.0);

                        foreach (var grade2 in Grades)
                        {
                            double average = 0.0;
                            int numberOfGrades = 0;

                            if (grade2.Subject != null && grade2.Subject.Name == subjectName)
                            {
                                average += (int)grade2.GradeValue;
                                numberOfGrades++;
                            }

                            if (numberOfGrades > 0)
                            {
                                average /= numberOfGrades;
                            }

                            averageGradePerSubject[subjectName] = average;
                        }
                    }
                }

                return averageGradePerSubject;
            }
        }
        public IDictionary<string, List<GradeScale>> GradesPerSubject
        {
            get
            {
                var gradesPerSubject = new Dictionary<string, List<GradeScale>>();

                foreach (var grade in Grades)
                {
                    if (grade.Subject != null)
                    {

                        if (!gradesPerSubject.ContainsKey(grade.Subject.Name))
                        {
                            gradesPerSubject.Add(grade.Subject.Name, new List<GradeScale>());
                        }

                        gradesPerSubject[grade.Subject.Name].Add(grade.GradeValue);
                    }
                }

                return gradesPerSubject;
            }

        }
    }
}
