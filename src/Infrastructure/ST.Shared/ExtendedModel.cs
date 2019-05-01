﻿using System;
using ST.BaseRepository;

namespace ST.Core
{
    public class ExtendedModel : BaseModel
    {
        /// <summary>
        /// Version of data
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Tenant id
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
