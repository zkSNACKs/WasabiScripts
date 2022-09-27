#!/usr/bin/env bash

function config_extract() {
    echo $(cat ~/.walletwasabi/client/Config.json | jq -r "$1")
}

ENABLED="$(config_extract '.JsonRpcServerEnabled')"
CREDENTIALS=$(config_extract '.JsonRpcUser + ":" + .JsonRpcPassword')
ENDPOINT=$(config_extract '.JsonRpcServerPrefixes[0]')

if [[ "$ENABLED" == "false" ]]; then
    echo "RPC server is disabled. Make sure to enable it by setting \"JsonRpcEnabled\":\"true\" in the Config.json file."
    quit
fi

if [[ "$CREDENTIALS" == ":" ]]; then
    BASIC_AUTH=""
else
    BASIC_AUTH="--user ${CREDENTIALS}"
fi


METHOD=$1
shift

if [ $# -ge 1 ]; then
    if [[ "$1" ]]; then
        PARAMS="\"$1\""
        shift
    else
        PARAMS="\"\""
        shift
    fi

    while (( "$#" )); do
        if [[ "$1" ]]; then
            PARAMS="$PARAMS, $1"
        else
            PARAMS='$PARAMS, ""'
        fi
        shift
    done
fi

REQUEST="{\"jsonrpc\":\"2.0\", \"id\":\"curltext\", \"method\":\"$METHOD\", \"params\":[$PARAMS]}"
RESULT=$(curl -s $BASIC_AUTH --data-binary "$REQUEST" -H "content-type: text/plain;" $ENDPOINT)
RESULT_ERROR=$(echo $RESULT | jq -r .error)

rawprint=(help)

if [[ "$RESULT_ERROR" == "null" ]]; then
    if [[ " ${rawprint[@]} " =~ " ${METHOD} " ]]; then
       echo $RESULT | jq -r .result
    else
        IS_ARRAY=$(echo $RESULT | jq -r '.result | if type=="array" then "true" else "false" end')
        if [[ "$IS_ARRAY" == "true" ]]; then
           echo $RESULT | jq -r '.result | [.[]| with_entries( .key |= ascii_downcase ) ]
                                         |    (.[0] |keys_unsorted | @tsv)
                                            , (.[]|.|map(.) |@tsv)' | column -t
        else
           echo $RESULT | jq  .result
        fi
    fi
else
   echo $RESULT_ERROR | jq -r .message
fi
