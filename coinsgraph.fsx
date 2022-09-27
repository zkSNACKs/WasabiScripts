open System
open System.IO

type Row = { tx: string; idx: int; amount:int64; anonset:int; confirmed:bool; confirmations:int; address:string; spentBy:string; path:string}
let toGraph (tx: string * Row list) : string =
  let txid, outs = tx
  let indexes = outs |> List.map (fun x -> string x.idx) |> String.concat "|"
  let paths   = outs |> List.map (fun x -> x.path) |> String.concat "|"
  let anonsets = outs |> List.map (fun x -> string x.anonset) |> String.concat "|"
  let addresses = outs |> List.map (fun x -> x.address) |> String.concat "|"
  let amounts = outs |> List.map (fun x -> $"<idx{x.idx}>{x.amount}") |> String.concat "|"
  let outputs = $"{{ {{ Index | {indexes} }} | {{ Path | {paths} }} | {{ AnonSet | {anonsets} }} | {{ Address | {addresses} }} | {{ Amount | {amounts} }} }}"
  let edges   = outs |> List.filter (fun x -> x.spentBy <> "") |> List.map (fun x -> $"tx{x.tx}:idx{x.idx} -> tx{x.spentBy}") |> String.concat ";\n    "
  $"tx{txid} [label=\"Tx Id: {txid}|{{{{ {outputs} }} }}}}\"];\n    {edges}"

let tableArray = 
  fun _ -> Console.ReadLine()
  |> Seq.initInfinite
  |> Seq.takeWhile ((<>) null)
  |> List.ofSeq
  |> List.tail 
  |> List.map (fun l -> l.Split(" ", StringSplitOptions.RemoveEmptyEntries))
  |> List.map (fun e -> { tx = e[0]; idx = int e[1]; amount = int64 e[2]; anonset = int e[3]; confirmed = (if e[4] = "true" then true else false); confirmations = int e[5]; address = e[7]; path = e[6]; spentBy = (if e.Length = 9 then e[8] else "") })
  |> List.groupBy (fun x -> x.tx) 
  |> List.map toGraph 

let str = $"""
digraph G {{
    graph [center=1 rankdir=LR; overlap=false; splines=true;];
    edge [dir=forward];
    node [shape=record; ordering="in" ];
    {tableArray |> String.concat ";\n    "};
}}"""

Console.WriteLine(str)open System
open System.IO

type Row = { tx: string; idx: int; amount:int64; anonset:int; confirmed:bool; confirmations:int; address:string; spentBy:string; path:string}
let toGraph (tx: string * Row list) : string =
  let txid, outs = tx
  let indexes = outs |> List.map (fun x -> string x.idx) |> String.concat "|"
  let paths   = outs |> List.map (fun x -> x.path) |> String.concat "|"
  let anonsets = outs |> List.map (fun x -> string x.anonset) |> String.concat "|"
  let addresses = outs |> List.map (fun x -> x.address) |> String.concat "|"
  let amounts = outs |> List.map (fun x -> $"<idx{x.idx}>{x.amount}") |> String.concat "|"
  let outputs = $"{{ {{ Index | {indexes} }} | {{ Path | {paths} }} | {{ AnonSet | {anonsets} }} | {{ Address | {addresses} }} | {{ Amount | {amounts} }} }}"
  let edges   = outs |> List.filter (fun x -> x.spentBy <> "") |> List.map (fun x -> $"tx{x.tx}:idx{x.idx} -> tx{x.spentBy}") |> String.concat ";\n    "
  $"tx{txid} [label=\"Tx Id: {txid}|{{{{ {outputs} }} }}}}\"];\n    {edges}"

let tableArray = 
  fun _ -> Console.ReadLine()
  |> Seq.initInfinite
  |> Seq.takeWhile ((<>) null)
  |> List.ofSeq
  |> List.tail 
  |> List.map (fun l -> l.Split(" ", StringSplitOptions.RemoveEmptyEntries))
  |> List.map (fun e -> { tx = e[0]; idx = int e[1]; amount = int64 e[2]; anonset = int e[3]; confirmed = (if e[4] = "true" then true else false); confirmations = int e[5]; address = e[7]; path = e[6]; spentBy = (if e.Length = 9 then e[8] else "") })
  |> List.groupBy (fun x -> x.tx) 
  |> List.map toGraph 

let str = $"""
digraph G {{
    graph [center=1 rankdir=LR; overlap=false; splines=true;];
    edge [dir=forward];
    node [shape=record; ordering="in" ];
    {tableArray |> String.concat ";\n    "};
}}"""

Console.WriteLine(str)
