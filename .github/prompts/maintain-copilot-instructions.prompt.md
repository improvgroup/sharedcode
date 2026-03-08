---
mode: edit
description: Review and update the Copilot configuration files to keep them aligned with the current codebase.
---

# Maintain Copilot Instructions

Review and update all Copilot configuration files so they stay accurate as the codebase evolves.
Run this prompt after adding new modules, changing conventions, or updating the toolchain.

## Files to Review

### `.github/copilot-instructions.md`

- [ ] The module table lists every project currently in `SharedCode.sln`
      (run `grep '\.csproj' SharedCode.sln` to get the current list)
- [ ] Build and test commands are still correct
- [ ] The C# language version and target framework match `Directory.Build.props`
- [ ] Namespace conventions match the source files
- [ ] New patterns or base classes that have emerged are documented
- [ ] Analyzer packages and their roles match `Directory.Build.props`
- [ ] Testing framework details match `SharedCode.Core.Tests/Core.Tests.csproj`

### `.github/prompts/*.prompt.md`

- [ ] `add-extension-method.prompt.md` — namespace table is complete and templates compile
- [ ] `add-specification.prompt.md` — builder API still matches `ISpecificationBuilder<T>`
- [ ] `fix-code-analysis.prompt.md` — rule table covers the analyzers actually in use
- [ ] `create-module.prompt.md` — scaffolding steps still match the solution structure
- [ ] Each prompt's `description` front-matter field is accurate

### `.vscode/mcp.json`

- [ ] Listed MCP servers are still current and useful
- [ ] Docker image tags or npx package versions are up to date
- [ ] Any new MCP servers that would improve development tasks should be added

## Process

1. Open `SharedCode.sln` and list all current projects.
2. Open `Directory.Build.props` and `Directory.Packages.props` to confirm the current SDK
   targets, language version, and package versions.
3. Spot-check two or three recently changed source files for new patterns or conventions.
4. Update each file listed above as needed, keeping changes minimal and accurate.
5. Run `dotnet build SharedCode.sln` to confirm no build errors were introduced.
6. Commit with the message: `chore: update copilot instructions`.
