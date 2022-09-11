namespace SharedCode.Reflection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// The expression generator class.
/// </summary>
internal static class ExpressionGenerator
{
	internal static T?[]? Clone1DimArrayClassInternal<T>(T[] objFrom, T?[] objTo, DeepCloneState state)
	{
		if (objFrom is null || objTo is null)
		{
			return null;
		}

		var l = Math.Min(objFrom.Length, objTo.Length);
		state.AddKnownRef(objFrom, objTo);
		for (var i = 0; i < l; i++)
		{
			objTo[i] = (T?)DeepCloneGenerator.CloneClassInternal(objFrom[i], state);
		}

		return objTo;
	}

	internal static T[] Clone1DimArraySafeInternal<T>(T[] objFrom, T[] objTo, DeepCloneState state)
	{
		var l = Math.Min(objFrom.Length, objTo.Length);
		state.AddKnownRef(objFrom, objTo);
		Array.Copy(objFrom, objTo, l);
		return objTo;
	}

	internal static T[]? Clone1DimArrayStructInternal<T>(T[] objFrom, T[] objTo, DeepCloneState state)
	{
		if (objFrom is null || objTo is null)
		{
			return null;
		}

		var l = Math.Min(objFrom.Length, objTo.Length);
		state.AddKnownRef(objFrom, objTo);
		var cloner = DeepCloneGenerator.GetClonerForValueType<T>();
		for (var i = 0; i < l; i++)
		{
			objTo[i] = cloner(objTo[i], state);
		}

		return objTo;
	}

	[SuppressMessage("Performance", "CA1814:Prefer jagged arrays over multidimensional", Justification = "<Pending>")]
	internal static T?[,]? Clone2DimArrayInternal<T>(T[,] objFrom, T?[,] objTo, DeepCloneState state, bool isDeep)
	{
		if (objFrom is null || objTo is null)
		{
			return null;
		}

		if (objFrom.GetLowerBound(0) != 0 || objFrom.GetLowerBound(1) != 0 ||
			objTo.GetLowerBound(0) != 0 || objTo.GetLowerBound(1) != 0)
		{
			return (T[,]?)CloneAbstractArrayInternal(objFrom, objTo, state, isDeep);
		}

		var l1 = Math.Min(objFrom.GetLength(0), objTo.GetLength(0));
		var l2 = Math.Min(objFrom.GetLength(1), objTo.GetLength(1));
		state.AddKnownRef(objFrom, objTo);
		if ((!isDeep || DeepCloneSafeTypes.CanReturnSameObject(typeof(T)))
			&& objFrom.GetLength(0) == objTo.GetLength(0)
			&& objFrom.GetLength(1) == objTo.GetLength(1))
		{
			Array.Copy(objFrom, objTo, objFrom.Length);
			return objTo;
		}

		if (!isDeep)
		{
			for (var i = 0; i < l1; i++)
			{
				for (var k = 0; k < l2; k++)
				{
					objTo[i, k] = objFrom[i, k];
				}
			}

			return objTo;
		}

		if (typeof(T).IsValueType())
		{
			var cloner = DeepCloneGenerator.GetClonerForValueType<T>();
			for (var i = 0; i < l1; i++)
			{
				for (var k = 0; k < l2; k++)
				{
					objTo[i, k] = cloner(objFrom[i, k], state);
				}
			}
		}
		else
		{
			for (var i = 0; i < l1; i++)
			{
				for (var k = 0; k < l2; k++)
				{
					objTo[i, k] = (T?)DeepCloneGenerator.CloneClassInternal(objFrom[i, k], state);
				}
			}
		}

		return objTo;
	}

	/// <summary>
	/// Clones the abstract array internal.
	/// </summary>
	/// <param name="objFrom">The object from.</param>
	/// <param name="objTo">The object to.</param>
	/// <param name="state">The state.</param>
	/// <param name="isDeep">if set to <c>true</c> [is deep].</param>
	/// <returns>System.Nullable&lt;Array&gt;.</returns>
	/// <exception cref="InvalidOperationException">Invalid rank of target array</exception>
	/// <remarks>rare cases, very slow cloning. currently it's ok</remarks>
	internal static Array? CloneAbstractArrayInternal(Array objFrom, Array objTo, DeepCloneState state, bool isDeep)
	{
		// not null from called method, but will check it anyway
		if (objFrom is null || objTo is null)
		{
			return null;
		}

		var rank = objFrom.Rank;

		if (objTo.Rank != rank)
		{
			throw new InvalidOperationException("Invalid rank of target array");
		}

		var lowerBoundsFrom = Enumerable.Range(0, rank).Select(objFrom.GetLowerBound).ToArray();
		var lowerBoundsTo = Enumerable.Range(0, rank).Select(objTo.GetLowerBound).ToArray();
		var lengths = Enumerable.Range(0, rank).Select(x => Math.Min(objFrom.GetLength(x), objTo.GetLength(x))).ToArray();
		var idxesFrom = Enumerable.Range(0, rank).Select(objFrom.GetLowerBound).ToArray();
		var idxesTo = Enumerable.Range(0, rank).Select(objTo.GetLowerBound).ToArray();

		state.AddKnownRef(objFrom, objTo);

		// unable to copy any element
		if (lengths.Contains(0))
		{
			return objTo;
		}

		while (true)
		{
			if (isDeep)
			{
				objTo.SetValue(DeepCloneGenerator.CloneClassInternal(objFrom.GetValue(idxesFrom), state), idxesTo);
			}
			else
			{
				objTo.SetValue(objFrom.GetValue(idxesFrom), idxesTo);
			}

			var ofs = rank - 1;
			while (true)
			{
				idxesFrom[ofs]++;
				idxesTo[ofs]++;
				if (idxesFrom[ofs] >= lowerBoundsFrom[ofs] + lengths[ofs])
				{
					idxesFrom[ofs] = lowerBoundsFrom[ofs];
					idxesTo[ofs] = lowerBoundsTo[ofs];
					ofs--;
					if (ofs < 0)
					{
						return objTo;
					}
				}
				else
				{
					break;
				}
			}
		}
	}

	internal static object GenerateClonerInternal(Type realType, bool isDeepClone)
	{
		return realType.IsValueType()
			? throw new InvalidOperationException("Operation is valid only for reference types")
			: GenerateProcessMethod(realType, isDeepClone);
	}

	internal static T[] ShallowClone1DimArraySafeInternal<T>(T[] objFrom, T[] objTo)
	{
		var l = Math.Min(objFrom.Length, objTo.Length);
		Array.Copy(objFrom, objTo, l);
		return objTo;
	}

	private static object GenerateProcessArrayMethod(Type type, bool isDeep)
	{
		var elementType = type.GetElementType();
		var rank = type.GetArrayRank();

		var from = Expression.Parameter(typeof(object));
		var to = Expression.Parameter(typeof(object));
		var state = Expression.Parameter(typeof(DeepCloneState));

		var funcType = typeof(Func<,,,>).MakeGenericType(typeof(object), typeof(object), typeof(DeepCloneState), typeof(object));

		if (rank == 1 && type == elementType!.MakeArrayType())
		{
			if (!isDeep)
			{
				var callExpression = Expression.Call(
					typeof(ExpressionGenerator).GetPrivateStaticMethod("ShallowClone1DimArraySafeInternal")!
						.MakeGenericMethod(elementType),
					Expression.Convert(from, type),
					Expression.Convert(to, type));
				return Expression.Lambda(funcType, callExpression, from, to, state).Compile();
			}

			var methodName = "Clone1DimArrayClassInternal";
			if (DeepCloneSafeTypes.CanReturnSameObject(elementType)) methodName = "Clone1DimArraySafeInternal";
			else if (elementType.IsValueType()) methodName = "Clone1DimArrayStructInternal";
			var methodInfo = typeof(ExpressionGenerator).GetPrivateStaticMethod(methodName)!.MakeGenericMethod(elementType);
			var callS2 = Expression.Call(methodInfo, Expression.Convert(from, type), Expression.Convert(to, type), state);

			return Expression.Lambda(funcType, callS2, from, to, state).Compile();
		}

		// multidim or not zero-based arrays
		var methodInfo2 =
			rank == 2 && type == elementType?.MakeArrayType(2)
				? typeof(ExpressionGenerator).GetPrivateStaticMethod("Clone2DimArrayInternal")!.MakeGenericMethod(elementType)
				: typeof(ExpressionGenerator).GetPrivateStaticMethod("CloneAbstractArrayInternal");
		var callS = Expression.Call(methodInfo2!, Expression.Convert(from, type), Expression.Convert(to, type), state, Expression.Constant(isDeep));

		return Expression.Lambda(funcType, callS, from, to, state).Compile();
	}

	private static object GenerateProcessMethod(Type type, bool isDeepClone)
	{
		if (type.IsArray)
		{
			return GenerateProcessArrayMethod(type, isDeepClone);
		}

		var methodType = typeof(object);

		var expressionList = new List<Expression>();

		var from = Expression.Parameter(methodType);
		var fromLocal = from;
		var to = Expression.Parameter(methodType);
		var toLocal = to;
		var state = Expression.Parameter(typeof(DeepCloneState));

		fromLocal = Expression.Variable(type);
		toLocal = Expression.Variable(type);
		// fromLocal = (T)from
		expressionList.Add(Expression.Assign(fromLocal, Expression.Convert(from, type)));
		expressionList.Add(Expression.Assign(toLocal, Expression.Convert(to, type)));

		if (isDeepClone)
		{
			// added from -> to binding to ensure reference loop handling structs cannot loop here
			// state.AddKnownRef(from, to)
			expressionList.Add(Expression.Call(state, typeof(DeepCloneState).GetMethod("AddKnownRef")!, from, to));
		}

		var fi = new List<FieldInfo>();
		var tp = type;
		do
		{
			if (tp.Name == "ContextBoundObject") break;

			fi.AddRange(tp.GetDeclaredFields());
			tp = tp.BaseType();
		}
		while (tp is not null);

		foreach (var fieldInfo in fi)
		{
			if (isDeepClone && !DeepCloneSafeTypes.CanReturnSameObject(fieldInfo.FieldType))
			{
				var methodInfo = fieldInfo.FieldType.IsValueType()
					? typeof(DeepCloneGenerator).GetPrivateStaticMethod("CloneStructInternal")!
						.MakeGenericMethod(fieldInfo.FieldType)
					: typeof(DeepCloneGenerator).GetPrivateStaticMethod("CloneClassInternal");

				var get = Expression.Field(fromLocal, fieldInfo);

				var call = (Expression)Expression.Call(methodInfo!, get, state);
				if (!fieldInfo.FieldType.IsValueType())
				{
					call = Expression.Convert(call, fieldInfo.FieldType);
				}

				if (fieldInfo.IsInitOnly)
				{
					var setMethod = typeof(DeepCloneExpressionGenerator).GetPrivateStaticMethod("ForceSetField");
					expressionList.Add(
						Expression.Call(
							 setMethod!,
							 Expression.Constant(fieldInfo),
							 Expression.Convert(toLocal, typeof(object)),
							 Expression.Convert(call, typeof(object))));
				}
				else
				{
					expressionList.Add(Expression.Assign(Expression.Field(toLocal, fieldInfo), call));
				}
			}
			else
			{
				expressionList.Add(Expression.Assign(Expression.Field(toLocal, fieldInfo), Expression.Field(fromLocal, fieldInfo)));
			}
		}

		expressionList.Add(Expression.Convert(toLocal, methodType));

		var funcType = typeof(Func<,,,>).MakeGenericType(methodType, methodType, typeof(DeepCloneState), methodType);

		var blockParams = new List<ParameterExpression>();
		if (from != fromLocal)
		{
			blockParams.Add(fromLocal);
		}

		if (to != toLocal)
		{
			blockParams.Add(toLocal);
		}

		return Expression.Lambda(funcType, Expression.Block(blockParams, expressionList), from, to, state).Compile();
	}
}
