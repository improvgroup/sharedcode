// <copyright file="Converter.cs" company="improvGroup, LLC">
//     Copyright © 2009-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Class used to convert returned database records to strongly typed classes
    /// </summary>
    /// <typeparam name="T">The type of the object being converted to.</typeparam>
    internal class DataRecordConverter<T> where T : new()
    {
        /// <summary>
        /// The converter
        /// </summary>
        private readonly Func<IDataRecord, T> converter;

        /// <summary>
        /// The data reader
        /// </summary>
        private readonly IDataRecord dataRecord;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRecordConverter{T}" /> class.
        /// </summary>
        /// <param name="dataRecord">The data record.</param>
        internal DataRecordConverter(IDataRecord dataRecord)
        {
            this.dataRecord = dataRecord;
            this.converter = this.GetMapFunc();
        }

        /// <summary>
        /// Creates a new item from the data record.
        /// </summary>
        /// <returns>The item.</returns>
        internal T ConvertRecordToItem() => this.converter(this.dataRecord);

        /// <summary>
        /// Gets the map function.
        /// </summary>
        /// <returns>The map function.</returns>
        private Func<IDataRecord, T> GetMapFunc()
        {
            var expressions = new List<Expression>();

            var parameterExpression = Expression.Parameter(typeof(IDataRecord), nameof(dataRecord));

            var targetExpression = Expression.Variable(typeof(T));
            expressions.Add(Expression.Assign(targetExpression, Expression.New(targetExpression.Type)));

            // does int based lookup
            var indexerInfo = typeof(IDataRecord).GetProperty("Item", new[] { typeof(int) });

            var columnNames = Enumerable.Range(0, this.dataRecord.FieldCount)
                                        .Select(i => new { i, name = this.dataRecord.GetName(i) });

            foreach (var column in columnNames)
            {
                var property = targetExpression.Type.GetProperty(
                    column.name,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property is null)
                {
                    continue;
                }

                var columnNameExpression = Expression.Constant(column.i);

                var propertyExpression = Expression.MakeIndex(
                    parameterExpression,
                    indexerInfo,
                    new[] { columnNameExpression });

                var propertyValueExpression = Expression.Variable(typeof(object), "propertyValue");

                var convertExpression = Expression.Condition(
                    Expression.Equal(
                        propertyValueExpression,
                        Expression.Constant(DBNull.Value)),
                    Expression.Default(property.PropertyType),
                    Expression.Convert(propertyValueExpression, property.PropertyType));

                var propertyValueReadExpression = Expression.Block(
                    new[] { propertyValueExpression },
                    Expression.Assign(propertyValueExpression, propertyExpression),
                    convertExpression);

                var bindExpression = Expression.Assign(
                    Expression.Property(targetExpression, property),
                    propertyValueReadExpression);

                expressions.Add(bindExpression);
            }

            expressions.Add(targetExpression);
            return Expression.Lambda<Func<IDataRecord, T>>(
                Expression.Block(new[] { targetExpression }, expressions), parameterExpression).Compile();
        }
    }
}
