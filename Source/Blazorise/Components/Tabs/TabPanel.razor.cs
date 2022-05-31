﻿#region Using directives
using System;
using System.Threading.Tasks;
using Blazorise.States;
using Blazorise.Utilities;
using Microsoft.AspNetCore.Components;
#endregion

namespace Blazorise
{
    /// <summary>
    /// A container for each <see cref="Tab"/> inside of <see cref="Tabs"/> component.
    /// </summary>
    public partial class TabPanel : BaseComponent, IAnimatedComponent, IDisposable
    {
        #region Members

        /// <summary>
        /// Tracks whether the component fulfills the requirements to be lazy loaded and then kept rendered to the DOM.
        /// </summary>
        private bool lazyLoaded;

        /// <summary>
        /// A reference to the parent tabs state.
        /// </summary>
        private TabsState parentTabsState;

        /// <summary>
        /// A reference to the parent tabs content state.
        /// </summary>
        private TabsContentState parentTabsContentState;

        /// <summary>
        /// Manages the visibility states.
        /// </summary>
        private CloseableAdapter closeableAdapter;

        #endregion
        #region constructors

        /// <inheritdoc/>
        public TabPanel()
        {
            closeableAdapter = new( this );
        }


        #endregion

        #region Methods

        /// <inheritdoc/>
        protected override void BuildClasses( ClassBuilder builder )
        {
            builder.Append( ClassProvider.TabPanel() );
            builder.Append( ClassProvider.TabPanelActive( Active ) );
            builder.Append( ClassProvider.Fade(  ) );

            base.BuildClasses( builder );
        }

        /// <inheritdoc/>
        protected override void OnInitialized()
        {
            ParentTabs?.NotifyTabPanelInitialized( Name );

            ParentTabsContent?.NotifyTabPanelInitialized( Name );

            base.OnInitialized();
        }

        /// inheritdoc
        public Task BeginAnimation( bool visible )
        {
            if ( visible )
                DirtyStyles();
            else
                DirtyClasses();

            return InvokeAsync( StateHasChanged );
        }

        /// inheritdoc
        public Task EndAnimation( bool visible )
        {
            if ( visible )
                DirtyClasses();
            else
                DirtyStyles();

            return InvokeAsync( StateHasChanged );
        }

        /// <inheritdoc/>
        protected override Task OnParametersSetAsync()
        {
            if ( Active )
                lazyLoaded = ( RenderMode == TabsRenderMode.LazyLoad );
            return base.OnParametersSetAsync();
        }

        /// <inheritdoc/>
        protected override void Dispose( bool disposing )
        {
            if ( disposing )
            {
                ParentTabs?.NotifyTabPanelRemoved( Name );

                ParentTabsContent?.NotifyTabPanelRemoved( Name );
            }

            base.Dispose( disposing );
        }

        public Task Show()
        {
            return Task.CompletedTask;
        }

        public Task Hide()
        {
            return Task.CompletedTask;
        }

        #endregion

        #region Properties

        /// <summary>
        /// True if this panel is currently set as selected.
        /// </summary>
        protected bool Active => ParentTabsState?.SelectedTab == Name || ParentTabsContentState?.SelectedPanel == Name;

        /// <summary>
        /// Gets the current render mode.
        /// </summary>
        protected TabsRenderMode RenderMode => ParentTabsState?.RenderMode ?? TabsRenderMode.Default;

        /// <summary>
        /// Defines the panel name. Must match the corresponding tab name.
        /// </summary>
        [Parameter] public string Name { get; set; }

        /// <summary>
        /// Cascaded parent <see cref="Tabs"/> state.
        /// </summary>
        [CascadingParameter]
        protected TabsState ParentTabsState
        {
            get => parentTabsState;
            set
            {
                if ( parentTabsState == value )
                    return;

                parentTabsState = value;

                DirtyClasses();
            }
        }

        /// <summary>
        /// Cascaded parent <see cref="TabsContent"/> state.
        /// </summary>
        [CascadingParameter]
        protected TabsContentState ParentTabsContentState
        {
            get => parentTabsContentState;
            set
            {
                if ( parentTabsContentState == value )
                    return;

                parentTabsContentState = value;

                DirtyClasses();
            }
        }

        /// <summary>
        /// Cascaded parent <see cref="Tabs"/> component.
        /// </summary>
        [CascadingParameter] protected Tabs ParentTabs { get; set; }

        /// <summary>
        /// Cascaded parent <see cref="TabsContent"/> component.
        /// </summary>
        [CascadingParameter] protected TabsContent ParentTabsContent { get; set; }

        /// <summary>
        /// Specifies the content to be rendered inside this <see cref="TabPanel"/>.
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; }
        /// <inheritdoc/>
        [Parameter] public bool Animated { get; set; } = true;

        /// <inheritdoc/>
        [Parameter] public int AnimationDuration { get; set; } = 150;

        #endregion
    }
}
