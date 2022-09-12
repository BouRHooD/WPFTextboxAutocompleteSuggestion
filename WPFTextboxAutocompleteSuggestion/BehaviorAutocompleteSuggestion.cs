using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFTextboxAutocompleteSuggestion
{
    public class BehaviorAutocompleteSuggestion : DependencyObject
    {
        private static TextChangedEventHandler _OnTextChanged = new TextChangedEventHandler(OnTextChanged);
        private static KeyEventHandler _OnKeyDown = new KeyEventHandler(OnPreviewKeyDown);

        /// <summary>
        /// Коллекция для поиска совпадений <br/>
        /// -------------------------------------------- <br/>
        /// The collection to search for matches from
        /// </summary>
        public static readonly DependencyProperty AutoCompleteItemsSourceProperty =
            DependencyProperty.RegisterAttached
            (
                "AutoCompleteItemsSource",
                typeof(IEnumerable<String>),
                typeof(BehaviorAutocompleteSuggestion),
                new UIPropertyMetadata(null, OnAutoCompleteItemsSource)
            );

        public bool AutoCompleteItemsSource
        {
            set { SetValue(AutoCompleteItemsSourceProperty, value); }
            get { return (bool)GetValue(AutoCompleteItemsSourceProperty); }
        }

        /// <summary>
        /// Следует ли игнорировать регистр при поиске совпадений <br/>
        /// ---------------------------------------------------------------- <br/>
        /// Whether or not to ignore case when searching for matches
        /// </summary>
        public static readonly DependencyProperty AutoCompleteStringComparisonProperty =
            DependencyProperty.RegisterAttached
            (
                "AutoCompleteStringComparison",
                typeof(StringComparison),
                typeof(BehaviorAutocompleteSuggestion),
                new UIPropertyMetadata(StringComparison.Ordinal)
            );

        public bool AutoCompleteStringComparison
        {
            set { SetValue(AutoCompleteStringComparisonProperty, value); }
            get { return (bool)GetValue(AutoCompleteStringComparisonProperty); }
        }

        /// <summary>
        /// Какая строка должна указывать на то, что мы должны начать давать предложения автозаполнения. Например: @ <br/>
        /// Если это значение равно null или пусто, предложения автозавершения будут начинаться с начала текста текстового поля <br/>
        /// --------------------------------------------------------------------------------------------------------------------------------------- <br/>
        /// What string should indicate that we should start giving auto-completion suggestions.  For example: @ <br/>
        /// If this is null or empty, auto-completion suggestions will begin at the beginning of the textbox's text
        /// </summary>
        public static readonly DependencyProperty AutoCompleteIndicatorProperty =
            DependencyProperty.RegisterAttached
            (
                "AutoCompleteIndicator",
                typeof(String),
                typeof(BehaviorAutocompleteSuggestion),
                new UIPropertyMetadata(String.Empty)
            );

        public bool AutoCompleteIndicator
        {
            set { SetValue(AutoCompleteIndicatorProperty, value); }
            get { return (bool)GetValue(AutoCompleteIndicatorProperty); }
        }

        #region Items Source
        public static IEnumerable<String> GetAutoCompleteItemsSource(DependencyObject obj)
        {
            object objRtn = obj.GetValue(AutoCompleteItemsSourceProperty);
            if (objRtn is IEnumerable<String>) { return (objRtn as IEnumerable<String>); }
            return null;
        }

        public static void SetAutoCompleteItemsSource(DependencyObject obj, IEnumerable<String> value)
        {
            obj.SetValue(AutoCompleteItemsSourceProperty, value);
        }

        private static void OnAutoCompleteItemsSource(object sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (sender == null || tb == null) { return; }

            // Если завершили выполнение, то удаляем подписку на события (If we're being removed, remove the callbacks)
            // Удалите наш старый обработчик, независимо от того, есть ли у нас новый (Remove our old handler, regardless of if we have a new one)
            tb.TextChanged -= _OnTextChanged;
            tb.PreviewKeyDown -= _OnKeyDown;
            if (e.NewValue != null)
            {
                // Подписываемся на события (New source. Add the callbacks)
                tb.TextChanged += _OnTextChanged;
                tb.PreviewKeyDown += _OnKeyDown;
            }
        }
        #endregion

        #region String Comparison
        public static StringComparison GetAutoCompleteStringComparison(DependencyObject obj)
        {
            return (StringComparison)obj.GetValue(AutoCompleteStringComparisonProperty);
        }

        public static void SetAutoCompleteStringComparison(DependencyObject obj, StringComparison value)
        {
            obj.SetValue(AutoCompleteStringComparisonProperty, value);
        }
        #endregion

        #region Indicator
        public static String GetAutoCompleteIndicator(DependencyObject obj)
        {
            return (String)obj.GetValue(AutoCompleteIndicatorProperty);
        }

        public static void SetAutoCompleteIndicator(DependencyObject obj, String value)
        {
            obj.SetValue(AutoCompleteIndicatorProperty, value);
        }
        #endregion

        /// <summary>
        /// Используется для перемещения курсора в конец предлагаемого текста автодополнения <br/>
        /// -------------------------------------------------------------------------------------------------- <br/>
        /// Used for moving the caret to the end of the suggested auto-completion text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) { return; }

            TextBox tb = e.OriginalSource as TextBox;
            if (tb == null) { return; }

            // Если мы нажали ввод и если выделенный текст дошел до конца, перемещаем нашу позицию курсора в конец
            // (If we pressed enter and if the selected text goes all the way to the end, move our caret position to the end)
            if (tb.SelectionLength > 0 && (tb.SelectionStart + tb.SelectionLength == tb.Text.Length))
            {
                tb.SelectionStart = tb.CaretIndex = tb.Text.Length;
                tb.SelectionLength = 0;
            }
        }

        /// <summary>
        /// Находим предложения для автозаполнения <br/>
        /// -------------------------------------------- <br/>
        /// Search for auto-completion suggestions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if
            (
                (from change in e.Changes where change.RemovedLength > 0 select change).Any() &&
                (from change in e.Changes where change.AddedLength > 0 select change).Any() == false
            ) 
            { return; }

            TextBox tb = e.OriginalSource as TextBox;
            if (sender == null || tb == null) { return; }

            // Нет причин искать, если у нас нет никаких значений (No reason to search if we don't have any values)
            IEnumerable<String> values = GetAutoCompleteItemsSource(tb);
            if (values == null) { return; }

            // Нет смысла искать, если там ничего нет (No reason to search if there's nothing there)
            if (String.IsNullOrEmpty(tb.Text)) { return; }

            // Если у нас есть строка триггер, то нужно убедится, что она была введена до предложения по автозаполнению ( If we have a trigger string, make sure that it has been typed before giving auto-completion suggestions)
            string indicator = GetAutoCompleteIndicator(tb);    // Индикатор, который указывает с какой строки начинать (An indicator that experiences alarm with the beginning of the beginning)
            int startIndex = 0;                                 // Начинаем с начала строки (Start from the beginning of the line)
            string matchingString = tb.Text;                    // Текст из TextBox (Text from TextBox)
            if (!String.IsNullOrEmpty(indicator))
            {
                startIndex = tb.Text.LastIndexOf(indicator);
                // Если мы не ввели строку триггера, то ничего не делаем (If we haven't typed the trigger string, then don't do anything)
                if (startIndex == -1) {return;}

                startIndex += indicator.Length;
                matchingString = tb.Text.Substring(startIndex, (tb.Text.Length - startIndex));
            }

            // Если у нас ничего нет после строки триггера, выходим из метода (If we don't have anything after the trigger string, return)
            if (String.IsNullOrEmpty(matchingString)) { return; }

            Int32 textLength = matchingString.Length;

            StringComparison comparer = GetAutoCompleteStringComparison(tb);
            // Делаем поиск и изменения (Do search and changes here)
            String match =
            (
                from
                    value
                in
                (
                    from subvalue
                    in values
                    where subvalue != null && subvalue.Length >= textLength
                    select subvalue
                )
                where value.Substring(0, textLength).Equals(matchingString, comparer)
                select value.Substring(textLength, value.Length - textLength)/* Выбрать только последнюю часть предложения (Only select the last part of the suggestion) */
            ).FirstOrDefault();

            // Если ничего, выходим (Nothing. Leave 'em alone)
            if (String.IsNullOrEmpty(match)) { return; }

            int matchStart = (startIndex + matchingString.Length);
            tb.TextChanged -= _OnTextChanged;
            tb.Text += match;
            tb.CaretIndex = matchStart;
            tb.SelectionStart = matchStart;
            tb.SelectionLength = (tb.Text.Length - startIndex);
            tb.TextChanged += _OnTextChanged;
        }
    }
}
