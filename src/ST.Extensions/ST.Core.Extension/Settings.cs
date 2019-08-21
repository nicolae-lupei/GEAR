﻿using System;

namespace ST.Core
{
    public struct Settings
    {
        /// <summary>
        /// Default tenant id
        /// </summary>
        public static Guid TenantId = Guid.Parse("d11eeb3d-9545-4f1a-a199-632257326765");

        /// <summary>
        /// Default language
        /// </summary>
        public const string DEFAULT_LANGUAGE = "English";

        /// <summary>
        /// Super admin role
        /// </summary>
        public const string ADMINISTRATOR = "Administrator";

        /// <summary>
        /// Anonimous User role
        /// </summary>
        public const string ANONIMOUS_USER = "Anonymous User";

        /// <summary>
        /// Entity default schema
        /// </summary>
        public const string DEFAULT_ENTITY_SCHEMA = "system";

        /// <summary>
        /// Access denied message
        /// </summary>
        public const string ACCESS_DENIED_MESSAGE = "Access denied";

        public struct Tables
        {
            public const string CustomTable = "CustomTableName";
        }

        public static class Date
        {
            public static string DateFormat = "dd'.'MM'.'yyyy";
        }
    }

    public sealed class SystemConfig
    {
        /// <summary>
        /// This property value is used for cookie name and redis reserved key
        /// </summary>
        public string MachineIdentifier { get; set; }
    }
}
