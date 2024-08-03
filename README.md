# Both the ```Convert``` and the similar function ```Convert (Basic)``` support conversions for the same data types

**Important: The ```Convert``` class has full support for everything, the ```Convert (Basic)``` class only supports the conversion for primetive data types, without tuples, and the conversion form one enumeble into another enumeble, without type changing.**

**The following data types can be converted:**
- ```byte```
- ```ushort```
- ```uint```
- ```ulong```
---
- ```sbyte```
- ```short```
- ```int```
- ```long```
---
- ```nint```
- ```nuint```
---
- ```float```
- ```double```
- ```decimal```
---
- ```bool```
- ```char```
- ```string```
---

**To convert primitive data types and tuples you can write:**

```cs
Ignite.Convert.Primitive<TYPE> (value);

// Example:

var value = "10";
var result = Ignite.Convert.Primitive<int> (value) + 10; // The result here will be 20, since it will convert the string "10" to an int and then adds 10 to the value of the int

Console.WriteLine (result); // Prints the expected value: 20

// If you create a tuple like this:

var Vector3D = (0, 0, 0);

// You can also convert these tuplets by writing:

var Vector3D_byte = Ignite.Convert.Primitive<(byte X, byte Y, byte Z)> (Vector3D); // As a type you enter the wanted tuple type and the rest will be automatically converted, complex and nested tuplets are also supported
```

**If you include ```Ignite``` in your project, you can use the following, short statement for ```Convert.Primitive```:**

```cs
using Ignite;

var a = "10";
var b = a.To<int> () + 10;

Console.WriteLine (b); // Result: 20

// Tuples are supported as well
```

# Call the enumerable conversion functions

**You can use the Ignite library to convert enumerable into each other, the following enumerable can be converted:**

- ```Array```
- ```List```
- ```Stack```
- ```Queue```

**If you just want to change the type of an enumerable into another you can write:**
```cs
var a = new byte[] { 1, 2, 3, 4, 5 };
var b = Ignite.Convert.List (a); // Result: List with the elements { 1, 2, 3, 4, 5 }
var c = Ignite.Convert.Stack (a); // Result: Stack with the elements { 5, 4, 3, 2, 1 }
var d = Ignite.Convert.Queue (a); // Result: Queue with the elements { 1, 2, 3, 4, 5 }
var e = Ignite.Convert.Array (b); // Result: Array with the elements { 1, 2, 3, 4, 5 }
```

**To change the types of an enumerable you can write:**
```cs
var a = new string[] { "1", "2", "3", "4", "5" };
var b = Ignite.Convert.Array<int> (a); // Result: List with the elements { 1, 2, 3, 4, 5 } as int
var c = Ignite.Convert.List<int> (a); // Result: List with the elements { 1, 2, 3, 4, 5 } as int
var d = Ignite.Convert.Stack<int> (a); // Result: Stack with the elements { 5, 4, 3, 2, 1 } as int
var e = Ignite.Convert.Queue<int> (a); // Result: Queue with the elements { 1, 2, 3, 4, 5 } as int

// These functions also work with tuples and not just the primetive data types
```

**References are fully supported, so you can use ```in```, ```out``` and ```ref``` for every of these functions:**

``` cs
var input = "10";

Ignite.Convert.Primitive (input, out int output); // Result: 10 as an int
Ignite.Convert.Primitive (ref output, input + "0"); // Result: 100 as an int

// For enumerable:

var input = new byte[] { 1, 2, 3, 4, 5 };

Ignite.Convert.List<int> (input, out var output); // Result: { 1, 2, 3, 4, 5 } as a List<int>
Ignite.Convert.List (ref output, input.Skip (1)); // Result: { 2, 3, 4, 5 } as a List<int>
```

**Nice to have: The ```Primitive``` function allows a value to be returned if the conversion could not be completed successfully:**

```cs
var input = "1";
var output = Ignite.Convert.Primitive (input, false); // Result: false, since the string "1" can not be converted to a bool, so the given value false gets returned, this will also function for tuples and any type of references
```