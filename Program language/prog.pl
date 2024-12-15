func printStack plusNum
{
    pop
    print = R + plusNum>
}

var in1 = input
var in2 = input
var plus = input

ldi in1+in2
var a = R

var b = in1-in2

ldi in1 * in2>
var c = R

var d = in1 / in2>

push a
call printStack plus
push b
call printStack plus
push c
call printStack plus
push d
call printStack plus
