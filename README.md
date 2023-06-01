# Wasabi Scripts
Useful scripts for the [Wasabi Wallet](https://github.com/zksnacks/walletwasabi) client.

## coinjointimer
To randomize the time that Wasabi starts and stops to coinjoin.
#### Prerequisites
`wcli` has to be in the same folder.
#### How to use
`./coinjointimer.sh walletName="WALLET" hours=12 maxCoinjoinRounds=6`

`walletName` is the wallets name, `password` is the wallets password, `hours` is wait time period randomized by +-20%, `maxCoinjoinRounds` the number of coinjoins that should happen. In the above example, one coinjoin should happen every 12 hours, and in total 6 coinjoins, so the ceremony should be concluded in 72 hours.

`Example of hours argument usage`: `hours=100` - first and every other conjoin will start between `80-120`hours.
Value of hours between coinjoins is generated each time separately, so wait hour time for first coinjoin in this example should be randomly generated let say to `115` hours, for second coinjoin `81` for third `97` and so on.

## 1cj
Immediately start to coinjoin and stop after one successful round.
#### Prerequisites
`wcli` has to be in the same folder.
#### How to use
`./1cj.sh WALLET`
