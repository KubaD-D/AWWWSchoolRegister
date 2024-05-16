using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.VM
{
    public class SubjectVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IList<GroupVm>? Groups { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public int? TeacherId { get; set; }

    }
}
