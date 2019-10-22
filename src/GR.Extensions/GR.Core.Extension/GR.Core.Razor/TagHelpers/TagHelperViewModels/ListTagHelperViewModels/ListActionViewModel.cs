﻿using System.Collections.Generic;

namespace GR.Core.Razor.TagHelpers.TagHelperViewModels.ListTagHelperViewModels
{
    public sealed class ListActionViewModel : TagHelperBaseModel
    {
        public bool HasIcon { get; set; }
        public string Icon { get; set; }
        public string ClassStyle { get; set; }
        public BootstrapButton ButtonType { get; set; }
        public string Url { get; set; } = "#";
        public ICollection<ActionParameter> ActionParameters = new List<ActionParameter>();
        public bool IsJsEvent { get; set; }
        public JsActionButtonEvent ButtonEvent { get; set; }
    }

    public sealed class ActionParameter
    {
        public ActionParameter() { }

        public ActionParameter(string name, string value)
        {
            this.ParameterName = name;
            this.ObjectValue = value;
        }

        public string ParameterName { get; set; }
        public bool IsCustomValue { get; set; }
        public string ObjectValue { get; set; }

        public override string ToString()
        {
            if (IsCustomValue) return $"{ParameterName}={ObjectValue}";

            return $"{ParameterName}=${{row.{ObjectValue}}}";
        }
    }

    public sealed class JsActionButtonEvent : TagHelperBaseModel
    {
        /// <summary>
        /// Set js event
        /// </summary>
        public JsEvent JsEvent { get; set; } = JsEvent.OnClick;

        public string GetEvent => JsEvent.ToString().ToLower();

        public string JsEventHandler { get; set; }
    }

    public enum BootstrapButton
    {
        Primary,
        Danger,
        Success,
        Warning,
        Info
    }

    public enum JsEvent
    {
        OnClick,
        OnBlur,
        OnChange,
        OnDoubleClick,
        OnDie,
        OnFocus,
        OnFocusOut,
        OnHover,
        OnReady,
        OnKeyPress,
        OnKeyUp,
        OnMouseDown,
        OnMouseEnter,
        OnMouseLeave,
        OnMouseMove,
        OnMouseOut,
        OnMouseOver,
        OnMouseUp,
        OnResize,
        OnScroll,
        OnSelect,
        OnSubmit
    }
}