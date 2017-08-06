﻿using System.Collections;
using System.Collections.Generic;

namespace App.BLL.DynamicLinq
{
    internal class GroupResult
    {
        public object Key { get; set; }
        public int Count { get; set; }
        public IEnumerable Items { get; set; }
        public IEnumerable<GroupResult> SubGroups { get; set; }
        public override string ToString() { return string.Format("{0} ({1})", Key, Count); }
    }
}