# Basic-Maths-Parser
Basic maths parser in C# I wrote to solve a coursework problem in Year 1 COMP1000 Software Engineering. It can do + - * / operations as well as correctly evaluates brackets. It first converts the infix notation into RPN before evaluation.

It will evaluate a divide by 0 to infinity

First it converts the infix notation to reverse polish notation and thne uses the shunting yard algorithm to evaluate RPN expresions. Explination of shunting yard [here](https://brilliant.org/wiki/shunting-yard-algorithm/)

### Features to add at some point:

- Powers
- Roots
- Trig Functions
