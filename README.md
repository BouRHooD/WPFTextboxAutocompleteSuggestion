# WPFTextboxAutocompleteSuggestion
======================

- [Русский](README.md)
- [English](README.en.md)

Прикрепленное поведение для элемента управления TextBox WPF, которое предоставляет предложения автозаполнения из заданной коллекции. 
Подобное поведение есть в штатном ComboBox (в режиме IsEditable="True").

Каждый раз, когда изменяется параметр Text в привязанном TextBox, WPFTextboxAutocompleteSuggestion будет предоставлять вам предложения по автозаполнению. 
Чтобы принять предложение, просто нажмите «Enter».

## Как использовать эту библиотеку:

1. Установите пакет через NuGet

	```
		PM> Install-Package WPFTextboxAutocompleteSuggestion
	```

2. Добавьте ссылку на библиотеку в своем представлении View (*.xaml)

	``` csharp
		xmlns:behaviors="clr-namespace:WPFTextboxAutocompleteSuggestion;assembly=WPFTextboxAutocompleteSuggestion"
	```
	
## Пример использования с TextBox:

1. Создайте TextBox и привяжите «AutoCompleteItemsSource» к коллекции ```IEnumerable<String>```

	``` csharp
		<TextBox behaviors:BehaviorAutocompleteSuggestion.AutoCompleteItemsSource="{Binding TestItems}" />
	```
2. ![](https://user-images.githubusercontent.com/51342266/190638625-161d1e21-635a-4207-9623-9cf3d9463071.gif)
	
	
