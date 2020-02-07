using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LearningProcess.MVVM
{
    public class MkThemeSelector : DependencyObject
    {
        public static readonly DependencyProperty CurrentThemeDictionaryProperty =
         DependencyProperty.RegisterAttached("CurrentThemeDictionary", typeof(Uri),
         typeof(MkThemeSelector),
         new UIPropertyMetadata(null, CurrentThemeDictionaryChanged));

        public static Uri GetCurrentThemeDictionary(DependencyObject obj)
        {
            return (Uri)obj.GetValue(CurrentThemeDictionaryProperty);
        }

        public static void SetCurrentThemeDictionary(DependencyObject obj, Uri uri)
        {
            if (uri == default(Uri))
            { Application.Current.Resources.Clear(); }
            else
            { Application.Current.Resources.Source = uri; }
        }

        private static void CurrentThemeDictionaryChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is FrameworkElement) // works only on FrameworkElement objects
            {
                ApplyTheme(obj as FrameworkElement, GetCurrentThemeDictionary(obj));
            }
        }

        private static void ApplyTheme(FrameworkElement targetElement, Uri dictionaryUri)
        {
            if (targetElement == null) return;

            try
            {
                ThemeResourceDictionary themeDictionary = null;
                if (dictionaryUri != null)
                {
                    themeDictionary = new ThemeResourceDictionary();
                    themeDictionary.Source = dictionaryUri;

                    // add the new dictionary to the collection of merged dictionaries of the target object
                    targetElement.Resources.MergedDictionaries.Insert(0, themeDictionary);
                }

                // find if the target element already has a theme applied
                IList existingDictionaries =
                    (from dictionary in targetElement.Resources.MergedDictionaries
                     select dictionary).ToList();

                // remove the existing dictionaries
                foreach (ThemeResourceDictionary thDictionary in existingDictionaries)
                {
                    if (themeDictionary == thDictionary) continue;  // don't remove the newly added dictionary
                    targetElement.Resources.MergedDictionaries.Remove(thDictionary);
                }
            }
            finally { }
        }
    }

    public class ThemeResourceDictionary : ResourceDictionary
    {
    }
}