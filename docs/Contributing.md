[//]: # (Links)

[github-contribution-guide]: https://github.com/roots/guidelines/blob/master/CONTRIBUTING.md
[github-flow]: http://scottchacon.com/2011/08/31/github-flow.html

[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="/docs/logo/Logo.default.optimized.svg" />
	<a href="/docs/help/Contributing.md#readme">
		Documentation: Contribution guide
	</a>
</h1>

_This guide is based on [GitHub's example contribution guide][github-contribution-guide] 
and [Scott Chacon's GitHub flow article][github-flow]._  

<br/>

[//]: # (Body)

Please take a moment to review this document in order to make the contribution process easy and effective for everyone involved.

Following these guidelines helps to communicate that you respect the time of the developers managing and developing this open  source project.
In return, they should reciprocate that respect in addressing your issue or assessing patches and features.

## Using the issue tracker  
  
The issue tracker is the preferred channel for [bug reports](#bug-reports), [features requests](#feature-requests) and [submitting pull requests](#pull-requests), but please respect the following restrictions:  
  
* Please **do not** derail or troll issues.
* Keep the discussion on topic and respect the opinions of others.

## Bug reports  
  
A bug is a _demonstrable problem_ that is caused by the code in the repository.
Good bug reports are extremely helpful - thank you!

Guidelines for bug reports:

1. **Use the GitHub issue search**  
   Check if the issue has already been reported.
2. **Check if the issue has been fixed**  
   Try to reproduce it using the latest `master` or development branch in the repository.
3. **Isolate the problem**  
   Make sure that the code in the repository is _definitely_ responsible for the issue.

A good bug report shouldn't leave others needing to chase you up for more
information. Please try to be as detailed as possible in your report.

## Feature requests  
  
Feature requests are welcome. But take a moment to find out whether your idea fits with the scope and aims of the project.  
It's up to *you* to make a strong case to convince the developers of the merits of this feature.  
Please provide as much detail and context as possible.

## Contribution flow  
  
The contribution flow is loosely based on [Scott Chacon's GitHub flow article][github-flow].
Following this process is the best way to get your work merged:

**Please ask first** before embarking on any significant code change (e.g. implementing features, refactoring code), otherwise you risk spending a lot of time working on something that the developers might not want to merge into the project.

### Anything in the master branch is deployable  
  
Make sure your code is deploy ready.
Everything is thoroughly tested by unit-tests and code inspection.
However, if you're not sure you can and should deploy an alpha version and manually test before merging.

### Create descriptive branches off of master  
  
<details>
   <summary>(Optional) Fork</summary>
   <br/>

[Fork](http://help.github.com/fork-a-repo/) the repo, clone your fork, and configure the remotes:

```bash
# Clone your fork of the repo into the current directory
git clone https://github.com/<your-username>/<repo-name>
# Navigate to the newly cloned directory
cd <repo-name>
# Assign the original repo to a remote called "upstream"
git remote add upstream https://github.com/<upsteam-owner>/<repo-name>
```

**If you cloned a while ago,** get the latest changes from upstream:

```bash
git checkout main
git pull upstream main
```

</details>
  
Create a new topic branch (off the `main` branch) to contain your feature, change, or fix:

```bash
git checkout -b <issue-number>-<friendly-description>
```

_It's probably easiest to use the title of the issue as the `<friendly description>`._

_**Or** (if you're not forked)_ you can use the GitHub UI:

<details>
	<summary>Using the GitHub UI</summary>

![Create a branch form an issue](./images/github-create-branch.png)  
![Create a branch form an issue wizard](./images/github-create-branch-wizard.png)  

</details>  

### Push to named branches constantly  
  
Commit your changes in logical chunks, make your change's intent clear in the message.  

Please adhere to these [git commit message guidelines](http://tbaggery.com/2008/04/19/a-note-about-git-commit-messages.html) or your code is unlikely be merged into the main project.  
Please prepend all your commit messages with `[#<issue-number>]<space>`;  
You will see older commit messages but that turned out to be difficult with merge conflict messages and `git commit --amend` or it didn't link easily.  

Push often, make sure other contributors can see your progress.

Regularly rebase the upstream `main` branch into your topic branch, preferably before pushing:

   ```bash
   git pull --rebase upstream main
   ```

Use Git's [interactive rebase](https://help.github.com/articles/interactive-rebase) feature to tidy up your commits before making them public.

### Open a pull request at any time  

[dod]: /.github/pull_request_template.md#definition-of-done
  
Good pull requests - patches, improvements, new features - are a fantastic help.  
They should remain focused in scope and avoid containing unrelated commits.

Your changes do not have to be ready to open a pull request.  
However if you don't intend it to be merged, please state so in your PR.  
If you do intend it to be merged, please use the auto-merge functionality.  
Please don't squash, we care about the merge history.  

[Open a Pull Request](https://help.github.com/articles/using-pull-requests/) with a clear title and description.
The target branch should be the `main` branch unless you're instructed to use a different branch by a maintainer.  
When finalizing a PR for merging all items of the [DOD][dod] have to be checked, this [DOD][dod] will be included in the PR by default.  
You're not allowed to remove items. However, feel free to add any checks if applicable.

#### Final pull request  
  
When finalizing your change please make sure you:

* State it's meant for merging
* Turn on auto merge
* Add release notes to the `Changelog.md` in the projects you changed, with a link to the GitHub issue.
  Make sure it's in the `## @next` chapter so it get's added to the next package publish automagically.
  Eg. `- [#150](https://github.com/Marvin-Brouwer/FluentSerializer/issues/150) Added release notes`
* Check all the items in the [DOD][dod] that's included in the PR template.

### Release management  
  
Once your feature is merged a maintainer will take over.  
A deploy will be scheduled.  
See: [Maintaining: Release management](./Maintaining.md#release-management)
