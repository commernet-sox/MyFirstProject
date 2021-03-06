﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SDT.BaseTool
{
    public class EngineSettings
    {
        public string[] DependencyAssemblies { get; set; }

        public string[] MapperAssemblies { get; set; }

        public string[] AutoMapperAssemblies { get; set; }

        public string[] AutoDependencyAssemblies { get; set; }
    }
}
