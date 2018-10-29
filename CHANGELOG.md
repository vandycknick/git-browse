# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [v0.2.0] 2018-10-29
### Added
- Support overriding protocol via config value eg `git config --add browse.protocol http`
- Support overriding domain via config value eg `git config --add browse.domain something.com`
- Create urls for non master branches when being tracked

## [v0.1.0] - 2018-10-28
### Added
- Initial implementation to open git remote url in default browser.
- Support tool on Windows, Linux, Linux WSL
