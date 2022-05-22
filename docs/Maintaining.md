[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="/docs/logo/Logo.default.optimized.svg" />
	<a href="/docs/help/Maintaining.md#readme">
		Documentation: Maintenance guide
	</a>
</h1>

[//]: # (Body)

Please take a moment to review this document in order to make the maintenance process easy and effective for everyone involved.

Following these guidelines helps to communicate that you respect the time of
the developers managing and developing this open source project. In return,
they should reciprocate that respect in addressing your issue or assessing
patches and features.

## Current Maintainers

- [**Marvin-Brouwer** Marvin Brouwer](https://github.com/Marvin-Brouwer)

## Verifying pull-requests

It's important that you inspect pull-requests thoroughly. If you can, please pull down the branch and inspect yourself.
Have a look at the benchmarking results and make sure no large performance degradation is added.

Pull-requests need at least these requirements:

- An issue has to be created prior to PR
- The branch has to adhere to the [branching guidelines](/docs/help/Contributing.md#pull-requests)
  - Correct source branch
  - Correct target branch
  - Correct branch naming
  - Etc.
- All tests have to pass
- Performance cannot degrade by too much of a margin  
  This is pretty subjective but still important.
- Additional dependencies have to be cleared by the entire team of maintainers.
- Changelogs have to be updated.

## Release management

Release management is pretty straight forward.  
After a merge, or set of merges we need to release.  
  
Maintainers are responsible for releases and should coordinate weekly if there are changes to be released.  
If yes, a maintainer will be appointed `release coordinator` and will be responsible for the release.  
  
The release coordinator will be responsible for determining which packages need to be pushed and calculating the correct [semantic version](https://semver.org/) for each package.  
Currently this works with a [manual deploy pipeline](https://github.com/Marvin-Brouwer/FluentSerializer/actions/workflows/manual-deployment.yml).  
Just select the package containing the changes and the [semantic version](https://semver.org/) you'd like the next package to be.  
Please make sure you adhere to the [SemVer Guidelines](https://semver.org/), ask the team of maintainers for help if you have questions.
  
The release coordinator will also be responsible for moving the `## @next` chapters to the released versions after the release has been pushed completely so new change logs can be put in the  `## @next` fot a following release.  
> _Currently there is only one maintainer, so the need for the weekly coordination is not required._
