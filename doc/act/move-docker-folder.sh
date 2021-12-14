#!/bin/bash
# https://www.guguweb.com/2019/02/07/how-to-move-docker-data-directory-to-another-location-on-ubuntu/

newDockerDir=$1;
currentDockerDir="/var/lib/docker";
dockerConfigFile="/etc/docker/daemon.json";
searchPattern="^(.*[\"]data-root[\"]:[ \t]*[\"])(.*)([\"].*)$";
replaceDockerConfigLine="\"data-root\": \"$newDockerDir\"";

[ ! -d $newDockerDir ] && echo "Directory '$newDockerDir' DOES NOT exists." && exit 1;

if [ ! -f $dockerConfigFile ]; then

    # Stop docker
    dockerRunning="$(sudo service docker status)"
    if [ "${dockerRunning}" = " * Docker is running" ]; then
        sudo service docker stop;
    fi

    # Build config file
    echo "Creating '$dockerConfigFile' for '$newDockerDir'";
    sudo touch $dockerConfigFile;
    sudo echo "{" >>  $dockerConfigFile;
    sudo echo "$replaceDockerConfigLine" >>  $dockerConfigFile;
    sudo echo "}" >>  $dockerConfigFile;
else
    tmpfile=$(mktemp /tmp/move-docker-folder.XXXXXX)
    exec 3>"$tmpfile"
    exec 4<"$tmpfile"

    # Get the old docker folder name
    currentDockerDir=`sed -E "s|$searchPattern|\2|g" "$dockerConfigFile"`;
    echo "$currentDockerDir" >&3
    currentDockerDir=`sed -n '2{p;q;}' <&4`

    if [ "${newDockerDir}" = "${currentDockerDir}" ]; then
        echo "No change necessary" && exit 0;
    fi

    # Stop docker
    dockerRunning="$(sudo service docker status)"
    if [ "${dockerRunning}" = " * Docker is running" ]; then
        sudo service docker stop;
    fi

    # Replace value config file 
    `sed -i "s|$currentDockerDir|$newDockerDir|g" $dockerConfigFile`;
fi

echo "Moving '$currentDockerDir' to '$newDockerDir'";

# Make sure docker is stopped
while : ; do
    sleep 1
    dockerRunning="$(sudo service docker status)";
    if [ "${dockerRunning}" = " * Docker is not running" ]; then
        break;
    fi
done
    
# Move old folder
sudo rsync -aP $currentDockerDir/* $newDockerDir;
sudo mv -f $currentDockerDir "${currentDockerDir}.old"

# Start and test docker
sudo service docker start;
while : ; do
    sleep 1
    dockerRunning="$(sudo service docker status)";
    if [ "${dockerRunning}" = " * Docker is running" ]; then
        sudo docker run hello-world;
        sudo docker image rm hello-world --force;
        break;
    fi
done

echo "";
echo "If everything works fine and you don't want to keep the old directory run:";
echo "sudo rm -rf ${currentDockerDir}.old";