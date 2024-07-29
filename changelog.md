# Changelog

## [v0.4.2](https://github.com/devlooped/dotnet-trx/tree/v0.4.2) (2024-07-29)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/v0.4.1...v0.4.2)

:bug: Fixed bugs:

- Escape markup in CLI output from errors too [\#33](https://github.com/devlooped/dotnet-trx/pull/33) (@kzu)

## [v0.4.1](https://github.com/devlooped/dotnet-trx/tree/v0.4.1) (2024-07-17)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/v0.4.0...v0.4.1)

:sparkles: Implemented enhancements:

- Auto-document CLI options [\#25](https://github.com/devlooped/dotnet-trx/pull/25) (@kzu)

:bug: Fixed bugs:

- Ensure we don't render style for default option value [\#27](https://github.com/devlooped/dotnet-trx/pull/27) (@kzu)

## [v0.4.0](https://github.com/devlooped/dotnet-trx/tree/v0.4.0) (2024-07-15)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/v0.3.4...v0.4.0)

:sparkles: Implemented enhancements:

- Exit code should match success/failure status [\#24](https://github.com/devlooped/dotnet-trx/pull/24) (@kzu)

## [v0.3.4](https://github.com/devlooped/dotnet-trx/tree/v0.3.4) (2024-07-12)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/v0.3.3...v0.3.4)

:sparkles: Implemented enhancements:

- Add options to disable GH comment and summary reporting [\#23](https://github.com/devlooped/dotnet-trx/pull/23) (@kzu)
- Fix notices that can contain multiple lines [\#22](https://github.com/devlooped/dotnet-trx/pull/22) (@kzu)
- Link badges directly to job run for details [\#20](https://github.com/devlooped/dotnet-trx/pull/20) (@kzu)
- Add support for reusing the same PR comment for badges/details [\#19](https://github.com/devlooped/dotnet-trx/pull/19) (@kzu)

:bug: Fixed bugs:

- Error: Could not find color or style 'System.Int32'. [\#18](https://github.com/devlooped/dotnet-trx/issues/18)
- Make sure we escape markup when rendering formatted [\#21](https://github.com/devlooped/dotnet-trx/pull/21) (@kzu)

## [v0.3.3](https://github.com/devlooped/dotnet-trx/tree/v0.3.3) (2024-07-07)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/v0.3.2...v0.3.3)

## [v0.3.2](https://github.com/devlooped/dotnet-trx/tree/v0.3.2) (2024-07-07)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/v0.3.1...v0.3.2)

:sparkles: Implemented enhancements:

- Make macOS version description more palatable [\#17](https://github.com/devlooped/dotnet-trx/pull/17) (@kzu)

## [v0.3.1](https://github.com/devlooped/dotnet-trx/tree/v0.3.1) (2024-07-07)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/v0.3.0...v0.3.1)

:sparkles: Implemented enhancements:

- Add runtime information to GH message [\#16](https://github.com/devlooped/dotnet-trx/pull/16) (@kzu)

## [v0.3.0](https://github.com/devlooped/dotnet-trx/tree/v0.3.0) (2024-07-04)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/v0.2.0...v0.3.0)

:sparkles: Implemented enhancements:

- Add --no-skip to avoid rendering skipped tests? [\#3](https://github.com/devlooped/dotnet-trx/issues/3)
- Render badges in GH using shields.io [\#13](https://github.com/devlooped/dotnet-trx/pull/13) (@kzu)
- Sort test results by name, improve duration calculation [\#12](https://github.com/devlooped/dotnet-trx/pull/12) (@kzu)
- Add support for reporting to github [\#11](https://github.com/devlooped/dotnet-trx/pull/11) (@kzu)

:hammer: Other:

- If there are multiple trx files, only render the results for the newest one for a given assembly [\#2](https://github.com/devlooped/dotnet-trx/issues/2)

## [v0.2.0](https://github.com/devlooped/dotnet-trx/tree/v0.2.0) (2024-07-03)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/v0.1.0...v0.2.0)

:sparkles: Implemented enhancements:

- Render all results for all files found as a single summary [\#6](https://github.com/devlooped/dotnet-trx/issues/6)
- Render file links and relative paths [\#10](https://github.com/devlooped/dotnet-trx/pull/10) (@kzu)
- Add --skipped \(default true\) support [\#9](https://github.com/devlooped/dotnet-trx/pull/9) (@kzu)
- Render skipped test reason [\#8](https://github.com/devlooped/dotnet-trx/pull/8) (@kzu)
- Render all results for all files found as a single summary [\#7](https://github.com/devlooped/dotnet-trx/pull/7) (@kzu)
- Dim skipped tests by default [\#4](https://github.com/devlooped/dotnet-trx/pull/4) (@kzu)

## [v0.1.0](https://github.com/devlooped/dotnet-trx/tree/v0.1.0) (2024-07-02)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/v0.1.0-beta...v0.1.0)

## [v0.1.0-beta](https://github.com/devlooped/dotnet-trx/tree/v0.1.0-beta) (2024-07-02)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/v0.1.0-alpha...v0.1.0-beta)

## [v0.1.0-alpha](https://github.com/devlooped/dotnet-trx/tree/v0.1.0-alpha) (2024-07-02)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/da76f91bbb92492066d851ef07b833bb6618a8db...v0.1.0-alpha)

:sparkles: Implemented enhancements:

- Add support for rendering test output and errors [\#1](https://github.com/devlooped/dotnet-trx/pull/1) (@kzu)



\* *This Changelog was automatically generated by [github_changelog_generator](https://github.com/github-changelog-generator/github-changelog-generator)*
