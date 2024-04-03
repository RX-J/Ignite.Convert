# Call the basic Primitive function to convert one type into another

```cs
var a = 12;
var b = Ignite.Convert.Primitive<string> (a);
```

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
- ```float```
- ```double```
- ```decimal```
---
- ```bool```
- ```char```
- ```string```
---
- ```nint```
- ```nuint```
---

**If you include ```Ignite``` in your project, you can use the following statement:**

```cs
using Ignite;

var a = "10";
var b = a.To<int> () + 10;

Console.WriteLine (b); // Result: 20
```

# Call the Enumerable conversion functions

**You can use the Ignite Library to convert Enumerable into each other**

```cs
var a = new byte[] { 1, 2, 3, 4, 5 };
var b = Ignite.Convert.List (a); // Result: List with the elements { 1, 2, 3, 4, 5 }
var c = Ignite.Convert.Stack (a); // Result: Stack with the elements { 5, 4, 3, 2, 1 }
var d = Ignite.Convert.Queue (a); // Result: Queue with the elements { 1, 2, 3, 4, 5 }
```

**The following Enumerables can be converted**
- ```Array```
- ```List```
- ```Stack```
- ```Queue```