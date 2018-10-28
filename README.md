# git-browse

[![Build Status](https://dev.azure.com/vandycknick/git-browse/_apis/build/status/nickvdyck.git-browse)](https://dev.azure.com/vandycknick/git-browse/_build/latest?definitionId=3)

Git sub-command to open the current repository website (GitHub, GitLab, VSTS, ...) in your browser.

## Usage
The tool works as a git sub-command so you can just type `git browse` anywhere in your terminal to try it out.


`git browse --help` will show you usage information.
```
Usage: git-browse [options]

Options:
  --version      Show version information
```

### Examples

```sh
$ git browse
opens https://[SOURCE_CONTROL_PROVIDER/[TRACKED_REMOTE_USER]/[CURRENT_REPO]/tree/[URRENT_BRANCH]
```

### Supported platforms
This is a cross platform tool and thus should run on Linux, Windows, and Mac OSX.
Although given I have limited resources the tool is only actively tested on the following platforms:
- Windows
- WSL (Ubuntu 18.04)
- Ubuntu 18.04
- Ubuntu 16.04

## Installation

Download the [2.1.300](https://www.microsoft.com/net/download/windows) .NET Core SDK or newer.
Once installed, running the following  to install the application:

```sh
dotnet tool install --global git-browse
```

Or use the following when upgrading from a previous version:

```sh
dotnet tool update --global git-browse
```

You will need to perform the following steps when not running on windows:

### Ubuntu 18.0

```sh
sudo apt install libcurl3
```
