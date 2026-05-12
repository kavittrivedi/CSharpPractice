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
Interview Question: What is call back function in jquery? In jQuery, a callback function is a function that is passed as an argument to another function and is executed after the completion of that function. It ensures that a certain block of code runs after a specific event or action has completed.

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
