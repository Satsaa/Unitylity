# Values Short Explanation

A Value (class) is a container for a value (e.g. float), and hosts Modifiers that alter the behaviour of the Value.

A Modifier alters the operations that can be made to the contained value of a Value. For example you can alter how much a subtraction operation actually subtracts from the contained value, maybe even block it completely. Other uses include having an item which increases your health while its equipped, there a Modifier added by the item would alter the "Get" operation, incrementing the result by an amount defined by the item.  

Values take a reference to a ValueData ScriptableObject which stores settings for modifiers and more in the future. The current purpose of ValueData is to store the order of execution of Modifiers.  

ValueData             |  Health Value
:-------------------------:|:-------------------------:
![](https://i.imgur.com/uYbEEvw.png)  |  ![](https://i.imgur.com/QyKU0Nb.png)

