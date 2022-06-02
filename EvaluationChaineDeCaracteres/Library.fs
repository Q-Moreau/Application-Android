namespace EvaluationChaineDeCaracteres



(*findParanthesis returns the expression between paranthesis and the tail after paranthesis*)
module findParanthesis =
    let rec findParanthesis (listOfChar : char list) =
        let rec subFunction listOfChar paranthesisLevel = (*paranthesis level controls inner paranthesis*)
            match listOfChar with
            | [] -> failwith "no paranthesis"
            | ')'::tail when paranthesisLevel = 0 -> ([],tail)
            | '('::tail -> let (expression, newTail) = (subFunction tail (paranthesisLevel+1)) in ('('::expression, newTail)
            | ')'::tail -> let (expression, newTail) = (subFunction tail (paranthesisLevel-1)) in (')'::expression, newTail)
            | head::tail -> let (expression, newTail) = (subFunction tail paranthesisLevel) in (head::expression, newTail)
        in subFunction listOfChar 0;;

(*return the decimal part of a number : 50.61 -> 0.61*)
module findDecimalPartNumber =
    let rec findDecimalPartNumber (listOfChar : char list) =
        let rec subFunction listOfChar number =
            match listOfChar with
            | digit::tail when ((int digit) - (int '0') >=0) && ((int digit) - (int '0') <=9)
                    -> let (number,list) = subFunction tail (10.*number + (float digit) - (float '0')) in (number/10.,list)
            | _ -> (number,listOfChar)
        in subFunction listOfChar 0.;;

(*findNumber returns the number registered in the list*)
module findNumber =
    let rec findNumber (listOfChar : char list) =
        let rec subFunction listOfChar number =
            match listOfChar with
            | '.'::tail -> let (decimalPartNumber,newTail) = findDecimalPartNumber.findDecimalPartNumber tail in
                            (number+decimalPartNumber,newTail)
            | digit::tail when ((int digit) - (int '0') >=0) && ((int digit) - (int '0') <=9)
                    -> subFunction tail (10.*number + (float digit) - (float '0'))
            | _ -> (number,listOfChar)
        in subFunction listOfChar 0.;;


(*evaluateCharList returns the number calculated based on list*)
module evaluateCharList =
    let rec evaluateCharList (listOfChar : char list) =
        let rec subFunction listOfChar value =
            match listOfChar with
            | [] -> value
            | '+'::tail -> value + (subFunction tail 0.)
            | '-'::tail -> begin match tail with
                | '('::tail2 -> let (expression, newTail) = (findParanthesis.findParanthesis tail2) in value+(subFunction newTail (-(subFunction expression 0.)))
                | _ -> let (number,newTail) = (findNumber.findNumber tail) in value+(subFunction newTail (-number))
                end
            | '*'::tail -> begin match tail with
                | '('::tail2 -> let (expression, newTail) = (findParanthesis.findParanthesis tail2) in subFunction newTail (value*(subFunction expression 0.))
                | _ -> let (number,newTail) = (findNumber.findNumber tail) in subFunction newTail (value*number)
                end
            | '/'::tail -> begin match tail with
                | '('::tail2 -> let (expression, newTail) = (findParanthesis.findParanthesis tail2) in subFunction newTail (value/(subFunction expression 0.))
                | _ -> let (number,newTail) = (findNumber.findNumber tail) in subFunction newTail (value/number)
                end
            | '('::tail -> let (expression, newTail) = (findParanthesis.findParanthesis tail) in subFunction newTail (subFunction expression 0.)
            | ')'::tail -> value
            | _ -> let (number,newTail) = (findNumber.findNumber listOfChar) in subFunction newTail number
        in subFunction listOfChar 0.;;


module evaluateString =
    let evaluateString stringExpression = evaluateCharList.evaluateCharList (Seq.toList stringExpression);;
