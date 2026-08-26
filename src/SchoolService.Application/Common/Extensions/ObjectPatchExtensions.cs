using System.Reflection;

namespace SchoolService.Application.Common.Extensions
{
    /// <summary>
    /// Reflection-based helper for "PATCH-style" updates: copies every non-null property
    /// from a request/DTO onto an entity, matching by property name.
    /// Adding a new updatable field only requires adding the property to both the
    /// request DTO and the entity - no handler code needs to change.
    /// </summary>
    public static class ObjectPatchExtensions
    {
        /// <summary>
        /// Copies each non-null property of <paramref name="source"/> onto <paramref name="destination"/>
        /// when a writable property with the same name and a compatible type exists on the destination.
        /// String values are trimmed before being applied.
        /// </summary>
        /// <returns>true if at least one property was applied.</returns>
        public static bool ApplyNonNullProperties<TSource, TDestination>(this TSource source, TDestination destination)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (destination is null) throw new ArgumentNullException(nameof(destination));

            var destProps = typeof(TDestination)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .ToDictionary(p => p.Name);

            var applied = false;

            foreach (var srcProp in typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = srcProp.GetValue(source);
                if (value is null)
                {
                    continue;
                }

                if (value is string str)
                {
                    value = str.Trim();
                }

                if (!destProps.TryGetValue(srcProp.Name, out var destProp))
                {
                    continue;
                }

                var destType = Nullable.GetUnderlyingType(destProp.PropertyType) ?? destProp.PropertyType;
                if (!destType.IsInstanceOfType(value))
                {
                    continue;
                }

                destProp.SetValue(destination, value);
                applied = true;
            }

            return applied;
        }
    }
}
