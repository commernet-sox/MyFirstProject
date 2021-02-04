﻿using CPC.TaskManager.Plugins.TypeHandlers;
using System.Collections.Generic;
using Number = CPC.TaskManager.Plugins.TypeHandlers.NumberHandler.UnderlyingType;

namespace CPC.TaskManager.Plugins
{
    public class ViewOptions
    {
        /// <summary>
        /// Supports any value that is a viable as a img src attribute value: url, or base64
        /// src='data:image/jpeg;base64, LzlqLzRBQ...[end of base64 data]'
        /// Defaults to the original logo
        /// </summary>
        public string Logo { get; set; } = "Content/Images/logo.png";

        public string ProductName { get; set; } = "";

        public string VirtualPathRoot { get; set; } = "/";

        /// <summary>
        /// Supported value types in job data map.
        /// </summary>
        public List<TypeHandlerBase> StandardTypes { get; } = new List<TypeHandlerBase>();

        /// <summary>
        /// Default type for new job data map item.
        /// </summary>
        public TypeHandlerBase DefaultSelectedType { get; set; }

        public string DefaultDateFormat
        {
            get => DateTimeSettings.DefaultDateFormat;
            set => DateTimeSettings.DefaultDateFormat = value;
        }

        public string DefaultTimeFormat
        {
            get => DateTimeSettings.DefaultTimeFormat;
            set => DateTimeSettings.DefaultTimeFormat = value;
        }

        public ViewOptions()
        {
            DefaultSelectedType = new StringHandler() { Name = "String" };

            // order of StandardTypes is important due to TypeHandlerBase.CanHandle evaluation
            StandardTypes.Add(new FileHandler() { Name = "File", DisplayName = "Binary Data" });
            StandardTypes.Add(new BooleanHandler() { Name = "Boolean" });
            StandardTypes.Add(new DateTimeHandler() { Name = "Date", DisplayName = "Date", IgnoreTimeComponent = true });
            StandardTypes.Add(new DateTimeHandler() { Name = "DateTime" });
            StandardTypes.Add(new DateTimeHandler() { Name = "DateTimeUtc", DisplayName = "DateTime (UTC)", IsUtc = true });
            StandardTypes.Add(new NumberHandler(Number.Decimal));
            StandardTypes.Add(new NumberHandler(Number.Double));
            StandardTypes.Add(new NumberHandler(Number.Float));
            StandardTypes.Add(new NumberHandler(Number.Integer));
            StandardTypes.Add(new NumberHandler(Number.Long));
            StandardTypes.Add(DefaultSelectedType); // String
            StandardTypes.Add(new StringHandler() { Name = "MultilineString", DisplayName = "String (Multiline)", IsMultiline = true });
        }

        internal string ContentRootDirectory => null;
        internal string ViewsRootDirectory => null;
    }
}