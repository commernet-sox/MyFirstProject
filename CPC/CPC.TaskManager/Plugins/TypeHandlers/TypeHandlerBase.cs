using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CPC.TaskManager.Plugins.TypeHandlers
{
    public abstract class TypeHandlerBase
    {
        private string _name;

        /// <summary>
        /// Type Discriminator
        /// </summary>
        public string TypeId => GetTypeId(GetType());

        internal static string GetTypeId(Type type) => type.FullName;

        /// <summary>
        /// Unique name across <see cref="ViewOptions.StandardTypes"/>
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                DisplayName = DisplayName ?? _name;
            }
        }

        public string DisplayName { get; set; }

        public virtual string RenderView(Services services, object value) => services.TypeHandlers.Render(this, new
        {
            Value = value,
            StringValue = ConvertToString(value),
            TypeHandler = this
        });

        public virtual object ConvertFrom(Dictionary<string, object> formData) => ConvertFrom(formData?.Values?.FirstOrDefault());

        public abstract bool CanHandle(object value);

        /// <summary>
        /// If the value is expected type, just return the value. Every implementation should support conversion from String.
        /// </summary>
        public abstract object ConvertFrom(object value);

        /// <summary>
        /// Most of TypeHandlers support conversion from invariant string. Implement this method such as another TypeHandler can easily convert from this string.
        /// </summary>
        public virtual string ConvertToString(object value) => string.Format(CultureInfo.InvariantCulture, "{0}", value);

        public override string ToString() => DisplayName;

        public virtual bool IsValid(object value) => value != null;

        public override bool Equals(object obj) => Name.Equals((obj as TypeHandlerBase)?.Name);

        public override int GetHashCode() => Name.GetHashCode();
    }
}
