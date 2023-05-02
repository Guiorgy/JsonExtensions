# SingleOrArrayJsonConverter

## 1.0.1

- `SingleOrArrayJsonConverter<TValue>` now accepts a custom JsonConverter for `TValue`, deduces the array element type at runtime, and returns a `SingleOrArrayJsonConverter<TValue, TJsonConverter>`.
- Added documentation comments.

## 1.0.0 - Initial Release

- Created `SingleOrArrayJsonConverter<TValue>` where `TValue` is the array element type.
- Created `SingleOrArrayJsonConverter` which deduces the element type at runtime and return a `SingleOrArrayJsonConverter<TValue>`.
- Created `SingleOrArrayJsonConverter<TValue, TJsonConverter>` where `TValue` is the array element type, and `TJsonConverter` is the custom converter for the `TValue` type.
