# Setup act locally  
  
This is a guide to  make you able to run the github actions locally.  
  
# Windows  
  
Use the wsl2 for windows.  
Make sure you [enable wsl in windows](https://pureinfotech.com/install-windows-subsystem-linux-2-windows-10/) (the article is for 10 but it probably works for 11 too).  
  
```txt
wsl --set-default-version 2
wsl --install -d Ubuntu-20.04
wsl --set-default Ubuntu-20.04
```
  
Switch to Ubuntu by starting wsl.
Either call `wsl` in the `cmd` prompt or install [remote-wsl-for-vscode](https://code.visualstudio.com/blogs/2019/09/03/wsl2) and run it through the integrated terminal.  
  
# Installing  
  
These are the steps necessary to make act run on Ubuntu.
  
## Installing docker
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
For example `sudo sh ./doc/act/move-docker-folder.sh /mnt/d/docker-images`.  
**Note:** You are responsible for ensuring the drive and folder exist!

