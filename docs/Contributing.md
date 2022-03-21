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
<sub><i>

This guide is based on [github.com/roots/guidelines/CONTRIBUTING.md](https://github.com/roots/guidelines/blob/master/CONTRIBUTING.md).

</i></sub>

[//]: # (Body)

Please take a moment to review this document in order to make the contribution process easy and effective for everyone involved.

Following these guidelines helps to communicate that you respect the time of
the developers managing and developing this open source project. In return,
they should reciprocate that respect in addressing your issue or assessing
patches and features.

## Using the issue tracker

The issue tracker is the preferred channel for [bug reports](#bug-reports), [features requests](#feature-requests) and [submitting pull requests](#pull-requests), but please respect the following restrictions:

* Please **do not** derail or troll issues. Keep the discussion on topic and
  respect the opinions of others.

## Bug reports

A bug is a _demonstrable problem_ that is caused by the code in the repository.
Good bug reports are extremely helpful - thank you!

Guidelines for bug reports:

1. **Use the GitHub issue search** check if the issue has already been reported.

2. **Check if the issue has been fixed** try to reproduce it using the latest `master` or development branch in the repository.

3. **Isolate the problem** make sure that the code in the repository is _definitely_ responsible for the issue.

A good bug report shouldn't leave others needing to chase you up for more
information. Please try to be as detailed as possible in your report.

## Feature requests

Feature requests are welcome. But take a moment to find out whether your idea fits with the scope and aims of the project.  
It's up to *you* to make a strong case to convince the developers of the merits of this feature. 
Please provide as much detail and context as possible.

## Pull requests

Good pull requests - patches, improvements, new features - are a fantastic help. They should remain focused in scope and avoid containing unrelated commits.

**Please ask first** before embarking on any significant pull request (e.g. implementing features, refactoring code), otherwise you risk spending a lot of time working on something that the developers might not want to merge into the project.

Please adhere to the coding conventions used throughout the project (indentation, comments, etc.).

Adhering to the following this process is the best way to get your work merged:

1. [Fork](http://help.github.com/fork-a-repo/) the repo, clone your fork,
   and configure the remotes:

   ```bash
   # Clone your fork of the repo into the current directory
   git clone https://github.com/<your-username>/<repo-name>
   # Navigate to the newly cloned directory
   cd <repo-name>
   # Assign the original repo to a remote called "upstream"
   git remote add upstream https://github.com/<upsteam-owner>/<repo-name>
   ```

2. If you cloned a while ago, get the latest changes from upstream:

   ```bash
   git checkout release/<your release milestone>
   git pull upstream release/<your release milestone>
   ```

3. Create a new topic branch (off the main project `release/*` branch) to contain your feature, change, or fix:

   ```bash
   git checkout -b feature/<issue-number>-<friendly-description>
   ```

   You should pick a `release/*` branch based on the [milestone](/milestones) your issue is in.  
   Once you base, pull in, rebase from a `release/*` branch, you cannot merge into another `release/*` branch.

   It's probably easiest to use the title of the issue as the `<friendly description>`.
   We don't distinguish between features and buts at the moment, everything is a feature.  
  
4. Commit your changes in logical chunks.  
   Please adhere to these [git commit message guidelines](http://tbaggery.com/2008/04/19/a-note-about-git-commit-messages.html) or your code is unlikely be merged into the main project.  
   Please Proceed all your commit messages with `<issue-number>: `, you will see older commit messages with `#<issue-number> ` but that turned out to be difficult with merge conflict messages and `git commit --amend`.  
   Use Git's [interactive rebase](https://help.github.com/articles/interactive-rebase) feature to tidy up your commits before making them public.

5. Locally merge (or rebase) the upstream `release/*` branch into your topic branch:

   ```bash
   git pull [--rebase] upstream release/<your release milestone>
   ```

6. Push your topic branch up to your fork:

   ```bash
   git push origin feature/<issue-number>-<friendly-description>
   ```

7. [Open a Pull Request](https://help.github.com/articles/using-pull-requests/) with a clear title and description.
   The target branch should be the active `release/*` branch matching your issues [milestone](/milestones).  
   **Never** merge directly into `main`, unless you're instructed to do so by a maintainer.  
   Please just use automatic merge and don't squash. We value the merge history.
