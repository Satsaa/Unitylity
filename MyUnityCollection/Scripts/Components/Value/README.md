# Values guide

A Value is a container for a value, and hosts Modifiers that alter the behaviour of the Value. It takes two type parameters, T and This. T is the type of the contained value, commonly a float. This (type param) is the type of the class that is being declared (Used for knowing the type of the class within the class itself).  

A Modifier alters the operations that can be made to the contained value of a Value. For example you can alter how much a subtraction operation actually subtracts from the contained value, maybe even block it completely.

Values take a reference to a ValueData ScriptableObject which stores settings for modifiers and more in the future. The current purpose of ValueData is to store the order of execution of Modifiers.
