# GitIgnoreThis 🛡️

[![NuGet](https://img.shields.io/nuget/v/GitIgnoreThis.svg?style=flat-square)](https://www.nuget.org/packages/GitIgnoreThis/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/GitIgnoreThis.svg?style=flat-square)](https://www.nuget.org/packages/GitIgnoreThis/)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg?style=flat-square)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-blue.svg?style=flat-square)](https://dotnet.microsoft.com/)

**GitIgnoreThis** is a lightweight, interactive Terminal UI (TUI) and CLI tool to instantly manage and append files, directories, and wildcard patterns to `.gitignore`.

Stop opening `.gitignore` manually just to paste a path. Let **GitIgnoreThis** do it for you!

---

## ⚡ Features

- **Interactive TUI Mode**: Launch without arguments to get an interactive checkbox menu of untracked files and folders.
- **Fast CLI Mode**: Pass files or wildcards directly via arguments.
- **Smart Git Root Detection**: Automatically resolves the repository root even if executed deep inside subdirectories.
- **Clean Path Normalization**: Replaces backslashes with standard forward slashes (`/`) and handles trailing slashes for directories.
- **Duplicate Prevention**: Automatically detects already ignored items and skips duplicates.
- **Auto-creation**: Creates `.gitignore` automatically if it doesn't exist yet.

---

## 📦 Installation

Install **GitIgnoreThis** globally via .NET CLI:

```bash
dotnet tool install --global GitIgnoreThis