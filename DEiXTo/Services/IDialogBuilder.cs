﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public interface IDialogBuilder
    {
        void Build(ISaveFileDialog dialog);
    }
}
