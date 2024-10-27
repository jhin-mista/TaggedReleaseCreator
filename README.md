# ReleaseCreator

[![CI pipeline](https://github.com/jhin-mista/ReleaseCreator/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/jhin-mista/ReleaseCreator/actions/workflows/ci.yml)

This action creates a [GitHub release](https://docs.github.com/en/repositories/releasing-projects-on-github/about-releases) and automatically increases the [semantic version](https://semver.org) tag. It does this by finding the highest semantic version in the remote branch the action runs on and increasing it based on given inputs.

## Contents
1. [Detailed incrementation behaviour](#detailed-version-increase-behaviour)
1. [Inputs](#inputs)
1. [Outputs](#outputs)
1. [Usage](#usage)

## Detailed version increase behaviour
A semantic version can be increased in several ways based on the type of increase (major, minor, patch) and if it is a pre-release. The following tables aim to provide a detailed overview on how this action increases a semantic version based on the latest release/version tag and action inputs.

### Next stable release
| latest tag | increase type | pre-release identifier | new tag |
|--------|--------|--------|--------|
| `1.2.3` | major | `ignored` | `2.0.0` |
| `1.2.3` | minor | `ignored` | `1.3.0` |
| `1.2.3` | patch | `ignored` | `1.2.4` |

### Pre-release from stable release
| latest tag | increase type | pre-release identifier | new tag |
|--------|--------|--------|--------|
| `1.2.3` | major | alpha | `2.0.0-alpha.1` |
| `1.2.3` | minor | alpha | `1.3.0-alpha.1` |
| `1.2.3` | patch | alpha | `1.2.4-alpha.1` |
| `1.2.3` | major | `no argument` | `2.0.0-1` |
| `1.2.3` | minor | `no argument` | `1.3.0-1` |
| `1.2.3` | patch | `no argument` | `1.2.4-1` |

### Stable release from pre-release
| latest tag | increase type | pre-release identifier | new tag |
|--------|--------|--------|--------|
| `1.2.3-alpha.1` | `ignored` | `ignored` | `1.2.3` | 

### Next pre-release
| latest tag | increase type | pre-release identifier | new tag |
|--------|--------|--------|--------|
| `1.2.3-alpha.1` | `ignored` | `no argument` | `1.2.3-alpha.2` | 
| `1.2.3-alpha.1` | `ignored` | alpha | `1.2.3-alpha.2` |
| `1.2.3-alpha.2` | `ignored` | beta | `1.2.3-beta.1` | 
| `1.2.3-alpha` | `ignored` | `no argument` | `1.2.3-alpha.1` |
| `1.2.3-alpha` | `ignored` | alpha | `1.2.3-alpha.1` |
| `1.2.3-alpha` | `ignored` | beta | `1.2.3-beta.1` |
| `1.2.3-1` | `ignored` | `no argument`| `1.2.3-2` |
| `1.2.3-2` | `ignored` | alpha | `1.2.3-alpha.1` |

## Inputs

### `commit` - required
The commit SHA to add the release/version tag to.

### `type` - required
The type of release/version increment. Can be one of the following values:
- `Major`
- `Minor`
- `Patch`

### `token` - required
The authentication token used to create the release in GitHub. Can be the `GITHUB_TOKEN`. Needs the `contents: write` permissions set.

### `pre-release`
Boolean value indicating if the release to be created should be a pre-release. The default value is `false`

### `pre-release-identifier`
An optional pre-release identifier that will be prepended to the pre-release number e.g. `2.0.0-identifier.1`

### `log-level`
Specifies the log level for the action. Can be one of the following values:
- `Trace`
- `Debug`
- `Information`
- `Warning`
- `Error`
- `Critical`
- `None`

The default log level is `Information`.

## Outputs

### next-version
The tag name that has been computed by the action and was used to create the release.

## Used environment variables
- `GITHUB_REPOSITORY_ID` to create the release for the repository the action is called from
- `GITHUB_OUTPUT` to set the `next-version` output

## Usage

### As a manually triggered workflow
```yaml
on:
    workflow_dispatch: 
        inputs:
            release-type:
                description: The type of release
                type: choice
                required: true
                options:
                    - Major
                    - Minor
                    - Patch
            is-pre-release:
                description: Indicates if this is a pre-release
                type: boolean
                required: true
                default: false
            pre-release-identifier:
                description: An optional pre-release identifier
                type: string
                required: false
                default: ""

jobs:
    create-release:
        runs-on: ubuntu-latest
        permissions:
            contents: write
        steps:
            - name: Checkout
              uses: actions/checkout@v4

            - name: Call release creator action
              id: release-creator
              uses: jhin-mista/releasecreator
              with:
                commit: ${{ github.sha }} # This will be the HEAD of the branch the workflow runs on
                type: ${{ inputs.release-type }}
                token: ${{ secrets.GITHUB_TOKEN }}
                pre-release: ${{ inputs.is-pre-release }}
                pre-release-identifier: ${{ inputs.pre-release-identifier }}
```