﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataModels
{
    public class Teacher : User
    {
        public virtual IList<Subject> Subjects { get; set; } = new List<Subject>();
        public string Title { get; set; } = string.Empty;
    }
}
