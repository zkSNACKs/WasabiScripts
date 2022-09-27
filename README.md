# Wasabi Scripts
Useful scripts for the [Wasabi Wallet](https://github.com/zksnacks/walletwasabi) client.

## wcli
wcli is a human usable command line interface, it uses the Wasabi RPC server.
#### Prerequisites
`chmod u+x wcli.sh`
#### How to use
All Wasabi RPC commands are available: `getstatus`, `createwallet`, `listunspentcoins`, `getwalletinfo`, `getnewaddress`, `send`, `gethistory`, `listkeys`, `startcoinjoin`, `stopcoinjoin`, `stop` [See the [RPC Documentation](https://docs.wasabiwallet.io/using-wasabi/RPC.html)]

Examples: `./wcli.sh getstatus` `./wcli.sh startcoinjoin true true`


## coinsgraph
A visualization of the wallets local transaction graph.
#### Prerequisites
These packages need to be installed: `wcli` `bsdmainutils`, `graphviz`, `feh`, `jq`, `curl`
#### How to use
```
./wcli selectwallet <wallet-name>
./wcli listcoins > cointlist.txt
dotnet fsi coinsgraph.fsx | sed -e 's/    ;//' | dot -Tpng | feh  -
```

## coinjointimer
To randomize the time that Wasabi starts and stops to coinjoin.
#### Prerequisites
`wcli` has to be in the same folder.
#### How to use
`./coinjointimer.sh walletName="WALLET" password="PASSWORD" hours=12 maxCoinjoinRounds=6`

`walletName` is the wallets name, `password` is the wallets password, `hours` the time in which one coinjoin should happen, `maxCoinjoinRounds` the number of coinjoins that should happen. In the above example, one coinjoin should happen every 12 hours, and in total 6 coinjoins, so the ceremony should be concluded in 72 hours.
