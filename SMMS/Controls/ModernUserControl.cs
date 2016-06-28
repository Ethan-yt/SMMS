using System.Diagnostics;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;

namespace SMMS.Controls
{
    /// <summary>
    /// Class ModernUserControl.
    /// </summary>
    public class ModernUserControl : UserControl, IContent
    {
        /// <summary>
        /// Handles the <see cref="E:FragmentNavigation"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs"/> instance containing the event data.</param>
        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            FragmentNavigation?.Invoke(this, e);
        }

        /// <summary>
        /// Handles the <see cref="E:NavigatedFrom"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        public void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigatedFrom?.Invoke(this, e);
        }

        /// <summary>
        /// Handles the <see cref="E:NavigatedTo"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        public void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigatedTo?.Invoke(this, e);
        }

        /// <summary>
        /// Handles the <see cref="E:NavigatingFrom"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs"/> instance containing the event data.</param>
        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            NavigatingFrom?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when [navigating from].
        /// </summary>
        public event NavigatingCancelHandler NavigatingFrom;

        /// <summary>
        /// Occurs when [navigated from].
        /// </summary>
        public event NavigationEventHandler NavigatedFrom;

        /// <summary>
        /// Occurs when [navigated to].
        /// </summary>
        public event NavigationEventHandler NavigatedTo;

        /// <summary>
        /// Occurs when [fragment navigation].
        /// </summary>
        public event FragmentNavigationHandler FragmentNavigation;
    }

    /// <summary>
    /// Delegate NavigatingCancelHandler.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs"/> instance containing the event data.</param>
    public delegate void NavigatingCancelHandler(object sender, NavigatingCancelEventArgs e);

    /// <summary>
    /// Delegate NavigationEventHandler.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
    public delegate void NavigationEventHandler(object sender, NavigationEventArgs e);

    /// <summary>
    /// Delegate FragmentNavigationHandler.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs"/> instance containing the event data.</param>
    public delegate void FragmentNavigationHandler(object sender, FragmentNavigationEventArgs e);
}
