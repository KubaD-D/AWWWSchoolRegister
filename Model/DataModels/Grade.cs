using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataModels
{
    public class Grade
    {
        public int Id { get; set; }
        public DateTime DateOfIssue { get; set; }
        public GradeScale GradeValue { get; set; }
        public virtual Subject? Subject { get; set; }
        public int SubjectId { get; set; }
        public virtual Student? Student { get; set; }
        public int StudentId { get; set; }
    }
}
