# BootStrapper

A desktop application for bootstrapping Unity projects from reusable, composable templates.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/language-C%23-239120?style=flat&logo=csharp&logoColor=white)
![Avalonia](https://img.shields.io/badge/UI-Avalonia-2C2C54?style=flat)
![xUnit](https://img.shields.io/badge/tests-xUnit-5D4F85?style=flat)
![GitHub Actions](https://img.shields.io/badge/CI%2FCD-GitHub%20Actions-2088FF?style=flat&logo=githubactions&logoColor=white)
![Velopack](https://img.shields.io/badge/installer-Velopack-orange?style=flat)
![License](https://img.shields.io/badge/license-MIT-yellow.svg)

## Status

This project is feature complete for its initial scope (v1). It was built primarily as a
hands on learning exercise in desktop application architecture with C# and .NET, so it may
not receive frequent updates going forward.

## Overview

BootStrapper lets you create reusable templates for common Unity systems (networking, camera, state machines, Steam integration, and more) and combine several of them into a
single new Unity project at creation time. Instead of a single closed "project snapshot"template, BootStrapper is built around composing independent, versioned systems together.

## Features

- Create, edit and delete reusable script templates, each tied to one or more Unity versions
- Preview a template's folder and file structure before using it
- Combine multiple templates into a single new Unity project
- Detect installed Unity Editor versions automatically and create or open projects through them
- Browse both official and personal templates from the same interface
- Fully local and file based: no server or account required to use the app
- Configurable paths for projects, templates and the Unity Editor installation

## Architecture

BootStrapper follows the MVVM pattern and is split into three projects:

- `BootStrapper` the Avalonia UI project (Views, ViewModels, navigation)
- `BootStrapper.Core` framework agnostic business logic (models, services)
- `BootStrapper.Tests` unit tests for the Core project

Templates and projects are persisted as JSON manifests on the file system rather than in a
database. This keeps the tool fully local by default and avoids maintaining two sources of
truth for the same data. SQLite and EF Core were evaluated during development but dropped
for this reason; that experience is earmarked for a future templates backend instead.

## Tech Stack

- Avalonia UI + CommunityToolkit.Mvvm for a cross platform desktop UI following MVVM
- System.Text.Json for manifest based persistence
- xUnit for unit tests
- Velopack for installer packaging and auto updates
- GitHub Actions for CI/CD

## Getting Started

### Prerequisites

- .NET SDK 10 or newer
- Unity Hub with at least one Unity Editor version installed

### Build and run

```bash
git clone https://github.com/Komo404/BootStrapper.git
cd BootStrapper
dotnet build
dotnet run --project BootStrapper
```

## Project Structure

```
BootStrapper/
  BootStrapper/ # Avalonia UI project
  BootStrapper.Core/ # Business logic, models and services
  BootStrapper.Tests/ # Unit tests
```

## Contributing

Issue and pull request templates are available under `.github/`. Contributions are welcome,
but please open an issue first for larger changes.

## License

This project is licensed under the MIT License. See [LICENSE](./LICENSE) for details.