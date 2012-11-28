Interpreter
===========

Overview
--------
The Interpreter is an expression interpreter written in pure C#. It parses any mathematical or logical expression and returns a result. The return result depends on the return type of the last function / operation. An expression can contain variables that can be supplied before the expression is executed and the result returned. 

### Examples:
1. Passing a variable to the expression. The example below parses the express - and creates the expression tree via Engine.Parse(). The variables are then supplied to the expression and the expression executed via Execute().
```csharp
var expression = new Engine().Parse("SUM(A) * 2 - B");
expression.Variables["A"].Value = new Array(new decimal[] { 1, 2, 3, 4 });
expression.Variables["B"].Value = new Number(10);
decimal result = expression.Execute();
```

2. Using an IF function:
```csharp
decimal result = new Engine().Parse("IF(1 < 2, 10, 20)").Execute();
```

3. Custom functions can provide support for accessing data from a database:
```csharp
class GetMyDataFunction : Function
{
	public override string Name 
	{
		get 
		{
			return "GETMYDATA"; 
		}
	}

	public override Literal Execute(IConstruct[] arguments)
	{
		base.EnsureArgumentCountIs(arguments, 2);
	
		var tableName = base.GetTransformedArgument<Text>(arguments, 0);
		var fieldName = base.GetTransformedArgument<Text>(arguments, 1);

		// Retrieve data using tableName and fieldName and return Array<Number>.
		// This return value can then be used by any functions that accept Array<Number> as an argument such as SUM().
		// return new Array(new decimal[] { 1, 2, 3, 4 });
	}
}
var engine = new Engine();
engine.Register(new GetMyDataFunction());	
decimal result = Engine.Parse("SUM(GETMYDATA('MyTable', 'MyField'))").Execute();
```

4. Custom functions that manipulate values:
```csharp
class NegateNumber : Function
{
    public override string Name 
    {
        get 
        {
	        return "NEGATE"; 
        }
    }

    public override Literal Execute(IConstruct[] arguments)
    {
        base.EnsureArgumentCountIs(arguments, 1);

		decimal inputValue = base.GetTransformedArgument<Number>(arguments, argumentIndex: 0);

        return new Number(-inputValue);
    }
}
var engine = new Engine();
engine.Register(new NegateNumber());
decimal result = Engine.Parse("NEGATE(1)").Execute();
```

### Supported Functions 
* SUM(array)
* AVG(array)
* IF(condition, trueResult, falseResult)
* Array(item1, item2, ...) 
* Format(value [, format])  -- Formats a number or date/time
* Len(text) -- returns the length of a string
* Custom functions can be created by extending Function and registered it via `Engine.Register(Function)`

### Supported data types (can be extended)
* Number/decimal
  - Example: 1.0
* Boolean
  - Supports constants 'true' and 'false'
  - Example: true <> false
* Array
  - Can contain all data types
  - Data types can be mixed in the same array
  - Example: Array(1, 2, 3, 4)
* Text
  - Delimited by " or ' characters
  - Exampe: 'ABC'
* Date/Time 
  - Surrounded by '#' characters
  - Example: #2000-01-30 12:30:03# 
  
### Supported functions (can be extended)
* If(condition, trueResult, falseResult)
  - Example: If(1 > 2, 10, 20)
* Max(array)
  - Example: Max(Array(1, 2, 3))
* Min(array)
  - Example: Min(Array(1, 2, 3))
* Sum(array):
  - Example: Sum(Array(1, 2, 3))
* Today:
  - Returns the date component (no time component) for today.
  - Example: Today() + 1  -- returns the date for tomorrow
* Len(text)
  - Returns the length of a string
  - Example: Len('abc') -- returns 3

### Supported Operations (can be extended)
* +		- addition  (numbers, date/time + number, string concatenation)
* -		- subtraction (numbers, date/time - number)
* /		- divide
* *		- multiply
* =		- equal
* <>	- not equal to
* <		- less than
* >		- greater than
* >=	- greater than or equal to
* <=	- less than or equal to
* OR	- logical or
* AND	- logical and

Supported Platforms
-------------------
Supported platforms are Windows and MonoTouch/Mono. 

License
-------
The library can be used for commercial and non-commercial purposes.

Unit Tests
----------
The unit test project is available in a separate repository on [Github here](https://github.com/hisystems/Interpreter-UnitTests). It is also a good resource for examples on how to utilise the library. To run the unit tests project in conjunction with the library it must be located in the same directory as the library.

For example:

/Interpreter

/Interpreter.UnitTests
