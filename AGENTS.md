## Scope

These instructions apply to `Packages/com.easytoolkit.core`.

Core is the lowest-level EasyToolKit package. Keep it broadly reusable, language-neutral, and independent from
higher-level package behavior. Code added here should be useful to multiple packages or Unity projects, not tailored to
one feature, game, serializer format, logger policy, or inspector workflow.

## Package Boundaries

- Runtime Core must not depend on `com.easytoolkit.inspector`, `com.easytoolkit.logging`, or
  `com.easytoolkit.serialization`.
- Do not introduce package dependencies unless the dependency is genuinely foundational and is reflected in both
  `package.json` and the affected `.asmdef` files.
- Prefer moving neutral shared helpers into Core only when at least two packages can reasonably reuse them.
- Keep logging abstractions, serialization formats, and inspector presentation rules out of Core.
- Keep host-project validation examples in `Assets/`; keep reusable package source in this package.
- Do not manually create or edit Unity `.meta` files.

## Directory Responsibilities

- `Runtime/Foundation`: general-purpose primitives, collection helpers, comparison helpers, text utilities, conversion
  utilities, randomization, math, and IO extensions.
- `Runtime/Unity`: UnityEngine-focused helpers and extensions that are still broadly reusable.
- `Runtime/Reflection`: reflection accessors, invokers, member paths, expression helpers, type analysis, and type
  matching.
- `Runtime/Events`: event buses, bindable values/lists, registration helpers, and event-related primitives.
- `Runtime/Pooling`: object and GameObject pooling contracts, implementations, configuration, and managers.
- `Runtime/Patterns`: reusable pattern implementations such as dependency injection, state machines, and singletons.
- `Runtime/ThirdParty`: vendored or adapted third-party code. Keep attribution and license-sensitive text intact.
- `Editor`: editor-only infrastructure, GUI helpers, handles, editor utilities, windows, and internal editor paths.
- `Tests/Runtime`: Unity Test Framework coverage for runtime-facing Core behavior.

When adding code, extend the closest existing folder and namespace before creating a new area.

## Runtime And Editor Separation

- Never reference `UnityEditor` from `Runtime`.
- Runtime attributes, marker types, or contracts consumed by editor tooling belong in `Runtime`.
- Drawing, serialized property handling, editor windows, editor resources, handles, and IMGUI helpers belong in `Editor`.
- Editor code may depend on Unity editor APIs, but should still remain reusable infrastructure instead of feature-specific
  tooling.
- If a runtime API has both pure .NET and UnityEngine parts, keep the pure helper in `Foundation` and the Unity-specific
  helper in `Unity`.

## Area-Specific Notes

### Foundation

- Keep helpers general and format-agnostic.
- Prefer extension methods only when they read naturally and do not obscure side effects.
- Keep converter utilities neutral. Do not add serializer-specific assumptions here.

### Unity

- Keep Unity helpers broadly useful across projects.
- Avoid project-specific scene, prefab, layer, tag, input, camera, or resource conventions.
- For `MonoBehaviour` utilities, document lifecycle assumptions and avoid surprising object creation or destruction.

### Reflection

- Prefer existing accessor, invoker, path, analyzer, and matcher patterns before adding a new reflection mechanism.
- Cache reflection work only when cache keys and invalidation behavior are clear.
- Keep expression parsing and member path behavior covered by representative tests.
- Be careful with open generics, generic parameter constraints, inherited members, overloaded members, and value-type
  accessors.

### Events

- Keep event contracts simple and allocation-conscious.
- Avoid coupling event primitives to Unity scene lifecycles unless the type is explicitly Unity-specific.
- Preserve subscription disposal and unregistration behavior. Add tests when changing ordering, priority, or lifetime.

### Pooling

- Keep pool contracts independent from concrete managers where practical.
- Preserve lifecycle callbacks and object state transitions when changing pooling behavior.
- For GameObject pools, make ownership, activation, parent transform, and destruction behavior explicit.

### Patterns

- Keep pattern implementations reusable and opt-in.
- For singleton or registry changes, make initialization mode, teardown, duplicate handling, and Unity domain reload behavior
  clear.
- For dependency injection changes, avoid service-locator sprawl in unrelated modules.

### Editor

- Prefer existing `EasyEditorGUI`, scope, drag-and-drop, handle, and utility patterns.
- Keep editor helpers infrastructure-oriented. Inspector package-specific drawers and visual processors belong in
  `com.easytoolkit.inspector`.
- Keep editor-only serialized property and GUI behavior out of runtime contracts unless the runtime contract is a lightweight
  marker or configuration model.

### ThirdParty

- Do not reformat third-party code for style-only reasons.
- Preserve license headers, attribution, and upstream structure unless a deliberate vendored patch requires otherwise.
- Keep local patches small and documented.

## Tests And Verification

- Add or update package-local tests under `Tests/Runtime` for runtime behavior changes.
- Prefer focused tests for changed helpers, especially reflection, pooling, events, and text/conversion utilities.
- Cover both success and failure cases for public APIs with validation or lookup behavior.
- For package dependency or `.asmdef` changes, verify the affected assemblies compile in Unity.
- Check `git -C Packages/com.easytoolkit.core status --short` when summarizing work, because this package is a submodule.
