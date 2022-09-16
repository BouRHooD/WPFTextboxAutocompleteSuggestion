# WPFTextboxAutocompleteSuggestion
======================

- [Русский](README.md)
- [English](README.en.md)

An attached behavior for WPF's TextBox control that provides auto-completion suggestions from a given collection. 
Similar behavior exists in regular ComboBox (in mode IsEditable="True").

Whenever the Text setting in the bound TextBox changes, WPFTextboxAutocompleteSuggestion will provide you with autocomplete suggestions.
To accept the offer, simply press "Enter".

## How to use this Library:

1. Install the package via NuGet

	```
		PM> Install-Package WPFTextboxAutocompleteSuggestion
	```

2. Add a reference to the library in your View (*.xaml)

	``` csharp
		xmlns:behaviors="clr-namespace:WPFTextboxAutocompleteSuggestion;assembly=WPFTextboxAutocompleteSuggestion"
	```
	
## Usage example with TextBox:

Create a TextBox and bind the "AutoCompleteItemsSource" to a collection of ```IEnumerable<String>```

	``` csharp
		<TextBox behaviors:BehaviorAutocompleteSuggestion.AutoCompleteItemsSource="{Binding TestItems}" />
	```
	
![](https://github.com/BouRHooD/WPFTextboxAutocompleteSuggestion/_Docs/Resources/_TestApp_TextBox.gif)
