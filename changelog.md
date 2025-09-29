# Changelog

## [v1.0.0-beta](https://github.com/devlooped/dotnet-trx/tree/v1.0.0-beta) (2025-09-29)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/v0.5.2...v1.0.0-beta)

:sparkles: Implemented enhancements:

- Include className in output [\#14](https://github.com/devlooped/dotnet-trx/issues/14)
- When writing GH comment summary, include error message [\#90](https://github.com/devlooped/dotnet-trx/pull/90) (@kzu)

:bug: Fixed bugs:

- ErrorInfo.Message not displayed [\#85](https://github.com/devlooped/dotnet-trx/issues/85)

:hammer: Other:

- Request dotnet 9 compability [\#82](https://github.com/devlooped/dotnet-trx/issues/82)
- Remove dependency to Humanizer [\#79](https://github.com/devlooped/dotnet-trx/issues/79)

## [v0.5.2](https://github.com/devlooped/dotnet-trx/tree/v0.5.2) (2025-03-02)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/v0.5.1...v0.5.2)

:twisted_rightwards_arrows: Merged:

- Add mention to @devlooped for visibility [\#81](https://github.com/devlooped/dotnet-trx/pull/81) (@kzu)

## [v0.5.1](https://github.com/devlooped/dotnet-trx/tree/v0.5.1) (2025-02-18)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/v0.5.0...v0.5.1)

:twisted_rightwards_arrows: Merged:

- Remove confusing duplicate -v for version [\#76](https://github.com/devlooped/dotnet-trx/pull/76) (@kzu)

## [v0.5.0](https://github.com/devlooped/dotnet-trx/tree/v0.5.0) (2025-02-18)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/v0.4.5...v0.5.0)

:sparkles: Implemented enhancements:

- Quieter output by default [\#73](https://github.com/devlooped/dotnet-trx/pull/73) (@kzu)
- Improve path validation and reporting markup escaping [\#66](https://github.com/devlooped/dotnet-trx/pull/66) (@kzu)
- Always append summary even if file doesn't exist [\#42](https://github.com/devlooped/dotnet-trx/pull/42) (@kzu)

:bug: Fixed bugs:

- \[Ignore\] attribute on test causes "Object reference not set to an instance of an object" exception [\#65](https://github.com/devlooped/dotnet-trx/issues/65)
- Sample action for github [\#64](https://github.com/devlooped/dotnet-trx/issues/64)
- System.InvalidOperationException: Unbalanced markup stack [\#63](https://github.com/devlooped/dotnet-trx/issues/63)
- trx -v does not produce version info [\#58](https://github.com/devlooped/dotnet-trx/issues/58)
- Error: Object reference not set to an instance of an object. [\#57](https://github.com/devlooped/dotnet-trx/issues/57)
- Don't fail for non-executed tests which have no `duration` value [\#71](https://github.com/devlooped/dotnet-trx/pull/71) (@kzu)
- Render version with -v too [\#70](https://github.com/devlooped/dotnet-trx/pull/70) (@kzu)

:hammer: Other:

- Add flag to report only failed tests [\#59](https://github.com/devlooped/dotnet-trx/issues/59)

:twisted_rightwards_arrows: Merged:

- Ignore launch profile when generating help markdown [\#72](https://github.com/devlooped/dotnet-trx/pull/72) (@kzu)
- Remove previous workaround for styles [\#47](https://github.com/devlooped/dotnet-trx/pull/47) (@kzu)
- Improve style disable when NO\_COLOR [\#44](https://github.com/devlooped/dotnet-trx/pull/44) (@kzu)

## [v0.4.5](https://github.com/devlooped/dotnet-trx/tree/v0.4.5) (2024-08-08)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/v0.4.4...v0.4.5)

:sparkles: Implemented enhancements:

- Fix whitespace in elapsed time badge [\#37](https://github.com/devlooped/dotnet-trx/pull/37) (@kzu)

## [v0.4.4](https://github.com/devlooped/dotnet-trx/tree/v0.4.4) (2024-07-31)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/v0.4.3...v0.4.4)

:sparkles: Implemented enhancements:

- Fix markup style error if test contains \[ or \] [\#36](https://github.com/devlooped/dotnet-trx/pull/36) (@kzu)
- Render trx version in GH comment [\#35](https://github.com/devlooped/dotnet-trx/pull/35) (@kzu)

## [v0.4.3](https://github.com/devlooped/dotnet-trx/tree/v0.4.3) (2024-07-29)

[Full Changelog](https://github.com/devlooped/dotnet-trx/compare/v0.4.2...v0.4.3)

:sparkles: Implemented enhancements:

- Restore formatting and links for errors, escape per line [\#34](https://github.com/devlooped/dotnet-trx/pull/34) (@kzu)

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
