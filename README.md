# Wasabi Scripts
Useful scripts for the [Wasabi Wallet](https://github.com/zksnacks/walletwasabi) client

## wcli
##### Prerequisites
`chmod u+x wcli.sh`
##### How to use
Available sub-commands: `getstatus, createwallet, listunspentcoins, getwalletinfo, getnewaddress, send, gethistory, listkeys, stop, startcoinjoin, stopcoinjoin` [See the [RPC Documentation](https://docs.wasabiwallet.io/using-wasabi/RPC.html)]
`./wcli.sh getstatus`

## coinsgraph
##### Prerequisites
These packages need to be installed: `bsdmainutils`, `graphviz`, `feh`, `jq`, `curl`
##### How to use
```
./wcli selectwallet <wallet-name>
./wcli listcoins > cointlist.txt
dotnet fsi coinsgraph.fsx | sed -e 's/    ;//' | dot -Tpng | feh  -
```
