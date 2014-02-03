﻿// Copyright © 2007 by Initial Force AS.  All rights reserved.
// https://github.com/InitialForce/UnitsNet
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Linq;

#if NETFX_CORE
using System.Reflection;
#endif

namespace UnitsNet.Extensions
{
    /// <summary>
    /// Source: http://codereview.stackexchange.com/a/5354/32101
    /// </summary>
    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            return GetAttributes<TAttribute>(value).FirstOrDefault();
        }

        public static TAttribute[] GetAttributes<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            // Below code not compatible with WinRT.
            var type = value.GetType();
            var name = Enum.GetName(type, value);
#if NETFX_CORE
            return type.GetRuntimeField(name) // I prefer to get attributes this way
#else
            return type.GetField(name) // I prefer to get attributes this way
#endif
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .ToArray()
                ;
        }
        public static TAttribute GetAttribute<TAttribute>(this Enum value, Type attributeType) where TAttribute : Attribute
        {
            // Below code not compatible with WinRT.
            var type = value.GetType();
            var name = Enum.GetName(type, value);
#if NETFX_CORE
            return type.GetRuntimeField(name) // I prefer to get attributes this way
#else
            return type.GetField(name) // I prefer to get attributes this way
#endif
                .GetCustomAttributes(false)
                .Where(type2 => type2.GetType()
#if NETFX_CORE
                    .GetTypeInfo().IsAssignableFrom(attributeType.GetTypeInfo()))
#else
                    .IsAssignableFrom(attributeType))
#endif
                .Select(type2 => (TAttribute)type2)
                .SingleOrDefault();
        }
    }
}