﻿using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using System;
using System.Reflection;

namespace FluentSerializer.Core.Context
{
    /// <summary>
    /// Current context for serializing data
    /// </summary>
    public interface ISerializerContext
    {
        /// <summary>
        /// The original propertyInfo used to define this mapping
        /// </summary>
        PropertyInfo Property { get; }
        /// <summary>
        /// The actual type of the property with resolved generics based on the current <see cref="ClassType"/>
        /// <br /><b>
        /// If the <see cref="Property"/> is of type <see cref="Nullable{T}"/> this will return the value of
        /// <see cref="Nullable.GetUnderlyingType"/>
        /// </b>
        /// </summary>
        Type PropertyType { get; }
        /// <summary>
        /// The type of the class(on the current level on nesting) passed to the serializer
        /// <br /><b>
        /// If the current class type is of type <see cref="Nullable{T}"/> this will return the value of
        /// <see cref="Nullable.GetUnderlyingType"/>
        /// </b>
        /// </summary>
        Type ClassType { get; }

        /// <summary>
        /// The <see cref="INamingStrategy"/> passed for the property currently being (de)serialized
        /// </summary>
        INamingStrategy NamingStrategy { get; }

        /// <summary>
        /// The current serializer being used
        /// </summary>
        ISerializer CurrentSerializer { get; }

        /// <summary>
        /// Find the <see cref="INamingStrategy"/>  for any property if registered
        /// This can be useful when unpacking collections to a different data structure
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        INamingStrategy? FindNamingStrategy(PropertyInfo property);

        /// <summary>
        /// Find the <see cref="INamingStrategy"/>  for any <see cref="Type"/> if registered
        /// This can be useful when unpacking collections to a different data structure
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        INamingStrategy? FindNamingStrategy(Type type);
    }
}
