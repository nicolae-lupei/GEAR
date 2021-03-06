﻿using System;
using System.ComponentModel;
using GR.Core;

namespace GR.Processes.Abstractions.Models
{
    public class STProcessInstance : BaseModel
    {
        /// <summary>
        /// Get user who was started this instance 
        /// </summary>
        public Guid StartedBy { get; set; }

        /// <summary>
        /// Process for this instance
        /// </summary>
        public STProcess Process { get; set; }

        public Guid ProcessId { get; set; }
        
        /// <summary>
        /// State of this process instance
        /// </summary>
        [DefaultValue(ProcessInstanceState.Inactive)]
        public ProcessInstanceState InstanceState { get; set; }

        /// <summary>
        /// Get progress as percentage of this process instance 
        /// </summary>
        [DefaultValue(0)]
        public double Progress { get; set; }
    }

    public enum ProcessInstanceState
    {
        Active, Completed, Inactive, InProgress
    }
}
