
using SharedCode.Linq;

using System.Collections.Concurrent;
using System.Reflection;

namespace SharedCode.Reflection;
internal static class DeepCloneExpressionGenerator
{
	private static readonly MethodInfo? _fieldSetMethod;
	private static readonly ConcurrentDictionary<FieldInfo, bool> _readonlyFields = new();

	[SuppressMessage("Roslynator", "RCS1169:Make field read-only.", Justification = "<Pending>")]
	[SuppressMessage("Performance", "CA1823:Avoid unused private fields", Justification = "<Pending>")]
	[SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>")]
	[SuppressMessage("Roslynator", "RCS1213:Remove unused member declaration.", Justification = "<Pending>")]
	[SuppressMessage("Style", "GCop406:Mark {0} field as read-only.", Justification = "<Pending>")]
	private static FieldInfo? _attributesFieldInfo = typeof(FieldInfo).GetPrivateField("m_fieldAttributes");

	[SuppressMessage("Roslynator", "RCS1169:Make field read-only.", Justification = "<Pending>")]
	[SuppressMessage("Style", "GCop406:Mark {0} field as read-only.", Justification = "<Pending>")]
	[SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>")]
#pragma warning disable CS0649 // Field 'DeepCloneExpressionGenerator._canFastCopyReadonlyFields' is never assigned to, and will always have its default value false
	private static bool _canFastCopyReadonlyFields;
#pragma warning restore CS0649 // Field 'DeepCloneExpressionGenerator._canFastCopyReadonlyFields' is never assigned to, and will always have its default value false

	[SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
	[SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
	[SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline", Justification = "<Pending>")]
	[SuppressMessage("Roslynator", "RCS1075:Avoid empty catch clause that catches System.Exception.", Justification = "<Pending>")]
	static DeepCloneExpressionGenerator()
	{
		try
		{
			typeof(DeepCloneExpressionGenerator).GetPrivateStaticField(nameof(_canFastCopyReadonlyFields))!.SetValue(null, true);
			_fieldSetMethod = typeof(FieldInfo).GetRuntimeMethod("SetValue", new[] { typeof(object), typeof(object) });
			if (_fieldSetMethod is null)
			{
				throw new ArgumentNullException();
			}
		}
		catch (Exception)
		{
			// cannot
		}
	}

	[SuppressMessage("Reliability", "CA2002:Do not lock on objects with weak identity", Justification = "<Pending>")]
	internal static void ForceSetField(FieldInfo field, object obj, object value)
	{
		var fieldInfo = field.GetType().GetPrivateField("m_fieldAttributes");

		if (fieldInfo is null)
		{
			return;
		}

		var ov = fieldInfo.GetValue(field);
		if (ov is not FieldAttributes)
		{
			return;
		}

		var v = (FieldAttributes)ov;

		// protect from parallel execution, when first thread set field readonly back, and second
		// set it to write value
		lock (fieldInfo)
		{
			fieldInfo.SetValue(field, v & ~FieldAttributes.InitOnly);
			field.SetValue(obj, value);
			fieldInfo.SetValue(field, v | FieldAttributes.InitOnly);
		}
	}

	internal static object GenerateClonerInternal(Type realType, bool asObject) => GenerateProcessMethod(realType, asObject && realType.IsValueType());

	private static object GenerateProcessArrayMethod(Type type)
	{
		var elementType = type.GetElementType();
		var rank = type.GetArrayRank();

		MethodInfo methodInfo;

		// multidim or not zero-based arrays
		if (rank != 1 || type != elementType?.MakeArrayType())
		{
			if (rank == 2 && type == elementType?.MakeArrayType(2))
			{
				// small optimization for 2 dim arrays
				methodInfo = typeof(DeepCloneGenerator).GetPrivateStaticMethod("Clone2DimArrayInternal")!.MakeGenericMethod(elementType);
			}
			else
			{
				methodInfo = typeof(DeepCloneGenerator).GetPrivateStaticMethod("CloneAbstractArrayInternal")!;
			}
		}
		else
		{
			var methodName = "Clone1DimArrayClassInternal";
			if (DeepCloneSafeTypes.CanReturnSameObject(elementType))
			{
				methodName = "Clone1DimArraySafeInternal";
			}
			else if (elementType.IsValueType())
			{
				methodName = "Clone1DimArrayStructInternal";
			}

			methodInfo = typeof(DeepCloneGenerator).GetPrivateStaticMethod(methodName)!.MakeGenericMethod(elementType);
		}

		var from = Expression.Parameter(typeof(object));
		var state = Expression.Parameter(typeof(DeepCloneState));
		var call = Expression.Call(methodInfo!, Expression.Convert(from, type), state);

		var funcType = typeof(Func<,,>).MakeGenericType(typeof(object), typeof(DeepCloneState), typeof(object));

		return Expression.Lambda(funcType, call, from, state).Compile();
	}

	private static object GenerateProcessMethod(Type type, bool unboxStruct)
	{
		if (type.IsArray)
		{
			return GenerateProcessArrayMethod(type);
		}

		if (type.FullName is not null && type.FullName.StartsWith("System.Tuple`", StringComparison.Ordinal))
		{
			// if not safe type it is no guarantee that some type will contain reference to this
			// tuple. In usual way, we're creating new object, setting reference for it and filling
			// data. For tuple, we will fill data before creating object (in constructor arguments)
			var genericArguments = type.GenericArguments();
			// current tuples contain only 8 arguments, but may be in future... we'll write code
			// that works with it
			if (genericArguments.Length < 10 && genericArguments.All(DeepCloneSafeTypes.CanReturnSameObject))
			{
				return GenerateProcessTupleMethod(type);
			}
		}

		var methodType = unboxStruct || type.IsClass() ? typeof(object) : type;

		var expressionList = new List<Expression>();

		var from = Expression.Parameter(methodType);
		var fromLocal = from;
		var toLocal = Expression.Variable(type);
		var state = Expression.Parameter(typeof(DeepCloneState));

		if (!type.IsValueType())
		{
			var methodInfo = typeof(object).GetPrivateMethod(nameof(MemberwiseClone));

			// to = (T)from.MemberwiseClone()
			expressionList.Add(Expression.Assign(toLocal, Expression.Convert(Expression.Call(from, methodInfo!), type)));

			fromLocal = Expression.Variable(type);
			// fromLocal = (T)from
			expressionList.Add(Expression.Assign(fromLocal, Expression.Convert(from, type)));

			// added from -> to binding to ensure reference loop handling structs cannot loop here
			// state.AddKnownRef(from, to)
			expressionList.Add(Expression.Call(state, typeof(DeepCloneState).GetMethod(nameof(DeepCloneState.AddKnownRef))!, from, toLocal));
		}
		else if (unboxStruct)
		{
			// toLocal = (T)from;
			expressionList.Add(Expression.Assign(toLocal, Expression.Unbox(from, type)));
			fromLocal = Expression.Variable(type);
			// fromLocal = toLocal; // structs, it is ok to copy
			expressionList.Add(Expression.Assign(fromLocal, toLocal));
		}
		else
		{
			// toLocal = from
			expressionList.Add(Expression.Assign(toLocal, from));
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
			if (!DeepCloneSafeTypes.CanReturnSameObject(fieldInfo.FieldType))
			{
				var methodInfo = fieldInfo.FieldType.IsValueType()
					? typeof(DeepCloneGenerator).GetPrivateStaticMethod("CloneStructInternal")!.MakeGenericMethod(fieldInfo.FieldType)
					: typeof(DeepCloneGenerator).GetPrivateStaticMethod("CloneClassInternal");

				var get = Expression.Field(fromLocal, fieldInfo);

				var call = (Expression)Expression.Call(methodInfo!, get, state);
				if (!fieldInfo.FieldType.IsValueType())
				{
					call = Expression.Convert(call, fieldInfo.FieldType);
				}

				var isReadonly = _readonlyFields.GetOrAdd(fieldInfo, f => f.IsInitOnly);
				if (isReadonly)
				{
					if (_canFastCopyReadonlyFields)
					{
						expressionList.Add(Expression.Call(
							Expression.Constant(fieldInfo),
							_fieldSetMethod!,
							Expression.Convert(toLocal, typeof(object)),
							Expression.Convert(call, typeof(object))));
					}
					else
					{
						var setMethod = typeof(DeepCloneExpressionGenerator).GetPrivateStaticMethod("ForceSetField");
						expressionList.Add(Expression.Call(setMethod!, Expression.Constant(fieldInfo), Expression.Convert(toLocal, typeof(object)), Expression.Convert(call, typeof(object))));
					}
				}
				else
				{
					expressionList.Add(Expression.Assign(Expression.Field(toLocal, fieldInfo), call));
				}
			}
		}

		expressionList.Add(Expression.Convert(toLocal, methodType));

		var funcType = typeof(Func<,,>).MakeGenericType(methodType, typeof(DeepCloneState), methodType);

		var blockParams = new List<ParameterExpression>();
		if (from != fromLocal) blockParams.Add(fromLocal);
		blockParams.Add(toLocal);

		return Expression.Lambda(funcType, Expression.Block(blockParams, expressionList), from, state).Compile();
	}

	private static object GenerateProcessTupleMethod(Type type)
	{
		var from = Expression.Parameter(typeof(object));
		var state = Expression.Parameter(typeof(DeepCloneState));

		var local = Expression.Variable(type);
		var assign = Expression.Assign(local, Expression.Convert(from, type));

		var funcType = typeof(Func<object, DeepCloneState, object>);

		var tupleLength = type.GenericArguments().Length;

		var constructor =
			Expression.Assign(
				local,
				Expression.New(
					type.GetPublicConstructors().First(x => x.GetParameters().Length == tupleLength),
					type.GetPublicProperties()
						.Where(x => x.CanRead && x.Name.StartsWith("Item", StringComparison.Ordinal) && char.IsDigit(x.Name[4]))
						.OrderBy(x => x.Name)
						.Select(x => Expression.Property(local, x.Name))));

		return Expression.Lambda(funcType, Expression.Block(new[] { local },
			assign, constructor, Expression.Call(state, typeof(DeepCloneState).GetMethod(nameof(DeepCloneState.AddKnownRef))!, from, local),
				from),
			from, state).Compile();
	}
}
