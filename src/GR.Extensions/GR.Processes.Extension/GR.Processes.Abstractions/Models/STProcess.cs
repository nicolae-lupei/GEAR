﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GR.Audit.Abstractions.Attributes;
using GR.Audit.Abstractions.Enums;
using GR.Core;

namespace GR.Processes.Abstractions.Models
{
    /// <inheritdoc />
    [TrackEntity(Option = TrackEntityOption.AllFields)]
    public class STProcess : BaseModel
    {
        /// <summary>
        /// Name of process
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Reference to schema
        /// </summary>
        public STProcessSchema ProcessSchema { get; set; }
        public Guid ProcessSchemaId { get; set; }

        /// <summary>
        /// Current version of process
        /// </summary>
        [DefaultValue(1)]
        [Required]
        public new int Version { get; set; }

        /// <summary>
        /// Initial Process with version 1
        /// </summary>
        public STProcess IntitialProcess { get; set; }
        public Guid? IntitialProcessId { get; set; }

        /// <summary>
        /// Started instances of this process
        /// </summary>
        public IEnumerable<STProcessInstance> ProcessInstances { get; set; }

        /// <summary>
        /// Defined process transitions
        /// </summary>
        public IList<STProcessTransition> ProcessTransitions { get; set; }
        /// <summary>
        /// Store author settings of process
        /// </summary>
        public string ProcessSettings { get; set; }
    }
}
