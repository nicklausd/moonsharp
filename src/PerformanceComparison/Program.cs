﻿using System;
using System.Collections.Generic;
using MoonSharp.Interpreter.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Execution;
using NLua;
using System.Diagnostics;

namespace PerformanceComparison
{
	class Program
	{
		const int ITERATIONS = 100;

		static  string scriptText1 = @"
			function move(n, src, dst, via)
				if n > 0 then
					move(n - 1, src, via, dst)
					--print(src, 'to', dst)
					move(n - 1, via, dst, src)
				end
			end
 
			move(4, 1, 2, 3)
			";
		static  string scriptText = @"
N = 8
 
board = {}
for i = 1, N do
    board[i] = {}
    for j = 1, N do
		board[i][j] = false
    end
end
 
function Allowed( x, y )
    for i = 1, x-1 do
	if ( board[i][y] ) or ( i <= y and board[x-i][y-i] ) or ( y+i <= N and board[x-i][y+i] ) then 
  	    return false 
	end
    end		
    return true
end
 
function Find_Solution( x )
    for y = 1, N do
	if Allowed( x, y ) then 
  	    board[x][y] = true 
	    if x == N or Find_Solution( x+1 ) then
		return true
	    end
	    board[x][y] = false			 
	end		
    end
    return false
end
 
if Find_Solution( 1 ) then
    for i = 1, N do
 	for j = 1, N do
  	    if board[i][j] then 
		print( 'Q' )
	    else 
		print( 'x' )
	    end
	end
	print( '|' )
    end
else
    print( 'NO!' )
end
 
			";
		static StringBuilder g_MoonSharpStr = new StringBuilder();
		static StringBuilder g_NLuaStr = new StringBuilder();

		public static RValue Print(RValue[] values)
		{
			foreach (var val in values)
			{
				g_MoonSharpStr.Append(val.AsString());
			}

			g_MoonSharpStr.AppendLine();
			return RValue.Nil;
		}

		private static void Example()
		{
			Table t = new Table();
			t[new RValue("print")] = new RValue(new CallbackFunction(Print));

			Script script = MoonSharpInterpreter.LoadFromFile(@"c:\temp\test.lua", t);

			RValue retVal = script.Execute();
		}

		public static void NPrint(params object[] values)
		{
			foreach (var val in values)
			{
				g_NLuaStr.Append(val.ToString());
			}
			g_NLuaStr.AppendLine();
		}

		static Lua lua = new Lua();
		static string testString = "world";

		static void Main(string[] args)
		{
			Stopwatch sw;

			sw = Stopwatch.StartNew();

			Table t = new Table();
			t[new RValue("print")] = new RValue(new CallbackFunction(Print));

			MoonSharpInterpreter.LoadFromString(scriptText, t);

			sw.Stop();

			Console.WriteLine("Build : {0} ms", sw.ElapsedMilliseconds);

			sw = Stopwatch.StartNew();

			t = new Table();
			t[new RValue("print")] = new RValue(new CallbackFunction(Print));

			var script = MoonSharpInterpreter.LoadFromString(scriptText, t);

			sw.Stop();

			Console.WriteLine("Build 2: {0} ms", sw.ElapsedMilliseconds);


			sw = Stopwatch.StartNew();
			for (int i = 0; i < ITERATIONS; i++)
			{
				script.Execute();
			}
			sw.Stop();

			Console.WriteLine("Moon# : {0} ms", sw.ElapsedMilliseconds);


			lua.RegisterFunction("print", typeof(Program).GetMethod("NPrint"));

			File.WriteAllText(@"c:\temp\hanoi.lua", scriptText);

			var fn = lua.LoadFile(@"c:\temp\hanoi.lua");

			sw = Stopwatch.StartNew();
			for (int i = 0; i < ITERATIONS; i++)
			{
				fn.Call();
			}
			sw.Stop();

			Console.WriteLine("NLua  : {0} ms", sw.ElapsedMilliseconds);

			Console.WriteLine("M# == NL ? {0}", g_MoonSharpStr.ToString() == g_NLuaStr.ToString());

			//Console.WriteLine("=== Moon# ===");
			//Console.WriteLine(g_MoonSharpStr.ToString());
			//Console.WriteLine("");
			//Console.WriteLine("=== NLua  ===");
			//Console.WriteLine(g_NLuaStr.ToString());

			Console.ReadKey();
		}
	}
}