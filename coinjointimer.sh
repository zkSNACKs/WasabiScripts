`#!/usr/bin/env bash`

for ARGUMENT in "$@"
do
	KEY=$(echo $ARGUMENT | cut -f1 -d=)

	KEY_LENGTH=${#KEY}
	VALUE="${ARGUMENT:$KEY_LENGTH+1}"

	export "$KEY"="$VALUE"
done

if [ -z $maxCoinjoinRounds ];
then
	echo "Parameter maxCoinjoinRounds cannot be empty"
	exit 0
fi

minimalInputValue=1

if [ $hours -lt $minimalInputValue ];
then
echo "Error: Value of hour parameter cannot be smaller than 1. Provided value = ($hours)"
exit 0
fi

if [ $maxCoinjoinRounds -lt $minimalInputValue ];
then
echo "Error: Value of maxCoinjoinRounds parameter cannot be smaller than 1. Provided value = ($maxCoinjoinRounds)"
exit 0
fi

# count 80% value of hours
minHours=$(( $hours * 80/100 ))
# count 120% value of hours
maxHours=$(( $hours * 120/100 ))

minSeconds=$(( $minHours * 3600))
maxSeconds=$(( $maxHours * 3600))

secondsToWait=$[$RANDOM % $maxSeconds + $minSeconds]

hoursToWait=$[secondsToWait / 3600]

./wcli.sh selectwallet $walletName

sleep $secondsToWait

function coinjoin() {

./wcli.sh startcoinjoin true true

echo "Coinjoining started"

( tail -f -n0 ~/.walletwasabi/client/Logs.txt & ) | grep -q "Broadcasted. Coinjoin TxId"

./wcli.sh stopcoinjoin

echo "Coinjoin was successful - coinjoining stopped"

}

i=0

while [ $i -lt $maxCoinjoinRounds ]
do
	coinjoin
	
	 ((i++))
    	
    	if [ $i -eq $maxCoinjoinRounds ];
      then
      
      	echo "Last (of $maxCoinjoinRounds) conjoins successfully finished"
      
      exit 0
      fi

	hoursToWait=$[secondsToWait / 3600]

	echo "Waiting for $hoursToWait hours"

	secondsToWait=$[$RANDOM % $maxSeconds + $minSeconds]

	sleep $secondsToWait 

	echo "Ready for next coinjoin"

done
	
