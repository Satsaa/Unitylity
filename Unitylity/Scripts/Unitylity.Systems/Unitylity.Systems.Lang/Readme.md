# Lang

## Purpose

This provides a syntax for formatting specifically crafted strings to text, and allows custom strings for each languages.

## Components

### Lang

Singleton which contains the functions to get formatted strings.

### LangText

Extended TMPro UI Text Component, ready to be used in UI. Value is formatted automatically.

### LangTextFormat

Extended TMPro UI Text Component, ready to be used in UI. Value is formatted automatically and accepts object arguments for formatting.

## Text files

Language specific files are placed in `Assets/Resources/{CustomizablePath}/{lang}-{country}.json` and loaded when needed.

```js
// en-US.json
{
	"ExampleString": "An example string.",
	"NestedExampleString": "An example of formatting: {ExampleString}",
	// Above would output "An example of formatting: An example string."
}
```
```js
// fi-FI.json
{
	"ExampleString": "Esimerkki merkkijono.",
	"NestedExampleString": "Esimerkki muotoilusta: {ExampleString}",
	// Above would output "Esimerkki muotoilusta: Esimerkki merkkijono."
}
```


## Syntax

##### Basic formatting...
```cs
{0}    -> "1"
{0:F2} -> "1.00"
```

##### Conditional formatting
```cs
{0?charge|charges} -> "charge"  // (0: 1)
{0?charge|charges} -> "charges" // (0: 5)
```

##### Conditional formatting with nesting
```cs
{0?infinite|{1}} -> "infinite" // (0: false, 1: ?)
{0?infinite|{1}} -> "5"        // (0: true , 1: 5)
```

##### String by strId
```cs
{MyString}         -> "My cool string"
{MyInfiniteString} -> "My looping string {MyInfiniteString}" -> ... // Infinite loop
{MyString_{0}}     -> {MyString_Weapon} -> "My string specific for weapons"
```

## Planned features

##### Array/flags
```cs
{MyString_{0}}      -> {MyString_Enemy}, {MyString_Wall} -> "Dynamic enemy, Dynamic wall" // Enum flags with multiple values
{MyString_{0}:"; "} -> {MyString_Enemy}; {MyString_Wall} -> "Dynamic enemy ; Dynamic wall" // Enum flags with custom separator
{{0}_Title}         -> {Swamp_Title}, {Arctic_Title} -> "Swamp Clan, Arctic Boys" // Array of strings
```

##### String manipulations
```cs
{MyString}   -> "my coOL string" // Untouched
{MyString:C} -> "My coOL string" // Capitalized sentence
{MyString:W} -> "My CoOL String" // Capitalized words
{MyString:U} -> "MY COOL STRING" // Uppercase
{MyString:L} -> "my cool string" // Lowercase
```
