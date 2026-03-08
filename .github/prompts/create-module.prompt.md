---
mode: edit
description: Scaffold a new SharedCode project that publishes as a separate NuGet package.
---

# Create a New SharedCode Module

Add a new project to the SharedCode solution that will be published as its own NuGet package.

## Steps

### 1 — Create the project folder

```
SharedCode.<ModuleName>/
```

### 2 — Create the `.csproj` file

All shared MSBuild settings come from `Directory.Build.props`; only module-specific values
belong here.

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <AssemblyName>SharedCode.<ModuleName></AssemblyName>
    <Description>A library of [short description] shared for free use to help with common scenarios.</Description>
    <PackageTags>shared code, c#, [relevant tags]</PackageTags>
    <RootNamespace>SharedCode.<ModuleName></RootNamespace>
    <TargetFramework>net9.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <!-- Add <PackageReference> or <ProjectReference> entries here.
         Versions are managed in Directory.Packages.props — do NOT specify Version here. -->
  </ItemGroup>

  <ItemGroup>
    <None Include="README.md" Pack="true" PackagePath="\" />
    <None Include="share.ico" />
    <None Include="share.png">
      <Pack>True</Pack>
      <PackagePath></PackagePath>
    </None>
  </ItemGroup>
</Project>
```

### 3 — Copy shared assets

Copy `share.ico` and `share.png` from any existing module into the new folder.

### 4 — Create `README.md`

```markdown
# Shared Code <ModuleName> Library

A library of code shared for free use to help with common scenarios.

To use this, search NuGet for SharedCode.<ModuleName>
```

### 5 — Add to the solution

```bash
dotnet sln SharedCode.sln add SharedCode.<ModuleName>/SharedCode.<ModuleName>.csproj
```

### 6 — Add new package versions (if needed)

If the module depends on new NuGet packages, add `<PackageVersion>` entries to
`Directory.Packages.props`:

```xml
<PackageVersion Include="Some.New.Package" Version="x.y.z" />
```

Then reference the package in the `.csproj` **without** a version attribute:

```xml
<PackageReference Include="Some.New.Package" />
```

### 7 — Update documentation

- Add the new package to the table in the root `README.md`.
- Add a row to the module table in `.github/copilot-instructions.md`.

### 8 — Verify the build

```bash
dotnet build SharedCode.sln
```

Zero warnings are required before merging.
