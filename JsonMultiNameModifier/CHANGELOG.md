# JsonMultiNameModifier

## 1.0.2.2

- Dropped the target for `.NET 7`
- Added the target for `.NET 9`
- Bumped `System.Text.Json` from `8.0.0` to `9.0.0`.

## 1.0.2.1

- Added support for `.NET 8`.
- Bumped `System.Text.Json` from `7.0.2` to `8.0.0`.

## 1.0.2

- Fixed #4: `JsonMultiNameModifier` ignored the custom JsonConverter.

## 1.0.1

- Fixed #1: `JsonMultiNameModifier` couldn't be used on non-public properties/fields.
- Added documentation comments.

## 1.0.0 - Initial Release

- Created `JsonPropertyNamesAttribute` attribute.
- Created `JsonMultiNameModifier` modifier.
