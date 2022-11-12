`#!/usr/bin/env bash`

./wcli.sh selectwallet $1
./wcli.sh startcoinjoin $1 true true

( tail -f -n0 ~/.walletwasabi/client/Logs.txt & ) | grep -q "Broadcasted. Coinjoin TxId"

./wcli.sh selectwallet $1
./wcli.sh stopcoinjoin

echo "Coinjoin was successful - coinjoining stopped"
