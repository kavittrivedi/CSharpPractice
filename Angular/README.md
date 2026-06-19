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
