# Changelog

## 1.1.0 - 2026-11-07

### Added

- Windows x64 installer and portable package, and Linux x64 AppImage distribution.

### Changed

- Use Unity CLI to detect installed Editors and create and open Unity projects, replacing direct Editor executable discovery and launch.
- **Unity CLI is now required:** install it separately and make sure the `unity` command is available on `PATH`, with at least one Unity Editor installed.
- Load installed Editor versions asynchronously, with loading, empty-list, and error messages in project and template screens.
- Store project manifests in the Unity project root instead of a separate BootStrapper projects directory.
- Use a single configurable destination for Unity projects and centralize application settings in the user's local application data directory.
- Remove the Unity Editor installation path setting; Editor discovery is handled by Unity CLI.
- Update the automatic updater to use the current BootStrapper release repository.

### Fixed

- Preserve script selection when removing files or folders from a template's preview tree.
- Refresh the template tree after successfully deleting files from disk.
- Copy only selected scripts, including files at the source root, into each supported Unity version directory.
- Write template and project manifests only after script copying succeeds, preventing incomplete operations from being registered as complete.
- Add timeouts and cancellation support to Unity CLI operations and report command failures.
- Use the GitHub Releases update source for automatic updates.

### Internal

- Extract script tree operations and organize filesystem and manifest helpers.
- Add ZIP bundling support for template directories in the Core service.
- Add regression tests covering selected-file copying, version directories, project registration, and removal from the project list.

## 1.0.0

- Initial release with reusable Unity templates, project creation, local JSON manifests, and Velopack packaging and automatic updates.
