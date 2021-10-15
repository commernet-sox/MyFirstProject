using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyCoreTest
{
    public partial class TestApi : BaseEntity
    {
        [MaxLength(30)]
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
