﻿#region Using directives
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
#endregion

namespace Blazorise
{
    /// <summary>
    /// Basic type for all <see cref="SelectItem{TValue}"/> components.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface ISelectItem<TValue>
    {
        /// <summary>
        /// Gets or sets the item value.
        /// </summary>
        TValue Value { get; set; }

        /// <summary>
        /// Gets or sets the item render fragment.
        /// </summary>
        RenderFragment ChildContent { get; set; }
    }

    /// <summary>
    /// Option item in the <see cref="Select{TValue}"/> component.
    /// </summary>
    /// <typeparam name="TValue">The type of the <see cref="Value"/>.</typeparam>
    public partial class SelectItem<TValue> : BaseComponent, ISelectItem<TValue>, IDisposable
    {
        #region Methods

        /// <inheritdoc/>
        protected override Task OnInitializedAsync()
        {
            ParentSelect?.NotifySelectItemInitialized( this );

            return base.OnInitializedAsync();
        }

        /// <inheritdoc/>
        protected override void Dispose( bool disposing )
        {
            if ( disposing )
            {
                DisposeResources();
            }

            base.Dispose( disposing );
        }

        /// <inheritdoc/>
        protected override ValueTask DisposeAsync( bool disposing )
        {
            if ( disposing )
            {
                DisposeResources();
            }

            return base.DisposeAsync( disposing );
        }

        private void DisposeResources()
        {
            ParentSelect?.NotifySelectItemRemoved( this );
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the flag that indicates if item is selected.
        /// </summary>
        protected bool Selected => ParentSelect?.ContainsValue( Value ) == true;

        /// <summary>
        /// Convert the value to string because option tags are working with string internally. Otherwise some datatypes like booleans will not work as expected.
        /// </summary>
        protected string StringValue => Value?.ToString();

        /// <summary>
        /// Gets or sets the item value.
        /// </summary>
        [Parameter] public TValue Value { get; set; }

        /// <summary>
        /// Disable the item from mouse click.
        /// </summary>
        [Parameter] public bool Disabled { get; set; }

        /// <summary>
        /// Hide the item from the list so it can be used as a placeholder.
        /// </summary>
        [Parameter] public bool Hidden { get; set; }

        /// <summary>
        /// Specifies the select component in which this select item is placed.
        /// </summary>
        [CascadingParameter] protected virtual Select<TValue> ParentSelect { get; set; }

        /// <summary>
        /// Specifies the content to be rendered inside this <see cref="SelectItem{TValue}"/>.
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; }

        #endregion
    }
}
