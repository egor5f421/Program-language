func printStack
pop
print
end

var in1 = input
var in2 = input

ldi in1+in2
var a = R

var b = in1-in2

ldi in1 * in2>
var c = R

var d = in1 / in2>

push a
call printStack
push b
call printStack
push c
call printStack
push d
call printStack
