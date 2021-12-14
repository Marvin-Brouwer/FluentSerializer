# Setup act locally  
  
This is a guide to  make you able to run the github actions locally.  
  
# WSl  
  
To make it a consistent experience for everyone this guide uses `wsl2` for windows users.  
Make sure you [enable wsl in windows](https://pureinfotech.com/install-windows-subsystem-linux-2-windows-10/) (the article is for 10 but it probably works for 11 too).  
  
```txt
wsl --set-default-version 2
wsl --install -d Ubuntu-20.04
wsl --set-default Ubuntu-20.04
```
  
Switch to Ubuntu by starting wsl.
Either call `wsl` in the `cmd` prompt or install [remote-wsl-for-vscode](https://code.visualstudio.com/blogs/2019/09/03/wsl2) and run it through the integrated terminal.  
  
# Installing on Ubuntu  
  
These are the steps necessary to make act run on `Ubuntu`.
  
## Installing docker  
  
_**Note:** The scripts in this guide are to use at your own risk._

Follow the steps of https://docs.docker.com/engine/install/ubuntu/ or run the **interactive** install script:
```txt
sudo chmod +rwx ./doc/act/install-docker.sh
sh ./doc/act/install-docker.sh
```
If you'd like to move your docker container directory you can use this **interactive** script:
```txt
sudo chmod +rwx ./doc/act/move-docker-folder.sh
sh ./doc/act/move-docker-folder.sh [FOLDERNAME]
```
For example `sudo sh ./doc/act/move-docker-folder.sh /mnt/d/docker`.  
**Note:** You are responsible for ensuring the drive and folder exist!
  
Make sure you have permissions to run docker:  
```txt
sudo addgroup --system docker
sudo adduser $USER docker
newgrp docker
```

## Installing Act  
  
Installing `act` is pretty straightforward, either follow this guide.  
Or if you don't want to use brew, see the readme here: https://github.com/nektos/act#installation.  
  
### Installing brew

If you already have brew, you can skip to the next step.   
```txt
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
```
Run the generated command to install brew to your path.  
Install the `build-essential` package
``txt
sudo apt-get install build-essential
```
### Installing the Act application  
  
Next install the `act` application:  
```txt
brew install act
```
To validate act is installed correctly run:  
```txt
act -n
```
  
# Running Act  
  
To actually test your pipeline you can use the following command because we setup the CI pipeline for workflow_dispatch:  
```txt
act workflow_dispatch
```
**Keep in mind that** this will download a full ubuntu server image for the pipeline runner, so make sure you have enough disk space.  
This is **_especially_** important if you're running through `wsl` as having no more disk space during the download may destroy your linux image in the wsl.  