[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="https://github.com/Marvin-Brouwer/FluentSerializer/raw/main/docs/logo/Logo.default.optimized.svg" />
	<a href="https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/docs/help/Maintaining.md#readme">
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

- [Marvin Brouwer](https://github.com/Marvin-Brouwer)

## Verifying pull-requests

It's important that you inspect pull-requests thoroughly. If you can, please pull down the branch and inspect yourself.
Have a look at the benchmarking results and make sure no large performance degradation is added.

Pull-requests need at least these requirements:

- An issue has to be created prior to PR
- The branch has to adhere to the [branching guidelines](https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/docs/help/Contributing.md#pull-requests)
  - Correct source branch
  - Correct target branch
  - Correct branch naming
  - Etc.
- All tests have to pass
- Performance cannot degrade by too much of a margin  
  This is pretty subjective but still important.
- Additional dependencies have to be cleared by the entire team of maintainers.

## Release management

Release management is pretty straight forward.  
There is always a `release` branch and a [milestone](https://github.com/Marvin-Brouwer/FluentSerializer/milestones) containing issues for an upcoming release.

Once the release is ready, packages need to be deployed.  
Currently this works with a [manual deploy pipeline](https://github.com/Marvin-Brouwer/FluentSerializer/actions/workflows/manual-deployment.yml).  
Just select the package containing the changes and the [semantic version](https://semver.org/) you'd like the next package to be.  
Please make sure you adhere to the [SemVer Guidelines](https://semver.org/), ask the team of maintainers for help if you have questions.

Every release has an owner appointed at the start, who is responsible for versioning and publishing.

After releasing, merge the `release` branch into the `main` branch.
