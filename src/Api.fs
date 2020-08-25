[<RequireQualifiedAccess>]
module Api

let rnd = System.Random()

let generateRandom = async {
  do! Async.Sleep 1000
  let randomNumber = rnd.Next()
  return randomNumber
}