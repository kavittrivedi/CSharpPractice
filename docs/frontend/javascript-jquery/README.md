# jQuery and Javascript Interview Practice

## How to check DOM is fully loaded?

Yes, you can use `$(document).ready()` in jQuery to ensure that the DOM is fully loaded before executing any JavaScript code. Here’s a proper syntax example:

```javascript
$(document).ready(function() {
    // Your code here
});
```

Alternatively, you can use the shorthand version:

```javascript
$(function() {
    // Your code here
});
```

Both methods are correct and ensure that your code will run after the DOM is fully loaded.

## Can we write $(document).ready() multiple times? 

Yes, you can write `$(document).ready()` multiple times in the same file. Each instance will be executed once the DOM is fully loaded. jQuery will queue up all `$(document).ready()` functions and run them in the order they were declared after the page is loaded. Here's an example:

```javascript
$(document).ready(function() {
    console.log("First ready function");
});

$(document).ready(function() {
    console.log("Second ready function");
});
```

Both functions will run once the DOM is ready.

## How many ways we can bind events in jquery? 

In jQuery, there are several ways to bind events to elements:

1. **Inline HTML**: Using `onclick`, `onmouseover`, etc., within the HTML element.

   ```html
   <button onclick="alert('Clicked')">Click me</button>
   ```

2. **`on()` Method**: The preferred method to bind events, supporting multiple events and delegation.

   ```javascript
   $('#myButton').on('click', function() {
       alert('Button clicked');
   });
   ```

3. **Shortcut Methods**: Like `.click()`, `.dblclick()`, `.hover()`.

   ```javascript
   $('#myButton').click(function() {
       alert('Button clicked');
   });
   ```

4. **`bind()` Method**: Older way of binding, now replaced by `on()`.

   ```javascript
   $('#myButton').bind('click', function() {
       alert('Button clicked');
   });
   ```

5. **Event Delegation**: Bind events to dynamically added elements using `on()`.

   ```javascript
   $('#parentDiv').on('click', 'button', function() {
       alert('Dynamic button clicked');
   });
   ```

## WHat is $ in jQuery? 

In jQuery, `$` is simply an alias for `jQuery`. It’s a shorthand notation used to access jQuery functions, such as selecting elements, manipulating the DOM, handling events, etc.

For example:

```javascript
// Using $
$(document).ready(function() {
    console.log("DOM is ready");
});

// Equivalent to:
jQuery(document).ready(function() {
    console.log("DOM is ready");
});
```

You can also use `noConflict()` if `$` is conflicting with other libraries that use the `$` symbol.

## What is call back function in jquery? 

In jQuery, a callback function is a function that is passed as an argument to another function and is executed after the completion of that function. It ensures that a certain block of code runs after a specific event or action has completed.

For example:

```javascript
$("#myDiv").fadeOut("slow", function() {
    alert("Fade out complete");
});
```

In this example, the anonymous function is a callback that runs after the `fadeOut` effect is completed.

Callback functions are useful for handling asynchronous operations.

In jQuery, a callback function is a function that is passed as an argument to another function and is executed after the completion of that function. It ensures that a certain block of code runs after a specific event or action has completed.

For example:

```javascript
$("#myDiv").fadeOut("slow", function() {
    alert("Fade out complete");
});
```

In this example, the anonymous function is a callback that runs after the `fadeOut` effect is completed.

Callback functions are useful for handling asynchronous operations.
Here's another example of a callback function in jQuery:

```javascript
function logMessage() {
    console.log("Animation complete!");
}

$("#myDiv").slideUp(1000, logMessage);
```

In this example, `logMessage` is a callback function. It gets passed as the second argument to `slideUp`, and is called after the sliding animation is finished. The callback ensures that the message is logged only after the animation completes, not before.

Callbacks are often used for animations, AJAX requests, or any time you need to wait for an action to finish before executing another.

## What is difference between == and === in javascript?

In JavaScript, `==` and `===` are both comparison operators, but they behave differently when evaluating equality:

### 1. **`==` (Equality Operator)**

* The `==` operator checks for **value equality** but performs **type coercion**. This means that if the operands are of different types, JavaScript will attempt to convert one or both of them to a common type before making the comparison.
* Example:

  ```javascript
  console.log(5 == '5'); // true, because '5' is converted to a number
  console.log(null == undefined); // true, they are considered equal
  console.log(0 == false); // true, 0 is converted to false
  ```

### 2. **`===` (Strict Equality Operator)**

* The `===` operator checks for both **value equality** and **type equality**. It does not perform any type coercion; both operands must be of the same type and value to be considered equal.
* Example:

  ```javascript
  console.log(5 === '5'); // false, different types (number vs. string)
  console.log(null === undefined); // false, different types (null vs. undefined)
  console.log(0 === false); // false, different types (number vs. boolean)
  ```

### Summary

* Use `==` when you want to compare values and allow type conversion (though it is generally not recommended due to potential confusion).
* Use `===` when you want a strict comparison that checks both the value and the type, which is the preferred practice to avoid unexpected results in your code.

## Which jquery library we can use for pagination?

For pagination in jQuery, a popular library you can use is **jQuery DataTables**. It provides built-in support for pagination, sorting, and searching in table data. Another option is **twbsPagination**, which is a simple, lightweight plugin for pagination that works well with AJAX content.

### Example with DataTables:

```javascript
$(document).ready(function() {
    $('#myTable').DataTable({
        paging: true
    });
});
```

Both of these libraries help in handling pagination efficiently.

For pagination using **jQuery DataTables**, the library's file name is typically:

* **`jquery.dataTables.min.js`** for the JavaScript file.
* **`jquery.dataTables.min.css`** for the optional CSS file to style the table.

For **twbsPagination**, the file name is:

* **`jquery.twbsPagination.min.js`** for the JavaScript file.

These libraries need to be included in your project for pagination functionality. You can download them from the official DataTables or twbsPagination websites or use a CDN.

## var vs let vs const in JavaScript / jQuery

`var`, `let`, and `const` are used to declare variables in JavaScript. They work the same way whether the code is plain JavaScript or written inside jQuery functions like `$(document).ready()` or click handlers.

### 1. var

`var` is the old way of declaring variables in JavaScript.

Important points:

* `var` has function scope.
* It does not have block scope.
* It can be re-declared and updated.
* It is hoisted and initialized with `undefined`.

Example:

```javascript
function testVar() {
    if (true) {
        var name = "John";
    }

    console.log(name); // John
}
```

Here, `name` is accessible outside the `if` block because `var` is function scoped.

Another example:

```javascript
var age = 25;
var age = 30;

console.log(age); // 30
```

`var` allows re-declaration, which can sometimes create bugs in large code.

### 2. let

`let` is the modern way of declaring variables whose value can change.

Important points:

* `let` has block scope.
* It can be updated.
* It cannot be re-declared in the same scope.
* It is hoisted, but cannot be used before declaration because of the temporal dead zone.

Example:

```javascript
function testLet() {
    if (true) {
        let name = "John";
        console.log(name); // John
    }

    // console.log(name); // Error: name is not defined
}
```

Here, `name` is only available inside the `if` block.

Another example:

```javascript
let count = 1;
count = 2;

console.log(count); // 2
```

`let` is useful when the variable value needs to change later.

### 3. const

`const` is used to declare variables whose assignment should not change.

Important points:

* `const` has block scope.
* It must be initialized at the time of declaration.
* It cannot be re-declared in the same scope.
* It cannot be reassigned.
* For objects and arrays, the reference cannot change, but internal values can change.

Example:

```javascript
const pi = 3.14;

// pi = 3.14159; // Error: Assignment to constant variable
```

Object example:

```javascript
const user = {
    name: "John"
};

user.name = "David"; // Allowed

console.log(user.name); // David
```

This is allowed because the object reference is still the same. Only the property value changed.

But this is not allowed:

```javascript
const user = {
    name: "John"
};

// user = { name: "David" }; // Error
```

### jQuery example

```javascript
$(document).ready(function() {
    const button = $("#btnSave");
    let clickCount = 0;

    button.on("click", function() {
        clickCount++;
        console.log("Button clicked " + clickCount + " times");
    });
});
```

In this example:

* `const button` is used because the selected jQuery element reference does not need to change.
* `let clickCount` is used because the count changes every time the button is clicked.
* `var` is avoided because `let` and `const` provide better scoping and safer code.

### Main difference table

| Feature | var | let | const |
| --- | --- | --- | --- |
| Scope | Function scope | Block scope | Block scope |
| Re-declaration | Allowed | Not allowed in same scope | Not allowed in same scope |
| Reassignment | Allowed | Allowed | Not allowed |
| Must initialize | No | No | Yes |
| Hoisting | Yes, initialized with undefined | Yes, but temporal dead zone | Yes, but temporal dead zone |
| Modern usage | Avoid mostly | Use when value changes | Use by default |

### Interview answer

`var` is function scoped and can be re-declared, so it can create unexpected bugs. `let` and `const` are block scoped and are preferred in modern JavaScript. I use `let` when the value needs to change, and `const` when the variable should not be reassigned. In jQuery also, the same rules apply because jQuery is just a JavaScript library.

