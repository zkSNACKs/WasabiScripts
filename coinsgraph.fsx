#if INTERACTIVE
#r "nuget:FSharp.Data"
#endif 

open System
open System.IO
open System.Net.Http
open FSharp.Data

type Config = JsonProvider<"""{
  "JsonRpcServerEnabled": true,
  "JsonRpcUser": "",
  "JsonRpcPassword": "",
  "JsonRpcServerPrefixes": [
    "http://127.0.0.1:37128/"
  ]
}""">

type RpcResponse = JsonProvider<"""{
  "result": [
     {"txid":"73af1dd","index":0,"amount":2390000,"anonymitySet":"a1.0","confirmed":true,"confirmations":116,"keyPath":"84/0","address":"tb1q","spentBy":"2d7c3f"}
  ]
}""">

let config = Config.Load(
  Path.Combine (
    Environment.ExpandEnvironmentVariables ("%HOME%/.walletwasabi/client/"),
    "Config.json"))

let args = Environment.GetCommandLineArgs()
let walletname = args[2]
let firstTxId = args[3] 

let http = new HttpClient()
let rpcJsonResponseAsync () = async {
  let content = new StringContent ($"{{\"jsonrpc\":\"2.0\", \"id\":\"id\", \"method\":\"selectwallet\", \"params\":[\"{walletname}\"]}}")
  let! response = http.PostAsync(config.JsonRpcServerPrefixes[0], content) |> Async.AwaitTask

  let content = new StringContent ("{\"jsonrpc\":\"2.0\", \"id\":\"id\", \"method\":\"listcoins\"}")
  let! response = http.PostAsync(config.JsonRpcServerPrefixes[0], content) |> Async.AwaitTask
  let! jsonResult = response.Content.ReadAsStringAsync() |> Async.AwaitTask
  return RpcResponse.Parse(jsonResult)
} 

let rpcJsonResult = rpcJsonResponseAsync () |> Async.RunSynchronously

let coins = 
    rpcJsonResult.Result 
    |> Array.skipWhile (fun x -> x.Txid <> firstTxId)

let coinsGroupedByTx =
    coins
    |> Array.groupBy (fun x -> x.Txid)

let graphNodes = 
    coinsGroupedByTx
    |> Seq.map ( fun (txid, outs) ->
      let indexes = outs |> Array.map (fun x -> string x.Index) |> String.concat "|"
      let paths = outs |> Array.map (fun x -> x.KeyPath) |> String.concat "|"
      let anonsets = outs |> Array.map (fun x -> x.AnonymitySet) |> String.concat "|"
      let amounts = outs |> Array.map (fun x -> $"<idx{x.Index}>{x.Amount}") |> String.concat "|"
      let addresses = outs |> Array.map (fun x -> x.Address) |> String.concat "|" 
      $"tx{txid} [label=\"Tx Id: {txid[..8]}|{{{{ {{ {{ PrvScore | {anonsets} }} | {{ Path | {paths} }} | {{ Idx | {indexes} }} | {{ Address | {addresses} }} |  {{ Amount | {amounts} }} }}}} }}}}\"]"
    )
    |> List.ofSeq
        
let graphEdges =
    coins 
    |> Array.filter (fun x -> x.SpentBy <> "")
    |> Array.map (fun x -> $"tx{x.Txid}:idx{x.Index} -> tx{x.SpentBy}")
    |> List.ofArray

let allLines =
    graphNodes @ graphEdges
    |> String.concat ";\n"

let str = $"""
digraph G {{
graph [center=1 rankdir=LR; overlap=false; splines=true;];
edge [dir=forward];
node [shape=record; ordering="in" ];
{allLines}
}}"""

Console.WriteLine(str)

