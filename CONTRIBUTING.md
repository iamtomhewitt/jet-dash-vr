# Contributing 🎉

## Issues

Are tracked under the [Issues](https://github.com/iamtomhewitt/jet-dash-vr/issues) tab. 

### Creating An Issue

If you are creating an issue, give the issue a clear title and a descriptive comment on what went wrong / how something can be improved. Use the relevant tags and add it to the [Issue Management Project](https://github.com/iamtomhewitt/jet-dash-vr/projects/1) so the issues can be tracked effectively.

### Fixing An Issue

If you are fixing an issue, branch off of the `develop` branch to fix it. Name the branch after the issue number, so if you were fixing Issue number 15, create a branch called `Issue#15`. Don't forget to change the issue in [Issue Management](https://github.com/iamtomhewitt/jet-dash-vr/projects/1) to `In Progress`.

### Commit Messages

Although not enforced, consider starting the commit message with an applicable emoji:
- 🎨 `:art:` when making a visual change that the player would see
- 💡 `:light bulb:` when adding a new feature
- 🐛 `:bug:` when fixing a bug in the code 
- ⚙️ `:gear:` when doing something with a build
- 🔧 `:wrench:` when fixing something to do with git
- 💎 `:gem stone:` when refactoring code 
- 🏷 `:label tag:` when tagging in the `beta` or `master` branch (pull requests)

## Pull Request Process

When you have fixed an issue on your issue branch, create a pull request to merge your changes into the `develop` branch. We'll take a look and merge it if all is good!

## Betas & Production Builds

### Branches
When a Beta is ready to be made, a Beta version of the app (`.apk`) will be made off of the develop branch, and uploaded into the Beta on Google Play. When that has been done successfully, and the Beta has gone live, the changes from `develop` will be merged into the `beta` branch.

When a production app release is ready, the Beta version in Google Play will be promoted from Beta to Production. When the Production version has gone live, the changes in the `beta` branch will be merged into the `master` branch.

### Tags

Tags should be created against the version of the `.apk` file. For beta releases, tag the version with a suffix of `-BETA` on the `beta` branch (e.g. `2.3.1-BETA`), and tag just the version on the master branch for production releases (e.g. `2.3.1`).