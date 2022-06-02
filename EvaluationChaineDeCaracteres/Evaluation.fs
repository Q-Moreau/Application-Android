(*Evaluation String*)


module Page.Evaluation





let rec findParanthesis (listOfChar : char list) =
    let rec subFunction listOfChar paranthesisLevel =
        match listOfChar with
        | [] -> failwith "no paranthesis"
        | ')'::tail when paranthesisLevel = 0 -> ([],tail)
        | '('::tail -> let (expression, newTail) = (subFunction tail (paranthesisLevel+1)) in ('('::expression, newTail)
        | ')'::tail -> let (expression, newTail) = (subFunction tail (paranthesisLevel-1)) in (')'::expression, newTail)
        | head::tail -> let (expression, newTail) = (subFunction tail paranthesisLevel) in (head::expression, newTail)
    in subFunction listOfChar 0;;

let rec findDecimalPartNumber (listOfChar : char list) =
    let rec subFunction listOfChar number =
        match listOfChar with
        | digit::tail when ((int digit) - (int '0') >=0) && ((int digit) - (int '0') <=9)
                -> let (number,list) = subFunction tail (10.*number + (float digit) - (float '0')) in (number/10.,list)
        | _ -> (number,listOfChar)
    in subFunction listOfChar 0.;;

let rec findNumber (listOfChar : char list) =
    let rec subFunction listOfChar number =
        match listOfChar with
        | '.'::tail -> let (decimalPartNumber,newTail) = findDecimalPartNumber tail in
                        (number+decimalPartNumber,newTail)
        | digit::tail when ((int digit) - (int '0') >=0) && ((int digit) - (int '0') <=9)
                -> subFunction tail (10.*number + (float digit) - (float '0'))
        | _ -> (number,listOfChar)
    in subFunction listOfChar 0.;;

let rec evaluateCharList (listOfChar : char list) =
    let rec subFunction listOfChar value =
        match listOfChar with
        | [] -> value
        | '+'::tail -> value + (subFunction tail 0.)
        | '-'::tail -> value - (subFunction tail 0.)
        | '*'::tail -> value * (subFunction tail 0.)
        | '/'::tail -> value / (subFunction tail 0.)
        | '('::tail -> let (expression, newTail) = (findParanthesis tail) in subFunction newTail (subFunction expression 0.)
        | ')'::tail -> value
        | _ -> let (number,newTail) = (findNumber listOfChar) in subFunction newTail number
    in subFunction listOfChar 0.;;


let evaluateString stringExpression = evaluateCharList (Seq.toList stringExpression);;





//let rec summationCharList (listOfChar : char list) =
//    let rec subFunction listOfChar value =
//        match listOfChar with
//        | [] -> value
//        | '+'::tail -> value + (subFunction tail 0.)
//        | '-'::tail -> value - (subFunction tail 0.)
//        | '*'::tail -> value * (subFunction tail 0.)
//        | '/'::tail -> value / (subFunction tail 0.)
//        | '('::tail -> let (expression, newTail) = (findParanthesis tail) in subFunction newTail (subFunction expression 0.)
//        | ')'::tail -> value
//        | _ -> let (number,newTail) = (findNumber listOfChar) in subFunction newTail number
//    in subFunction listOfChar 0.;;


//let rec productCharList (listOfChar : char list) =
//    let rec subFunction listOfChar value =
//        match listOfChar with
//        | [] -> value
//        | '+'::tail -> value + (subFunction tail 0.)
//        | '-'::tail -> value - (subFunction tail 0.)
//        | '*'::tail -> value * (subFunction tail 0.)
//        | '/'::tail -> value / (subFunction tail 0.)
//        | '('::tail -> let (expression, newTail) = (findParanthesis tail) in subFunction newTail (subFunction expression 0.)
//        | ')'::tail -> value
//        | _ -> let (number,newTail) = (findNumber listOfChar) in subFunction newTail number
//    in subFunction listOfChar 0.;;