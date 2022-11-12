`#!/usr/bin/env bash`

for ARGUMENT in "$@"
do
	KEY=$(echo $ARGUMENT | cut -f1 -d=)

	KEY_LENGTH=${#KEY}
	VALUE="${ARGUMENT:$KEY_LENGTH+1}"

	export "$KEY"="$VALUE"
done

( tail -f -n0 ~/.walletwasabi/client/Logs.txt & ) | grep -q "Broadcasted. Coinjoin TxId"

./wcli.sh selectwallet $wallet
./wcli.sh stopcoinjoin

echo "Coinjoin was successful - coinjoining stopped"
