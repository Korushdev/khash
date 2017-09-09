## Synopsis

KHash is a strong typed interpreted language written on top of C#. At this stage of development the language is HIGHLY EXPERIMENTAL and should not be used for real projects!

## Installation

Clone the repo or download a zip, then import into VS

The language can be run in various ways, however defining a .ini file is a necessary step

Setting a .ini file
1. Create a directory called .conf and place it in the same directory as the .exe file
2. Copy and rename the sample KHash.ini.example to conf/KHash.ini

Running test code using a string
1. Open command line and change cd into the directory of the .exe
2. Run $: KHash --string "int i = 5; send i;"
3. You should see the output of 5!

Running the test code using a file
1. Open command line and change cd into the directory of the .exe
2. Run $: KHash --file /path/to/your/.khash


## Examples

### Ex1 - variable assignment
```
int a = 5;
int b = 10;
int c = a + b;

send c;
```

### Ex2 - IF statement
```
bool isOk = true;
if( isOk == true )
{
    send "is ok!";
}
```

### Ex3 - For Statement
```
for( int a = 0; a < 5; a++ )
{
    send a;
}
```

### Ex4 - While Statement
```
int a = 5;
while( a < 10 )
{
    send "See me until int a is less than 10";
    a = a + 1;
}
```

### Ex5 - Switch statement
```
int a = 5;
switch( a )
{
    case 3:
        send "is 3";
        break;
    case 4:
        send "is 4";
        break;
    case 5:
        send "is 5";
        break;
}
```

### Ex6 - Function Declaration
```
int a = 3;
int function add( int num )
{
	return num + 7;
}
send add( a );
```

## Language reference

### Variable types
```
int = 1;
float = 2.5;
double = 2.8975287349723;
decimal = 4.329035809238509;
bool = true/false;
```

## Math Operators
`+` Plus<br/>
`-` Minus<br/>
`/` Divide<br/>
`*` Multiply<br/>
`a++` Post Increment<br/>
`++a` Pre Increment<br/>
`a--` Post Decrement<br/>
`--a` Pre Decrement<br/>

### Comparison Operators
`==` Equals<br/>
`!=` Not equals<br/>
`>` Greater than<br/>
`<` Less than<br/>
`>=` Greater than or equal<br/>
`<=` Less than or equal<br/>

### Keywords
`send` Prints to the output buffer


## Tests

No tests ( as of yet!)

## Contributors

Contributors are welcome to join the project, add code, suggest changes and help create an awesome language!

## Todo

1. Write unit Tests
2. Comment source code
3. Support namespaces
4. Add Inbuilt classes

## License

Open source