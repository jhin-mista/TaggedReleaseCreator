It seems like you consider contributing to this project. This is very much appreciated. Please read on to find out how we can ensure a good collaboration experience.

## Contributing Code and Content

This repository accepts fixes and features. Here is what you should do when writing code for it:

- Follow the coding conventions used throughout the project
- Add, remove, or delete unit tests to cover your changes. Make sure tests are specific to the changes you are making. Tests need to be provided for every bug/feature that is completed
- Provide the issue number in the commit message, if applicable
- Any code or documentation you share with the project should fall under the projects license agreement

### Identifying the Scale of a Contribution

If you would like to contribute to the project, first identify the scale of what you would like to contribute. If it is small (grammar/spelling or a bug fix), feel free to start working on a fix. If you are submitting a feature or substantial code contribution, please discuss it with us first. 

You might also read these two blogs posts on contributing code: [Open Source Contribution Etiquette](http://tirania.org/blog/archive/2010/Dec-31.html) by Miguel de Icaza and [Don't "Push" Your Pull Requests](https://www.igvita.com/2011/12/19/dont-push-your-pull-requests/) by Ilya Grigorik. These blog posts highlight good open source collaboration etiquette and help align expectations between you and us.

All code submissions will be reviewed and tested, and only those that meet the expected quality and design/roadmap appropriateness will be merged into the project. Fear not though, you will be given plenty of constructive feedback as needed.

### Submitting a Pull Request

If you don't know what a pull request is, read this article: https://help.github.com/articles/using-pull-requests. Make sure the repository can build and all tests pass. It is also a good idea to familiarize yourself with the project workflow and our coding conventions.

Once you have submitted your pull request towards the main branch, a GitHub workflow is triggered that:
- validates the coding style by running `dotnet format`
- builds the solution
- runs all tests

If any of the above steps fail, the pull request cannot be merged and you will need to fix everything. The workflow log should point you in the right direction on what went wrong.

## Attributions

[bUnits contributing file](https://github.com/bUnit-dev/bUnit/blob/c0260b135330ba85c9a3499e0a2474d1ab7a27c4/CONTRIBUTING.md) was used as a template for this document.
