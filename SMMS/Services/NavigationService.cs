using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using FirstFloor.ModernUI.Windows.Controls;

namespace SMMS.Services
{
    /// <summary>
    /// The navigation message.
    /// </summary>
    public class NavigationService : IModernNavigationService
    {
        private readonly Dictionary<string, Uri> _ViewsByKey;
        private readonly List<string> _historic;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        public NavigationService()
        {
            _ViewsByKey = new Dictionary<string, Uri>();
            _historic = new List<string>();
        }

        /// <summary>
        /// Gets the key corresponding to the currently displayed page.
        /// </summary>
        /// <value>
        /// The current page key.
        /// </value>
        public string CurrentPageKey
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <value>
        /// The parameter.
        /// </value>
        public object Parameter { get; set; }

        public MainWindow mainWindow
        {
            get; private set;
        }

        /// <summary>
        /// The go back.
        /// </summary>
        public void GoBack()
        {
            if (_historic.Count > 1)
            {
                _historic.RemoveAt(_historic.Count - 1);
                NavigateTo(_historic.Last(), null);
            }
        }

        /// <summary>
        /// The navigate to.
        /// </summary>
        /// <param name="pageKey">
        /// The page key.
        /// </param>
        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        /// <summary>
        /// The navigate to.
        /// </summary>
        /// <param name="pageKey">
        /// The page key.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public virtual void NavigateTo(string pageKey, object parameter)
        {
            lock (_ViewsByKey)
            {
                if (!_ViewsByKey.ContainsKey(pageKey))
                {
                    throw new ArgumentException(string.Format("No such page: {0}. Did you forget to call NavigationService.Configure?", pageKey), "pageKey");
                }

                var frame = GetDescendantFromName(Application.Current.MainWindow, "ContentFrame") as ModernFrame;


                // Set the frame source, which initiates navigation
                if (frame != null)
                {
                    frame.Source = _ViewsByKey[pageKey];
                }
                Parameter = parameter;
                _historic.Add(pageKey);
                CurrentPageKey = pageKey;
            }
        }

        /// <summary>
        /// Configures the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="pageType">Type of the page.</param>
        public void Configure(string key, Uri pageType)
        {
            lock (_ViewsByKey)
            {
                if (_ViewsByKey.ContainsKey(key))
                {
                    _ViewsByKey[key] = pageType;
                }
                else
                {
                    _ViewsByKey.Add(key, pageType);
                }
            }
        }

        /// <summary>
        /// Gets the name of the descendant from.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="name">The name.</param>
        /// <returns>The FrameworkElement.</returns>
        public static FrameworkElement GetDescendantFromName(DependencyObject parent, string name)
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);

            if (count < 1)
            {
                return null;
            }

            for (var i = 0; i < count; i++)
            {
                var frameworkElement = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                if (frameworkElement != null)
                {
                    if (frameworkElement.Name == name)
                    {
                        return frameworkElement;
                    }

                    frameworkElement = GetDescendantFromName(frameworkElement, name);
                    if (frameworkElement != null)
                    {
                        return frameworkElement;
                    }
                }
            }

            return null;
        }
    }
}
