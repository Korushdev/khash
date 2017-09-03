﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Lexer
{
    public enum TokenType
    {
        Var,
        BeginCode,
        EndCode,
        If,
        Else,
        Send,
        Function,
        WhiteSpace,
        EOF,
        LBracket,
        RBracket,
        Increment,
        Decrement,
        Plus,
        Minus,
        Equals,
        Match,
        NotMatch,
        Not,
        OpenParenth,
        CloseParenth,
        Slash,
        Ampersand,
        And,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Or,
        Colon,
        SemiColon,
        Dot,
        Word,
        QuotedString,
        Void,
        Boolean,
        Int,
        Float,
        String,
        ScopeStart,
        Asterix,
        True,
        False,
        Nil,
        Switch,
        Case,
        CaseOf,
        Break,
        Comma,
        Return,
        While,
        For
    }
}
