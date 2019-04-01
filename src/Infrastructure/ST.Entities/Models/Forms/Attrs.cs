﻿
using System;
using ST.Audit.Models;

namespace ST.Entities.Models.Forms
{
    public class Attrs : ExtendedModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public Row Row { get; set; }
        public Guid? RowId { get; set; }
        public Field Field { get; set; }
        public Guid? FieldId { get; set; }
        public AttrValueType Type { get; set; } = AttrValueType.String;
    }

    public enum AttrValueType
    {
        String, Object, ObjectList
    }
}