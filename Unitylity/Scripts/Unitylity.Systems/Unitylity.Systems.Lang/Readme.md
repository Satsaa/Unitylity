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
	"ExampleStr": "An example string.",
	"NestedExampleStr": "An example of formatting: {ExampleStr}",
	// Above would output "An example of formatting: An example string."
	"ContextExample": "{@Name} hit {@Target} with {@Weapon}, and {@Target} lost {@Damage} {@Damage?heart:hearts}",
	// Above would output e.g. "Ganondorf hit Link with Tail, and Link lost 5 hearts."
}
```
```js
// fi-FI.json
{
	"ExampleStr": "Esimerkki merkkijono.",
	"NestedExampleStr": "Esimerkki muotoilusta: {ExampleStr}",
	// Above would output "Esimerkki muotoilusta: Esimerkki merkkijono."
	"ContextExample": "{@Name} iski {@Target} {@Weapon}, ja {@Target} menetti {@Damage} {@Damage?sydämen:sydäntä}",
	// Above would output e.g. "Ganondorf iski Link Häntä, ja Link menetti 5 sydäntä."
	// Note that here we have a problem with locative forms.
	// To solve this we can add the forms ourselves like follows:
	"ContextExample": "{@Name} iski {{@Target}_Partitive} {{@Weapon}_Adessive}, ja {@Target} menetti {@Damage} {@Damage?sydämen:sydäntä}",
	// Above would output e.g. "Ganondorf iski Linkiä Hännällä, ja Link menetti 5 sydäntä."

	// You could also create a custom string formatter by extending the Lang singleton and overriding GetStringFormatter.
	// The formatter could e.g. get locative forms from a library so that:
	// "{@Weapon:Adessive}" would fetch the adessive form for "häntä", which is "hännällä"
	// Such libraries would not provide the locative forms for unusual names like "Link"
}
```


## Syntax

## Basics

##### Just strings
```cs
Hello world         -> "Hello world"
Hello world \{^.^\} -> "Hello world {^.^}" -> ... // Infinite loop
{MyString_{0}}      -> {MyString_Weapon} -> "My string specific for weapons"
```

##### String by string id
```cs
{MyString}         -> "My cool string"
{MyInfiniteString} -> "My looping string {MyInfiniteString}" -> ... // Infinite loop
{MyString_{0}}     -> {MyString_Weapon} -> "My string specific for weapons"
```

##### Context values
```cs
{@UnitHealth} -> "68"
{@PoolIsOpen} -> "True"
```

## Formatting

##### Basic formatting...
```cs
{@MyInt}    -> "1"
{@MyInt:F2} -> "1.00"
```

##### Conditional formatting
```cs
{@Ammo?charge|charges} -> "charge"  // (Ammo: 1)
{@Ammo?charge|charges} -> "charges" // (Ammo: 5)
```

##### Conditional formatting with nesting
```cs
{@IsFinite?infinite|{@MyFloat}} -> "infinite" // (IsFinite: false, MyFloat: 5)
{@IsFinite?infinite|{@MyFloat}} -> "5"        // (IsFinite: true , MyFloat: 5)
```

##### String manipulations
```cs
{MyString}    -> "my coOL string" // Unchanged
{MyString:U}  -> "MY COOL STRING" // Uppercase
{MyString:L}  -> "my cool string" // Lowercase
{MyString:C}  -> "My coOL string" // Capitalize first char
{MyString:LC} -> "My coOL string" // Capitalized first char only
{MyString:W}  -> "My CoOL String" // Capitalize words first char
{MyString:LW} -> "My Cool String" // Capitalized words first char only
```

## Planned features

##### Array/flags
```cs
{MyString_{@Objects}}      -> {MyString_Enemy}, {MyString_Wall} -> "Dynamic enemy, Dynamic wall" // Enum flags with multiple values
{MyString_{@Objects}:"; "} -> {MyString_Enemy}; {MyString_Wall} -> "Dynamic enemy; Dynamic wall" // Enum flags with custom separator
{{@Biomes}_Title}          -> {Swamp_Title}, {Arctic_Title}     -> "Swamp Clan, Arctic Boys" // Array of strings
```
