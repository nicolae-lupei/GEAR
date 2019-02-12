﻿using System.Collections.Generic;

namespace ST.Entities.ViewModels.Form
{
    public class OptionsViewModel
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
        public ICollection<AttrTagViewModel> Type { get; set; }
        public ICollection<AttrTagViewModel> ClassName { get; set; }
    }
}