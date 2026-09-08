# Angular Microfrontend Interview Notes

## 1. One-Line Definition

An **Angular Microfrontend** is a way to split a large frontend application into smaller, independently developed and deployed Angular applications, usually owned by different teams, and composed together inside one user experience.

Think of it like microservices, but for the UI.

## 2. Why Microfrontends?

Use microfrontends when a frontend has become too large for one team or one release pipeline.

Common reasons:

- Different teams can work independently.
- Each feature area can be built, tested, and deployed separately.
- Large Angular apps become easier to maintain.
- Teams can gradually modernize old frontend code.
- A failure in one feature should not break the whole application if designed well.
- Different parts of the UI can use different versions or even different frameworks, although Angular-only is simpler.

Interview sentence:

> We use microfrontends when organizational scaling becomes more important than keeping a single frontend codebase. The main goal is independent ownership, development, and deployment.

## 3. Important Terms

| Term | Meaning |
|---|---|
| Host / Shell / Container | The main Angular app that loads and composes microfrontends |
| Remote / Microfrontend | A separately built frontend app exposed to the shell |
| Module Federation | Webpack 5 feature commonly used to load remote Angular modules at runtime |
| Remote Entry | JavaScript file that tells the shell what a remote exposes |
| Exposed Module | Angular module, route, component, or bootstrap file made available to the shell |
| Shared Dependency | Common package like Angular, RxJS, or a design system shared between apps |
| Runtime Composition | Loading microfrontends in the browser while the app is running |
| Build-Time Composition | Combining microfrontends during build time |

## 4. Simple Mental Model

Remember this:

```text
Shell App
  |
  |-- loads Auth MFE
  |-- loads Product MFE
  |-- loads Cart MFE
  |-- loads Orders MFE
```

The shell usually owns:

- Layout
- Header/sidebar/navigation
- Authentication guard
- Global routing
- Loading remote apps
- Global error boundaries
- Shared configuration

Each microfrontend usually owns:

- Its feature screens
- Its child routes
- Its components
- Its API calls
- Its local state
- Its tests
- Its deployment

## 5. Monolith vs Microfrontend

| Frontend Monolith | Microfrontend |
|---|---|
| One Angular app | Multiple Angular apps |
| One build | Multiple builds |
| One deployment | Independent deployments |
| Easier at small scale | Better for large teams |
| Simple routing | Distributed routing |
| Simple shared state | State communication must be designed carefully |
| Fewer runtime risks | More integration/runtime risks |

Interview sentence:

> A monolithic Angular app is simpler and usually better for small teams. Microfrontends add complexity, so I would choose them only when independent team ownership and deployment justify that complexity.

## 6. Angular Microfrontend Approaches

### 6.1 Module Federation

Most common approach in Angular.

It uses **Webpack 5 Module Federation** to load code from another application at runtime.

Example:

```text
Shell app loads:
https://products.company.com/remoteEntry.js

Then it renders:
ProductsModule or ProductsRoutes
```

Best for:

- Independent deployments
- Runtime loading
- Angular-to-Angular microfrontends
- Large enterprise apps

### 6.2 Web Components

Each microfrontend is packaged as a custom element.

Example:

```html
<app-product-list></app-product-list>
```

Best for:

- Framework-agnostic integration
- Embedding Angular into non-Angular apps
- Smaller widgets

Tradeoff:

- Routing, dependency sharing, and state communication are more manual.

### 6.3 IFrame

Each microfrontend runs inside an iframe.

Best for:

- Strong isolation
- Legacy apps
- Security boundaries

Tradeoff:

- Harder communication
- Harder styling
- Worse user experience if overused

### 6.4 NPM Package Composition

Feature apps are published as libraries and consumed by a shell.

Best for:

- Shared UI libraries
- Build-time integration

Tradeoff:

- Not truly independently deployed, because shell must rebuild.

## 7. Most Common Angular Setup: Module Federation

Typical apps:

```text
workspace/
  shell/
  products/
  cart/
  orders/
```

The shell loads remote apps using their `remoteEntry.js`.

Example runtime flow:

```text
1. User opens shell application
2. Shell loads main layout and global routes
3. User clicks Products
4. Shell downloads products remoteEntry.js
5. Shell loads exposed ProductsModule or routes
6. Angular renders the remote feature inside router-outlet
```

## 8. Host and Remote Example

### Shell Route Example

```ts
const routes: Routes = [
  {
    path: 'products',
    loadChildren: () =>
      loadRemoteModule({
        type: 'module',
        remoteEntry: 'https://products.company.com/remoteEntry.js',
        exposedModule: './ProductsModule'
      }).then(m => m.ProductsModule)
  }
];
```

Meaning:

- When user visits `/products`, shell loads remote app.
- `remoteEntry` is the remote manifest file.
- `exposedModule` is what the remote app exposes.
- Then Angular lazy loads that module.

### Remote Exposes Example

```js
module.exports = {
  name: 'products',
  exposes: {
    './ProductsModule': './src/app/products/products.module.ts'
  }
};
```

Meaning:

- The remote app named `products` exposes `ProductsModule`.
- The shell can load it dynamically.

## 9. Routing Strategy

Good practice:

- Shell owns top-level routes.
- Remote owns child routes.
- Use lazy loading for remote features.
- Keep route names stable.
- Avoid remotes directly depending on shell route internals.

Example:

```text
/products          -> Shell route, loads Products MFE
/products/list     -> Products MFE child route
/products/details  -> Products MFE child route
/cart              -> Shell route, loads Cart MFE
```

Interview sentence:

> In Angular microfrontends, I usually let the shell own global navigation and top-level routing, while each remote owns its internal child routes. This keeps integration simple and preserves team autonomy.

## 10. Shared Dependencies

Angular, RxJS, and common libraries should usually be shared.

Example:

```js
shared: {
  '@angular/core': { singleton: true, strictVersion: true },
  '@angular/common': { singleton: true, strictVersion: true },
  '@angular/router': { singleton: true, strictVersion: true },
  'rxjs': { singleton: true, strictVersion: true }
}
```

Important options:

| Option | Meaning |
|---|---|
| singleton | Only one instance should be used |
| strictVersion | Version mismatch should fail instead of silently using wrong version |
| requiredVersion | Expected package version |

Why singleton matters:

- Angular core should not be loaded multiple times.
- Router should have one consistent instance.
- Shared services may break if duplicated.
- Bundle size is reduced.

Interview sentence:

> For Angular microfrontends, I usually share Angular packages as singletons because multiple Angular runtimes or router instances can create unpredictable behavior and increase bundle size.

## 11. Communication Between Microfrontends

Microfrontends should be loosely coupled.

Common communication options:

### 11.1 URL and Route Params

Best for page-level state.

Example:

```text
/products/42
```

Use when:

- State should be bookmarkable.
- Browser refresh should preserve context.

### 11.2 Shared Service

A shared Angular library can expose a service.

Use when:

- Apps are Angular-only.
- Communication is simple and controlled.

Risk:

- Can create tight coupling if overused.

### 11.3 Browser Events

Use custom events for framework-neutral communication.

Example:

```ts
window.dispatchEvent(new CustomEvent('cart:itemAdded', {
  detail: { productId: 42 }
}));
```

Use when:

- Remotes should not import each other.
- Apps may use different frameworks.

### 11.4 Shared State Library

Examples:

- NgRx
- RxJS store
- Signals-based store

Use carefully.

Good for:

- Authentication state
- User profile
- Cart count
- Global notifications

Avoid:

- Putting every feature's internal state into one global store.

### 11.5 Backend as Source of Truth

Often the cleanest option.

Each microfrontend communicates through APIs, not directly with each other.

Interview sentence:

> My first preference is URL state or backend-driven state. If UI-level communication is required, I use a small shared contract like events or a shared service, but I avoid making microfrontends depend deeply on each other's internals.

## 12. Authentication and Authorization

Usually the shell handles:

- Login redirect
- Token acquisition
- Session refresh
- Global auth guard
- User profile loading

Remotes handle:

- Feature-specific permissions
- Hiding/disabling actions
- Calling APIs with the authenticated context

Best practice:

- Keep auth state in a shared auth library or shell-provided service.
- Do not duplicate login logic in every remote.
- Enforce real authorization on the backend, not only in Angular.

Interview sentence:

> The shell usually owns authentication because it controls app entry and global routing. Remotes can consume user and permission context, but backend APIs must still enforce authorization.

## 13. Deployment

Each microfrontend can have its own pipeline.

Example:

```text
Shell     -> https://app.company.com
Products  -> https://products.company.com/remoteEntry.js
Cart      -> https://cart.company.com/remoteEntry.js
Orders    -> https://orders.company.com/remoteEntry.js
```

Important deployment concerns:

- Remote URLs per environment
- Version compatibility
- Cache busting
- Rollbacks
- Monitoring
- Runtime failure handling

### Environment Configuration

Avoid hardcoding remote URLs.

Better:

```json
{
  "products": "https://products.company.com/remoteEntry.js",
  "cart": "https://cart.company.com/remoteEntry.js"
}
```

The shell can load this configuration at startup.

## 14. Versioning Strategy

Microfrontends need clear contracts.

Version these carefully:

- Shared libraries
- Exposed module names
- Route contracts
- Event names and payloads
- API contracts
- Design system components

Good rule:

> A remote can change internally anytime, but anything consumed by the shell or another app is a public contract.

## 15. Error Handling

A remote can fail to load because:

- Network issue
- Deployment issue
- Wrong remote URL
- Version mismatch
- Remote app runtime error

Shell should handle failure gracefully.

Example behavior:

- Show a friendly fallback screen.
- Log error to monitoring.
- Keep rest of shell working.
- Allow retry.

Interview sentence:

> Since Module Federation loads remote code at runtime, I always plan for remote loading failure. The shell should show a fallback UI and log the error instead of breaking the entire application.

## 16. Performance

Benefits:

- Lazy load feature areas.
- Smaller initial shell bundle.
- Teams deploy independently.

Risks:

- Too many remote requests.
- Duplicate dependencies.
- Slow remote startup.
- Poor caching.
- Large shared libraries.

Best practices:

- Lazy load remotes by route.
- Share Angular dependencies as singletons.
- Use CDN caching for remote assets.
- Preload important remotes after shell startup.
- Keep shared libraries small.
- Monitor real user performance.

## 17. Styling and Design System

Common approaches:

- Shared design system library
- Shared CSS variables
- Angular Material or another UI library
- Strict component boundaries

Avoid:

- One remote accidentally overriding another remote's styles.
- Global CSS conflicts.
- Different teams creating inconsistent UI patterns.

Best practices:

- Use scoped component styles.
- Keep global styles minimal.
- Share design tokens.
- Version the design system.

Interview sentence:

> A design system is very important in microfrontends because independent teams can otherwise create inconsistent UI. I prefer shared tokens and reusable components, with minimal global CSS.

## 18. Testing Strategy

Test at multiple levels:

| Test Type | Purpose |
|---|---|
| Unit tests | Test components/services inside each remote |
| Integration tests | Test remote module loading and shared contracts |
| Contract tests | Verify routes, events, APIs, exposed modules |
| E2E tests | Test user journeys across shell and remotes |
| Smoke tests | Verify deployed remoteEntry URLs work |

Important interview point:

> Each microfrontend should test itself independently, but the shell also needs integration or E2E tests for critical journeys across remotes.

## 19. CI/CD

Typical setup:

```text
Products MFE pipeline:
  install -> test -> build -> publish remote assets -> smoke test

Shell pipeline:
  install -> test -> build -> deploy shell -> verify remote config
```

Good practices:

- Independent pipelines per remote.
- Shared library version checks.
- Automated contract checks.
- Smoke tests after deployment.
- Rollback strategy for each remote.

## 20. Advantages

- Independent development
- Independent deployment
- Better team ownership
- Smaller codebases
- Easier feature isolation
- Technology migration path
- Lazy loading by domain

## 21. Disadvantages

- More architecture complexity
- Runtime integration failures
- Dependency version problems
- Harder debugging
- More CI/CD coordination
- Potential UI inconsistency
- Cross-app communication complexity
- Testing end-to-end flows is harder

Interview sentence:

> Microfrontends solve team and deployment scaling problems, but they introduce runtime integration, dependency, and communication complexity. They are powerful, but not free.

## 22. When Not To Use Microfrontends

Avoid microfrontends when:

- The app is small.
- Only one team owns the frontend.
- Independent deployment is not needed.
- The team does not have strong CI/CD maturity.
- Shared state is very complex and tightly coupled.
- The organization wants microfrontends only because it sounds modern.

Interview sentence:

> I would not choose microfrontends for a small Angular app. A modular monolith with lazy-loaded feature modules is usually simpler until team boundaries and deployment independence become real problems.

## 23. Angular Modular Monolith vs Microfrontend

Angular already supports modular architecture.

A good first step is often:

```text
One Angular app
  feature modules
  lazy loading
  shared libraries
  clean boundaries
```

Move to microfrontends when:

- Teams need independent release cycles.
- Build time is too high.
- Code ownership is difficult.
- Different domains need autonomy.

Interview sentence:

> I prefer starting with a well-structured Angular modular monolith. If organizational scale demands independent deployments, then I move toward microfrontends.

## 24. Common Interview Questions and Answers

### Q1. What is Angular Microfrontend?

Angular microfrontend is an architecture where a large Angular frontend is split into multiple smaller frontend applications. A shell application composes them, and each remote application can be developed and deployed independently.

### Q2. What is the shell application?

The shell is the host Angular app. It usually owns the layout, top-level routing, authentication entry point, navigation, and runtime loading of remote microfrontends.

### Q3. What is a remote?

A remote is a separately built frontend application that exposes a module, route, component, or bootstrap file to the shell.

### Q4. What is Module Federation?

Module Federation is a Webpack 5 feature that allows one JavaScript application to dynamically load code from another independently built application at runtime.

### Q5. What is remoteEntry.js?

`remoteEntry.js` is the manifest-like file generated by Module Federation. It tells the host what modules the remote exposes and how to load them.

### Q6. How does Angular routing work in microfrontends?

The shell defines top-level routes like `/products` or `/cart`. When a route is activated, the shell lazy loads the remote module. The remote then manages its own child routes.

### Q7. How do microfrontends communicate?

They can communicate through route params, query params, shared services, browser events, shared state, or backend APIs. The best choice is usually the least coupled option that satisfies the use case.

### Q8. How do you share Angular dependencies?

In Module Federation, Angular dependencies like `@angular/core`, `@angular/router`, and `rxjs` are usually configured as singleton shared dependencies to avoid duplicate runtime instances and version conflicts.

### Q9. What are the biggest challenges?

The biggest challenges are dependency versioning, cross-app communication, consistent UI, runtime remote loading failures, testing across apps, and deployment coordination.

### Q10. How do you handle remote loading failure?

The shell should catch load errors, show a fallback UI, log the error to monitoring, and keep the rest of the application usable.

### Q11. Can different microfrontends use different frameworks?

Yes, but it increases complexity. Web Components or iframes can help with framework-independent integration. For most Angular enterprise apps, using Angular across all remotes is simpler.

### Q12. How is authentication handled?

The shell usually handles login, session, token refresh, and global route guards. Remotes consume auth context and enforce feature-level permissions, while backend APIs enforce real authorization.

### Q13. How do you deploy microfrontends?

Each remote can have its own CI/CD pipeline and publish its assets independently. The shell references remote URLs through environment configuration or runtime configuration.

### Q14. What is the difference between lazy-loaded Angular modules and microfrontends?

Lazy-loaded modules are still part of one Angular application and one deployment. Microfrontends are separate applications that can be built and deployed independently.

### Q15. What should be shared and what should not be shared?

Share stable things like Angular packages, design system components, auth contracts, and utility libraries. Do not share feature internals or every piece of state, because that creates tight coupling.

## 25. Architecture Diagram

```text
                         +----------------------+
                         |      Shell App       |
                         |----------------------|
                         | Layout               |
                         | Top-level routing    |
                         | Auth entry           |
                         | Remote loading       |
                         +----------+-----------+
                                    |
              +---------------------+---------------------+
              |                     |                     |
              v                     v                     v
     +----------------+     +----------------+     +----------------+
     | Products MFE   |     | Cart MFE       |     | Orders MFE     |
     |----------------|     |----------------|     |----------------|
     | Child routes   |     | Child routes   |     | Child routes   |
     | Product APIs   |     | Cart APIs      |     | Order APIs     |
     | Local state    |     | Local state    |     | Local state    |
     +----------------+     +----------------+     +----------------+
```

## 26. Remember With This Formula

```text
MFE = Shell + Remotes + Contracts + Independent Deployment
```

Or:

```text
S R C D

S = Shell composes
R = Remotes own features
C = Contracts connect them
D = Deploy independently
```

If you remember only one thing:

> Microfrontend is not mainly a technical pattern. It is a team-scaling and deployment-independence pattern implemented with frontend composition.

## 27. Best Interview Answer Template

Use this structure for almost any Angular microfrontend answer:

```text
First explain ownership:
The shell owns global concerns, and remotes own feature domains.

Then explain loading:
With Module Federation, the shell loads remoteEntry.js and lazy loads the exposed module or routes.

Then explain contracts:
Communication should happen through stable contracts like routes, events, shared services, APIs, or shared libraries.

Then explain tradeoffs:
The benefit is independent development and deployment. The cost is more complexity in versioning, testing, dependency sharing, and runtime failure handling.
```

## 28. Final 60-Second Interview Answer

Angular microfrontend architecture means splitting a large Angular frontend into multiple smaller applications. Usually there is a shell or host app that owns layout, top-level routing, authentication entry, and navigation. Feature teams own remote apps like Products, Cart, or Orders.

In Angular, the common implementation is Webpack Module Federation. Each remote exposes a module or routes through `remoteEntry.js`, and the shell lazy loads it at runtime when the user navigates to that feature. Angular packages like `@angular/core`, `@angular/router`, and `rxjs` are usually shared as singletons.

The main benefit is independent team ownership and independent deployment. The tradeoff is extra complexity around shared dependencies, routing contracts, communication, styling consistency, testing, and handling remote loading failures. I would use it for large enterprise apps with multiple teams, but for a small app I would prefer a modular Angular monolith with lazy-loaded feature modules.

# Top 20 Angular Interview Questions and Answers

## 1. What Is Angular?

Angular is a TypeScript-based frontend framework used to build single-page applications. It provides a complete platform with components, templates, routing, dependency injection, forms, HTTP communication, testing support, and build tooling.

Interview answer:

> Angular is a full-featured frontend framework for building scalable single-page applications. It uses TypeScript, component-based architecture, dependency injection, routing, forms, and services to organize frontend code in a maintainable way.

## 2. What Are Components in Angular?

A component is the basic building block of an Angular UI.

It contains:

- TypeScript class for logic
- HTML template for view
- CSS/SCSS for styling
- Metadata using `@Component`

Example:

```ts
@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {
  name = 'John';
}
```

Interview answer:

> A component controls a part of the screen. It combines template, logic, styles, and metadata. Angular applications are built as a tree of components.

## 3. What Is the Difference Between Component and Directive?

| Component | Directive |
|---|---|
| Has a template | Usually does not have a template |
| Controls a UI section | Changes behavior or appearance of an element |
| Uses `@Component` | Uses `@Directive` |
| Example: user profile card | Example: highlight text |

Interview answer:

> A component is a directive with a template. Directives are mainly used to change DOM behavior or appearance, while components create actual UI views.

## 4. What Are Angular Directives?

Directives are classes that add behavior to elements in the DOM.

Types:

- **Component directive**: directive with template
- **Structural directive**: changes DOM layout, like `*ngIf`, `*ngFor`
- **Attribute directive**: changes appearance or behavior, like `ngClass`, `ngStyle`

Interview answer:

> Directives allow Angular to attach behavior to DOM elements. Structural directives add or remove elements, while attribute directives modify the behavior or appearance of existing elements.

## 5. What Is Data Binding in Angular?

Data binding connects the component class and template.

Types:

| Binding | Syntax | Purpose |
|---|---|---|
| Interpolation | `{{ value }}` | Component to template text |
| Property binding | `[src]="imageUrl"` | Component to DOM property |
| Event binding | `(click)="save()"` | Template event to component method |
| Two-way binding | `[(ngModel)]="name"` | Sync both directions |

Interview answer:

> Data binding is how Angular synchronizes data between the component and the template. Angular supports interpolation, property binding, event binding, and two-way binding.

## 6. What Is Dependency Injection in Angular?

Dependency Injection is a design pattern where Angular provides required dependencies to a class instead of the class creating them manually.

Example:

```ts
constructor(private userService: UserService) {}
```

Benefits:

- Loose coupling
- Easier testing
- Reusable services
- Centralized object creation

Interview answer:

> Dependency Injection lets Angular inject services or dependencies into components and other services. This improves testability, reusability, and loose coupling.

## 7. What Are Services in Angular?

Services are classes used to share business logic, data, API calls, or reusable functionality across components.

Example:

```ts
@Injectable({
  providedIn: 'root'
})
export class UserService {
  getUsers() {
    return this.http.get('/api/users');
  }
}
```

Interview answer:

> Services are used for reusable logic that should not live inside components, such as API calls, shared state, logging, or business rules.

## 8. What Is `providedIn: 'root'`?

`providedIn: 'root'` registers a service at the application root level.

This means:

- One singleton instance is created.
- The service is available throughout the app.
- Angular can tree-shake unused services.

Interview answer:

> `providedIn: 'root'` makes a service available application-wide as a singleton and also supports tree shaking if the service is not used.

## 9. What Is Angular Module?

An Angular module, or `NgModule`, groups related components, directives, pipes, and services.

Common modules:

- `AppModule`
- Feature modules
- Shared modules
- Core modules

Example:

```ts
@NgModule({
  declarations: [UserComponent],
  imports: [CommonModule],
  exports: [UserComponent]
})
export class UserModule {}
```

Interview answer:

> An Angular module is a container for related features. It helps organize components, directives, pipes, and imported dependencies.

Note:

> In modern Angular, standalone components can reduce the need for NgModules, but many enterprise projects still use modules.

## 10. What Are Standalone Components?

Standalone components are Angular components that do not need to be declared inside an `NgModule`.

Example:

```ts
@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user.component.html'
})
export class UserComponent {}
```

Interview answer:

> Standalone components simplify Angular architecture by allowing components, directives, and pipes to be used without declaring them in an NgModule.

## 11. What Is Angular Routing?

Angular routing allows navigation between different views without reloading the full page.

Example:

```ts
const routes: Routes = [
  { path: 'users', component: UsersComponent },
  { path: 'orders', component: OrdersComponent },
  { path: '', redirectTo: 'users', pathMatch: 'full' }
];
```

Interview answer:

> Angular Router maps URL paths to components. It enables single-page application navigation, lazy loading, route guards, child routes, and route parameters.

## 12. What Is Lazy Loading?

Lazy loading means loading a feature only when it is needed.

Example:

```ts
const routes: Routes = [
  {
    path: 'admin',
    loadChildren: () =>
      import('./admin/admin.module').then(m => m.AdminModule)
  }
];
```

Benefits:

- Smaller initial bundle
- Faster first load
- Better performance for large apps

Interview answer:

> Lazy loading improves performance by loading feature modules or routes only when the user navigates to them, instead of loading the entire application upfront.

## 13. What Are Route Guards?

Route guards control whether navigation is allowed.

Common guards:

| Guard | Purpose |
|---|---|
| `CanActivate` | Can user enter route? |
| `CanActivateChild` | Can user enter child route? |
| `CanDeactivate` | Can user leave route? |
| `Resolve` | Load data before route opens |
| `CanMatch` | Can route be matched/lazy loaded? |

Interview answer:

> Route guards protect routes and control navigation. They are commonly used for authentication, authorization, unsaved form checks, and preloading route data.

## 14. What Is Observable in Angular?

An Observable represents a stream of asynchronous data over time. Angular uses Observables heavily with `HttpClient`, forms, router events, and RxJS.

Example:

```ts
this.userService.getUsers().subscribe(users => {
  this.users = users;
});
```

Interview answer:

> Observables are used to handle asynchronous streams of data. Unlike Promises, they can emit multiple values over time and provide powerful operators through RxJS.

## 15. Observable vs Promise

| Observable | Promise |
|---|---|
| Can emit multiple values | Emits one value |
| Lazy by default | Eager |
| Can be cancelled by unsubscribe | Cannot be cancelled directly |
| Has RxJS operators | Limited chaining |
| Used heavily in Angular | Used in general async JS |

Interview answer:

> A Promise handles a single future value, while an Observable can handle multiple values over time. Observables are lazy, cancellable, and support RxJS operators.

## 16. What Is RxJS?

RxJS is a library for reactive programming using Observables.

Common operators:

| Operator | Use |
|---|---|
| `map` | Transform data |
| `filter` | Filter values |
| `switchMap` | Cancel previous request and switch to new one |
| `mergeMap` | Run inner streams in parallel |
| `concatMap` | Run inner streams sequentially |
| `debounceTime` | Wait before emitting, useful for search |
| `catchError` | Handle errors |

Interview answer:

> RxJS helps manage asynchronous streams in Angular. It is useful for HTTP calls, search boxes, route changes, form changes, and event streams.

## 17. What Is `switchMap` and Why Is It Useful?

`switchMap` maps a value to a new Observable and cancels the previous inner Observable when a new value arrives.

Best example:

```ts
this.searchControl.valueChanges.pipe(
  debounceTime(300),
  switchMap(searchText => this.userService.searchUsers(searchText))
);
```

Interview answer:

> `switchMap` is useful when only the latest request matters, like search autocomplete. It cancels previous pending requests and switches to the latest Observable.

## 18. What Is Change Detection in Angular?

Change detection is Angular's process of checking whether component data changed and updating the DOM.

Angular usually runs change detection after:

- Events
- HTTP responses
- Timers
- Promise resolution
- Observable emissions used with async pipe

Interview answer:

> Change detection keeps the template in sync with component data. Angular checks components and updates the DOM when data changes.

## 19. What Is `OnPush` Change Detection?

`OnPush` tells Angular to check a component only in specific cases.

Angular checks an `OnPush` component when:

- An `@Input` reference changes
- An event occurs inside the component
- An Observable used with `async` pipe emits
- Change detection is manually triggered

Example:

```ts
@Component({
  selector: 'app-user-list',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './user-list.component.html'
})
export class UserListComponent {}
```

Interview answer:

> `OnPush` improves performance by reducing unnecessary checks. It works best with immutable data and Observables with the async pipe.

## 20. What Is the `async` Pipe?

The `async` pipe subscribes to an Observable or Promise in the template and automatically unsubscribes when the component is destroyed.

Example:

```html
<div *ngFor="let user of users$ | async">
  {{ user.name }}
</div>
```

Benefits:

- Cleaner code
- Automatic subscription handling
- Works well with `OnPush`
- Reduces memory leak risk

Interview answer:

> The `async` pipe is used to bind Observables or Promises directly in the template. It automatically subscribes, updates the view, and unsubscribes when the component is destroyed.

## Quick Revision: Angular Must-Remember Points

```text
Component = UI block
Directive = changes DOM behavior
Service = reusable logic
DI = Angular provides dependencies
Routing = URL to component
Lazy loading = load feature only when needed
Guard = control navigation
Observable = async data stream
RxJS = operators for async streams
OnPush = optimized change detection
Async pipe = auto subscribe/unsubscribe
```

## Final Angular Interview Summary

Angular is a complete frontend framework based on TypeScript and component architecture. Components build the UI, services hold reusable logic, dependency injection provides services, routing handles navigation, lazy loading improves performance, guards protect routes, and RxJS Observables manage asynchronous data. For performance, Angular provides features like `OnPush` change detection, lazy loading, trackBy, pure pipes, and the `async` pipe.
