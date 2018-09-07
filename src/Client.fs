namespace WebSharper.MathJS.Samples

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.UI.Templating
open WebSharper.MathJS
open WebSharper.MathJax

[<JavaScript>]
module Main =

    type MainTemplate = Template<"wwwroot/index.html", clientLoad = ClientLoad.FromDocument>

    let BigNumbers() =
        let rvInput1 = Var.Create "1"
        let rvInput2 = Var.Create "1"

        let viewInput1 = rvInput1.View
        let viewInput2 = rvInput2.View

        let result operator =
            View.Map2 (fun (l: string) (r: string) ->
                try
                    (operator (Math.Bignumber l) (Math.Bignumber r)).ToString()
                with _ ->
                    "Wrong input"
            ) viewInput1 viewInput2

        let resultAdd = result (fun l r -> l + r)
        let resultSubtract = result (fun l r -> l - r)
        let resultMultiply = result (fun l r -> l * r)
        let resultDivide = result (fun l r -> l / r)

        MainTemplate.BigNumbers()
            .Input1(rvInput1)
            .Input2(rvInput2)
            .ResultAdd(resultAdd)
            .ResultSubtract(resultSubtract)
            .ResultMultiply(resultMultiply)
            .ResultDivide(resultDivide)
            .Doc()

    let Differentiation() =
        MathJax.Hub.Config(
            MathJax.Config(
                Extensions = [| "tex2jax.js" |],
                Jax = [| "input/TeX"; "output/HTML-CSS"; |],
                Tex2jax = MathJax.Tex2jax(InlineMath = [| ("$", "$"); ("\\(", "\\)") |])
            )
        )

        let rvFormula = Var.Create "x^2/(x^9 + x^2) + x^4/2"
        let rvDerivateBy = Var.Create "x"

        let viewFormula = rvFormula.View
        let viewDerivateBy = rvDerivateBy.View

        let texFormula =
            View.Map2 (fun (formula : string) (derivateby : string) ->
                try
                    let simplify = MathJS.DerivativeOption(Simplify=true)
                    (MathJS.Math.Derivative(formula, derivateby, simplify)).ToTex()
                with _ ->
                    "The\ formula\ isn\'t\ correct."
            ) viewFormula viewDerivateBy

        let tex =
            div [
                on.viewUpdate texFormula (fun e v ->
                    e.TextContent <- "$$" + v + "$$"
                    MathJax.Hub.Queue([| "Typeset", MathJax.MathJax.Hub :> obj, [| e :> obj |] |]) |> ignore
                )
            ] [text ("$$" + texFormula.V + "$$")]

        MainTemplate.Differentiation()
            .Formula(rvFormula)
            .DerivateBy(rvDerivateBy)
            .Tex(tex)
            .Doc()

    let Vectors() =
        MathJax.Hub.Config(
            MathJax.Config(
                Extensions = [| "tex2jax.js" |],
                Jax = [| "input/TeX"; "output/HTML-CSS"; |],
                Tex2jax = MathJax.Tex2jax(InlineMath = [| ("$", "$"); ("\\(", "\\)") |])
            )
        )

        let vector1 = [| 8.; 6.; 4.; 6. |]
        let vector2 = [| 6.; 14.; 8.; 2. |]

        let prettyVector1 = "$" + (Math.Parse("[" + string vector1 + "]").ToTex()) + "$"
        let prettyVector2 = "$" + (Math.Parse("[" + string vector2 + "]").ToTex()) + "$"

        let result op = "[" + string (op vector1 vector2) + "]"

        let resultAdd = result (fun l r -> Math.Add(MathNumber(l), MathNumber(r)))
        let resultSubtract = result (fun l r -> Math.Subtract(MathNumber(l), MathNumber(r)))
        let resultMultiply = result (fun l r -> Math.Multiply(MathNumber(l), MathNumber(r)))

        let prettyResultAdd = "$" + (Math.Parse(string resultAdd).ToTex()) + "$"
        let prettyResultSubtract = "$" + (Math.Parse(string resultSubtract).ToTex()) + "$"
        let prettyResultMultiply = "$" + (Math.Parse(string resultMultiply).ToTex()) + "$"

        MainTemplate.Vectors()
            .Vector1(prettyVector1)
            .Vector2(prettyVector2)
            .ResultAdd(prettyResultAdd)
            .ResultSubtract(prettyResultSubtract)
            .ResultMultiply(prettyResultMultiply)
            .Doc()

    let Matrices() =
        MathJax.Hub.Config(
            MathJax.Config(
                Extensions = [| "tex2jax.js" |],
                Jax = [| "input/TeX"; "output/HTML-CSS"; |],
                Tex2jax = MathJax.Tex2jax(InlineMath = [| ("$", "$"); ("\\(", "\\)") |])
            )
        )

        let matrix1 = Math.Matrix([| [| 8.; 6. |]; [| 4.; 6. |] |])
        let matrix2 = Math.Matrix([| [| 6.; 14. |]; [| 8.; 2. |] |])

        let prettyMatrix1 = "$" + (Math.Parse(string matrix1).ToTex()) + "$"
        let prettyMatrix2 = "$" + (Math.Parse(string matrix2).ToTex()) + "$"

        let result op = (op matrix1 matrix2)

        let resultAdd = result (fun l r -> Math.Add(MathNumber(l), MathNumber(r)))
        let resultSubtract = result (fun l r -> Math.Subtract(MathNumber(l), MathNumber(r)))
        let resultMultiply = result (fun l r -> Math.Multiply(MathNumber(l), MathNumber(r)))
        let resultDivide = result (fun l r -> Math.Divide(MathNumber(l), MathNumber(r)))

        let prettyResultAdd = "$" + (Math.Parse(string resultAdd).ToTex()) + "$"
        let prettyResultSubtract = "$" + (Math.Parse(string resultSubtract).ToTex()) + "$"
        let prettyResultMultiply = "$" + (Math.Parse(string resultMultiply).ToTex()) + "$"
        let prettyResultDivide = "$" + (Math.Parse(string resultDivide).ToTex()) + "$"

        MainTemplate.Matrices()
            .Matrix1(prettyMatrix1)
            .Matrix2(prettyMatrix2)
            .ResultAdd(prettyResultAdd)
            .ResultSubtract(prettyResultSubtract)
            .ResultMultiply(prettyResultMultiply)
            .ResultDivide(prettyResultDivide)
            .Doc()

    let Units() =
        let rvInput1 = Var.Create "1 cm"
        let rvInput2 = Var.Create "1 cm"
        let rvTranslate = Var.Create "m"

        let viewInput1 = rvInput1.View
        let viewInput2 = rvInput2.View
        let viewTranslate = rvTranslate.View

        let result operator =
            View.Map2 (fun (l: string) (r: string) ->
                try
                    (operator (Math.Unit l) (Math.Unit r)).ToString()
                with _ ->
                    "Wrong input"
            ) viewInput1 viewInput2

        let resultAdd = result (fun l r -> Math.Add(l, r))
        let resultSubtract = result (fun l r -> Math.Subtract(l, r))
        let resultMultiply = result (fun l r -> Math.Multiply(l, r))
        let resultDivide = result (fun l r -> Math.Divide(l, r))

        let translate unit trans =
            View.Map2 (fun (u: string) (t: string) ->
                try
                    (Math.To(Math.Unit(u), Math.Unit(t))).ToString()
                with _ ->
                    "Couldn't translate."
            ) unit trans

        let translateResultAdd = translate resultAdd viewTranslate
        let translateResultSubtract = translate resultSubtract viewTranslate
        let translateResultMultiply = translate resultMultiply viewTranslate
        let translateResultDivide = translate resultDivide viewTranslate

        MainTemplate.Units()
            .Input1(rvInput1)
            .Input2(rvInput2)
            .Translate(rvTranslate)
            .ResultAdd(resultAdd)
            .ResultSubtract(resultSubtract)
            .ResultMultiply(resultMultiply)
            .ResultDivide(resultDivide)
            .TranslatedResultAdd(translateResultAdd)
            .TranslatedResultSubtract(translateResultSubtract)
            .TranslatedResultMultiply(translateResultMultiply)
            .TranslatedResultDivide(translateResultDivide)
            .Doc()

    [<SPAEntryPoint>]
    let Main() =
        Doc.Concat [
            h3 [] [text "Big numbers"]
            BigNumbers()
            h3 [] [text "Differentiation"]
            Differentiation()
            h3 [] [text "Vector operations"]
            Vectors()
            h3 [] [text "Matrix operations"]
            Matrices()
            h3 [] [text "Units"]
            Units()
        ]
        |> Doc.RunById "main"
