using Model.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.VM
{
    public class GroupVm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public IList<Student>? Students { get; set; }
        public IList<Subject>? Subjects { get; set; }
    }
}
