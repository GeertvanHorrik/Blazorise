﻿#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using Blazorise.Localization;
#endregion

namespace Blazorise.DataGrid
{
    /// <summary>
    /// Helper extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Gets the next available direction based on the current one.
        /// </summary>
        /// <param name="direction">Current sort direction.</param>
        /// <returns>Returns the next available sort direction.</returns>
        public static SortDirection NextDirection( this SortDirection direction )
        {
            switch ( direction )
            {
                case SortDirection.None:
                    return SortDirection.Ascending;
                case SortDirection.Ascending:
                    return SortDirection.Descending;
                default:
                    return SortDirection.None;
            }
        }

        /// <summary>
        /// Handles the localization of datagrid based on the built-int localizer and a custom localizer handler.
        /// </summary>
        /// <param name="textLocalizer">Default localizer.</param>
        /// <param name="textLocalizerHandler">Custom localizer.</param>
        /// <param name="name">Localization name.</param>
        /// <param name="arguments">Arguments to format the text.</param>
        /// <returns>Returns the localized text.</returns>
        public static string Localize( this ITextLocalizer textLocalizer, TextLocalizerHandler textLocalizerHandler, string name, params object[] arguments )
        {
            if ( textLocalizerHandler != null )
                return textLocalizerHandler.Invoke( name, arguments );

            return textLocalizer[name, arguments];
        }


        /// <summary>
        /// Checks if a type is a collection.
        /// This is a generalized method, checking for various Collection, Array, IEnumerable derivatives.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>True if <paramref name="type"/> is a collection.</returns>
        public static bool IsCollection( this Type type )
            => typeof( ICollection ).IsAssignableFrom( type )
                || type.IsGenericIEnumerable()
                || Array.Exists( type.GetInterfaces(), IsGenericIEnumerable )
                && type != typeof(string);

        private static bool IsGenericICollection( this Type type )
            => type.IsGenericType
                && type.GetGenericTypeDefinition() == typeof( ICollection<> );

        private static bool IsGenericIEnumerable( this Type type )
            => type.IsGenericType
                && type.GetGenericTypeDefinition() == typeof( IEnumerable<> );
    }
}