
# ObjectList Template with Generational Indicies



# ObjectList Template with Generational Indicies
ObjectList serves as a template that can work on any type that is being specified during initialization.


## Usage

#### Create ObjectList instance specifying the generic type parameter.

```javascript
  var objectList = new ObjectList<TestObject>();
  //TestObject is the generic type parameter
```

#### Add
Takes an object with the type specified during initialization `(TestObject)`.
Returns `GenerationalIndexKey` object.
```javascript
  var a_key = objectList.Add(new TestObject { Value = "A" });
  var b_key = objectList.Add(new TestObject { Value = "B" });
```

#### Get
Takes `GenerationalIndexKey ` parameter and returns specified generic type `(TestObject)`.
```javascript
  var a_val = objectList.Get(a_key);
  Console.WriteLine(a_val.Value);
  //prints "A"

  var b_val = objectList.Get(b_key);
  Console.WriteLine(b_val.Value);
  //prints "B"
```

#### Remove
Takes `GenerationalIndexKey` parameter.
```javascript
  objectList.Remove(a_key);

  a_val = objectList.Get(a_key);
  //a_val is now null
```
  

