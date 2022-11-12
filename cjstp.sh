`#!/usr/bin/env bash`

( tail -f -n0 ~/.walletwasabi/client/Logs.txt & ) | grep -q "Broadcasted. Coinjoin TxId"

./wcli.sh selectwallet $1
./wcli.sh stopcoinjoin

echo "Coinjoin was successful - coinjoining stopped"
