﻿#if !(DOTNET_CORE || NETFX_CORE) 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MoonSharp.Interpreter.Compatibility.Frameworks
{
	abstract class FrameworkClrBase : FrameworkReflectionBase
	{
		BindingFlags BINDINGFLAGS_MEMBER = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
		BindingFlags BINDINGFLAGS_INNERCLASS = BindingFlags.Public | BindingFlags.NonPublic;

		public override Type GetTypeInfoFromType(Type t)
		{
			return t;
		}

		public override MethodInfo GetAddMethod(EventInfo ei)
		{
			return ei.GetAddMethod(true);
		}

		public override ConstructorInfo[] GetConstructors(Type type)
		{
			return type.GetConstructors(BINDINGFLAGS_MEMBER);
		}

		public override EventInfo[] GetEvents(Type type)
		{
			return type.GetEvents(BINDINGFLAGS_MEMBER);
		}

		public override FieldInfo[] GetFields(Type type)
		{
			return type.GetFields(BINDINGFLAGS_MEMBER);
		}

		public override Type[] GetGenericArguments(Type type)
		{
			return type.GetGenericArguments();
		}

		public override MethodInfo GetGetMethod(PropertyInfo pi)
		{
			return pi.GetGetMethod();
		}

		public override Type[] GetInterfaces(Type t)
		{
			return t.GetInterfaces();
		}

		public override MethodInfo GetMethod(Type type, string name)
		{
			return type.GetMethod(name);
		}

		public override MethodInfo[] GetMethods(Type type)
		{
			return type.GetMethods(BINDINGFLAGS_MEMBER);
		}

		public override Type[] GetNestedTypes(Type type)
		{
			return type.GetNestedTypes(BINDINGFLAGS_INNERCLASS);
		}

		public override PropertyInfo[] GetProperties(Type type)
		{
			return type.GetProperties(BINDINGFLAGS_MEMBER);
		}

		public override PropertyInfo GetProperty(Type type, string name)
		{
			return type.GetProperty(name);
		}

		public override MethodInfo GetRemoveMethod(EventInfo ei)
		{
			return ei.GetRemoveMethod();
		}

		public override MethodInfo GetSetMethod(PropertyInfo pi)
		{
			return pi.GetSetMethod();
		}


		public override bool IsAssignableFrom(Type current, Type toCompare)
		{
			return current.IsAssignableFrom(toCompare);
		}

		public override bool IsInstanceOfType(Type t, object o)
		{
			return t.IsInstanceOfType(o);
		}


		public override MethodInfo GetMethod(Type resourcesType, string name, Type[] types)
		{
			return resourcesType.GetMethod(name, types);
		}

		public override Type[] GetAssemblyTypes(Assembly asm)
		{
			return asm.GetTypes();
		}

	}
}

#endif